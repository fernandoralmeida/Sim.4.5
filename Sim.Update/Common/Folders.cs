using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Update.Common
{
    public class Folders
    {
        public static string Sim_Install
        {
            get { return @"SIM"; }
        }

        public static string AppData_Sim
        {
            get { return Directories.AppData + @"\" + Sim_Install + @"\"; }
        }
    }
}
