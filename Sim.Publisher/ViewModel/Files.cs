using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Publisher.ViewModel
{
    class Files
    {
        public static string Sim_Exe_Name
        {
            get { return @"Sim.App.exe"; }
        }

        public static string Sim_Exec_Path
        {
            get
            {
                return Directories.Sim_Dir + Sim_Exe_Name;
            }
        }

        public static string Sim_Package_Name
        {
            get; set;
        }

        public static string Sim_Install_Name
        {
            get; set;
        }
    }
}
