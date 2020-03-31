using Common.DbTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_KontrolerSistema
{
    public static class Data
    {
        public static DbLocalController Dblc = new DbLocalController();
        public static DbSystemController Dbsc = new DbSystemController();
        public static DbGenerator dbgen = new DbGenerator();
        public static bool RunningFlag = true;
        public static string DateFormat = "dd-MM-yyyy";
    }
}
