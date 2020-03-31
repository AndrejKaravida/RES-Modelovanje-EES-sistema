using Common.Enums;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LokalniKontroler
{
    public static class LocalPowerSimulator
    {
        public static Random r = new Random();

        public static void SimulateLocal()
        {
            double added = 0;
            double subbed = 0;

            while (Data.RunningFlag)
            {
                foreach (Generator g in Data.Generators.Where(gen => gen.Control == EControl.LOCAL))
                {
                    added = g.CurrentPower + g.CurrentPower * 0.1;
                    subbed = g.CurrentPower - g.CurrentPower * 0.1;

                    if ((r.Next(0, 2) == 0 || added > g.MaxPower) && subbed >= g.MinPower)
                    {
                        g.CurrentPower = subbed;
                    }
                    else if (added <= g.MaxPower)
                    {
                        g.CurrentPower = added;
                    }
                }

                Data.Dbg.UpdatePower(Data.Generators.Where(gen => gen.Control == EControl.LOCAL).ToList());

                Thread.Sleep(2000);
            }
        }

        public static void RunRemote()
        {
            Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
            
            while (Data.RunningFlag)
            {
                foreach (Generator g in Data.Generators.Where(gen => gen.Control == EControl.REMOTE))
                {
                    if (!threads.ContainsKey(g.Id))
                    {
                        threads.Add(g.Id, new Thread(() => SetpointGeneratorUpdater(g.Id)));
                        threads[g.Id].Start();
                    }
                    else if(!threads[g.Id].IsAlive)
                    {
                        threads.Remove(g.Id);
                    }
                }

                Thread.Sleep(1000);
            }

            foreach(KeyValuePair<int, Thread> t in threads)
            {
                t.Value.Join();
            }
        }

        private static void SetpointGeneratorUpdater(int genId)
        {
            Generator g;
            DateTime now;
            TimeSpan dueTime;
            int i;
            bool found;

            while (Data.RunningFlag)
            {
                g = Data.Dbg.GetGenerator(genId);

                if (g.Control == EControl.LOCAL)
                {
                    break;
                }

                if (g.Setpoints.Count > 0)
                {
                    now = DateTime.Now;
                    i = 0;
                    found = false;

                    g.Setpoints.ForEach(sp =>
                    {
                        if (sp.Date > now)
                        {
                            found = true;
                            return;
                        }

                        i++;
                    });

                    if (found)
                    {
                        dueTime = g.Setpoints[i].Date - now;
                        Thread.Sleep(dueTime);

                        if (Data.Dbg.GetGenerator(genId).Control == EControl.LOCAL)
                        {
                            break;
                        }

                        if (g.State == EState.OFFLINE)
                        {
                            g.State = EState.ONLINE;
                        }

                        g.CurrentPower = g.Setpoints[i].Value;
                        Data.Dblc.SaveChanges(g);
                    }
                    else
                    {
                        g.CurrentPower = 0;
                        g.State = EState.OFFLINE;
                        Data.Dblc.SaveChanges(g);
                        Thread.Sleep(2000);
                    }
                }
                else
                {
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
