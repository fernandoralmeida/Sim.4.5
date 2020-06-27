using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Publisher.ViewModel
{
    class Folders
    {
        public static string LocalInstaller
        {
            get { return @"Sim.Install.Local"; }
        }

        public static string Publish
        {
            get { return @"Published"; }
        }

        public static string Temp
        {
            get { return @"Temp"; }
        }
    }
}
