using Common.DbTools;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontrolerSistema
{
    public static class Data
    {
        public static DbLocalController Dblc = new DbLocalController();
        public static DbGenerator Dbg = new DbGenerator();
        public static DbSystemController Dbsc = new DbSystemController();
        public static List<LocalController> LocalControllers = new List<LocalController>();
        public static List<Generator> LocalGenerators = new List<Generator>();
        public static List<Generator> RemoteGenerators = new List<Generator>();
        public static List<string> LCCodes = new List<string>();
        public static bool RunningFlag = true;
        public static double RequiredPower = 0;
        public static double CurrentPowerGenerated = 0;
    }
}
