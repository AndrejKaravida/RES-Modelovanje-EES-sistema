using Common.Enums;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KontrolerSistema
{
    public static class PowerSimulator
    {
        public static void Run()
        {
            while (Data.RunningFlag)
            {
                Data.LocalGenerators = Data.Dbg.ReadAllLocal().Where(g => g.State == EState.ONLINE).ToList();
                Data.RemoteGenerators = Data.Dbg.ReadAllRemote().Where(g => Data.Dblc.IsOnline(g.LCCode)).OrderBy(g => g.WorkPrice).ThenBy(g => g.MinPower).ToList();

                double requiredPower = Data.Dbsc.GetRequiredPower();
                double requiredPowerLeft = requiredPower;
                double currentPowerGenerated = 0;

                foreach (Generator g in Data.LocalGenerators)
                {
                    requiredPowerLeft -= g.CurrentPower;
                    currentPowerGenerated += g.CurrentPower;
                }

                Generator tmp;
                double powerSetting = 0;

                if (requiredPowerLeft > 0)
                {
                    foreach (Generator g in Data.RemoteGenerators)
                    {
                        if (requiredPowerLeft > 0)
                        {
                            if (requiredPowerLeft >= g.MaxPower)
                            {
                                powerSetting = g.MaxPower;
                                requiredPowerLeft -= powerSetting;
                                currentPowerGenerated += powerSetting;
                            }
                            else if (requiredPowerLeft < g.MaxPower && requiredPowerLeft >= g.MinPower)
                            {
                                powerSetting = requiredPowerLeft;
                                requiredPowerLeft = 0;
                                currentPowerGenerated += powerSetting;
                            }
                            else
                            {
                                powerSetting = g.MinPower;
                                requiredPowerLeft -= powerSetting;
                                currentPowerGenerated += powerSetting;
                            }
                        }
                        else if (requiredPowerLeft <= 0)
                        {
                            powerSetting = 0;
                        }

                        SendSetpoints(g.Id, powerSetting);

                        if (requiredPowerLeft < 0)
                        {
                            do
                            {
                                tmp = Data.RemoteGenerators
                                    .Where(gen => Data.Dblc.IsOnline(gen.LCCode))
                                    .OrderByDescending(gen => gen.WorkPrice)
                                    .FirstOrDefault(gen => gen.CurrentPower == gen.MaxPower);

                                if (tmp != null)
                                {
                                    if ((tmp.MaxPower - tmp.MinPower) >= Math.Abs(requiredPowerLeft))
                                    {
                                        powerSetting = tmp.CurrentPower;
                                        powerSetting += requiredPowerLeft;
                                        currentPowerGenerated -= Math.Abs(requiredPowerLeft);
                                        requiredPowerLeft = 0;
                                    }
                                    else
                                    {
                                        requiredPowerLeft += tmp.CurrentPower - tmp.MinPower;
                                        currentPowerGenerated -= tmp.CurrentPower - tmp.MinPower;
                                        powerSetting = tmp.MinPower;
                                    }

                                    SendSetpoints(tmp.Id, powerSetting);
                                }
                                else
                                {
                                    requiredPowerLeft = 0;
                                }
                            } while (requiredPowerLeft < 0);
                        }
                    }
                }

                Thread.Sleep(10000);
            }
        }

        public static void SendSetpoints(int genId, double powerSetting)
        {
            Generator g = Data.Dbg.GetGenerator(genId);

            if(g.Setpoints.Count == 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    g.Setpoints[i].Value = powerSetting;
                    g.Setpoints[i].Date = DateTime.Now.AddSeconds(10 * i);
                }
            }
            
            Data.Dbsc.SaveSetpoints(g);
        }
    }
}
