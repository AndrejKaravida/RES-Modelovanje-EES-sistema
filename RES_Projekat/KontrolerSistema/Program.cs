using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KontrolerSistema
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Data.Dbsc.GetSystemController() == null)
            {
                Data.Dbsc.Create();
            }

            Thread refresher = new Thread(Refresh);
            refresher.Start();

            Thread simulator = new Thread(PowerSimulator.Run);
            simulator.Start();

            Console.WriteLine("Pres Return key to exit...");
            Console.ReadLine();
            Console.WriteLine("Shutting down...");

            Data.RunningFlag = false;

            refresher.Join();
            simulator.Join();
        }

        static void Refresh()
        {
            double totalPower;

            while (Data.RunningFlag)
            {
                Data.LocalControllers = Data.Dblc.ReadAll();
                totalPower = 0;

                for (int i = 0; i < Data.LocalControllers.Count; i++)
                {
                    foreach (Generator g in Data.Dbg.ReadAllForLC(Data.LocalControllers[i].Code))
                    {
                        totalPower += g.CurrentPower;
                    }

                    if (!Data.LCCodes.Contains(Data.LocalControllers[i].Code))
                    {
                        Console.WriteLine($"Local Controller {Data.LocalControllers[i].Code} has just connected.");
                        Data.LCCodes.Add(Data.LocalControllers[i].Code);
                    }
                }

                for (int i = 0; i < Data.LCCodes.Count; i++)
                {
                    if (!Data.LocalControllers.Any(lc => lc.Code == Data.LCCodes[i]))
                    {
                        Console.WriteLine($"Local Controller {Data.LCCodes[i]} has just disconnected.");
                        Data.LCCodes.Remove(Data.LCCodes[i]);
                    }
                }

                Data.Dbsc.SetTotalPower(totalPower);
                Thread.Sleep(500);
            }
        }
    }
}
