using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LokalniKontroler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("****** LOCAL CONTROLLER STARTUP WIZARD ******");
            Console.WriteLine("If you wish to connect to an existing controller, enter the corresponding code.");
            Console.WriteLine("If you wish to create a new controller, enter a new code.");
            Console.Write("INPUT: ");

            Data.Code = string.Empty;

            while ((Data.Code = Console.ReadLine()) == string.Empty)
            {
                Console.WriteLine("Connection string can't be empty!");
                Console.Write("INPUT: ");
            }

            Data.Dblc.Connect(Data.Code);

            Console.WriteLine("Local controller " + Data.Code + " successfully connected!");
            Data.Dblc.TurnOnLCGenerators(Data.Code);


            Thread refresher = new Thread(Refresh);
            refresher.Start();

            Thread simulator = new Thread(LocalPowerSimulator.SimulateLocal);
            simulator.Start();

            Thread remote = new Thread(LocalPowerSimulator.RunRemote);
            remote.Start();

            Console.WriteLine("Press Return key to exit...");
            Console.ReadLine();

            Console.WriteLine("Shutting down...");

            Data.RunningFlag = false;

            remote.Join();
            simulator.Join();
            refresher.Join();

            Data.Dblc.ShutDownGenerators(Data.Code);
            Data.Dblc.ShutDown(Data.Code);
        }


        static void Refresh()
        {
            while (Data.RunningFlag)
            {
                Data.Generators = Data.Dbg.ReadAllForLC(Data.Code);
                Thread.Sleep(1000);
            }
        }
    }
}
