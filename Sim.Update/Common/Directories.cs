using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Update.Common
{
    public class Directories
    {
        public static string CurrentApp
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string Documents
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
        }

        public static string AppData
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); }
        }

        public static string Desktop
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        }

        public static string Temp
        {
            get { return System.IO.Path.GetTempPath(); }
        }
        public static string Url_Http
        {
            get { return @"http://www.portalsk.com/sys/sim/"; }
        }

        public static string Ftp_Update
        {
            get { return @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/install/new/"; }
        }

        public static string Url_Ftp_Log
        {
            get { return @"ftp://ftp.portalsk.com/portalsk.com/web/sys/sim/"; }
        }

        public static string Net_Work_Path
        {
            //get { return @"E:\Sim\"; }
            get { return @"\\sec-desen\Sim\"; }
        }
    }
}
