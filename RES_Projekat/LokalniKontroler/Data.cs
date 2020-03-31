using Common.DbTools;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokalniKontroler
{
    public static class Data
    {
        public static DbGenerator Dbg = new DbGenerator();
        public static DbLocalController Dblc = new DbLocalController();
        public static string Code;
        public static List<Generator> Generators = new List<Generator>();
        public static bool RunningFlag = true;
    }
}
