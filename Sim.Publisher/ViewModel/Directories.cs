using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Publisher.ViewModel
{
    class Directories
    {

        public static string Sim_Dir
        {
            get
            {
                return string.Format(@"{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"\source\repos\fernandoralmeida\Sim.4.5\Sim\bin\Release\");
                //return string.Format(@"{0}{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"\CSharp\Projetos\Sim.4.5\Sim\bin\Release\");
            }
        }

        public static string SimTEMP
        {
            get { return System.IO.Path.GetTempPath() + "\\" + "SIM_Temp"; }
        }

        public static string Desktop
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        }

        public static string Net_Work_Path
        {
            get { return @"\\servidor\Sim"; }
            //get { return @"E:\Sim\"; }
        }

        public static string Publisher
        {
            get { return Directories.SimTEMP + "\\" + Folders.Publish; }
        }

    }
}
