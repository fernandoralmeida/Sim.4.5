﻿using System;
using System.Net;
using System.IO;

namespace Sim.Update.ViewModel.Functions
{
    class CheckUpdate
    {
        public static bool HasUpdate(string url_ftp)
        {
            //WebClient request = new WebClient();
            //request.Credentials = new NetworkCredential("portalsk", "fra@1705");
            try
            {
                //byte[] newFileData = request.DownloadData(url_ftp);
                //string tempxml = Common.Directories.Temp + Common.Files.Sim_Update_Xml;
                //File.WriteAllBytes(tempxml, newFileData);
                //System.Windows.MessageBox.Show(YesNo(url_ftp).ToString());
                return YesNo(url_ftp);
            }
            catch (WebException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool YesNo(string lastversionfile)
        {
            if (CurrentUpdate < LastUpdate(lastversionfile))
                return true;
            else
                return false;
        }

        private static int LastUpdate(string filepath)
        {
            return Convert.ToInt32(Xml.Read(filepath)[0]);
        }

        private static int CurrentUpdate
        {
            get
            {
                string path = Common.Directories.CurrentApp + @"\" + Common.Files.Sim_Update_Xml;

                if (File.Exists(path))
                    return Convert.ToInt32(Xml.Read(path)[0]);
                else
                    return 0;
            }
        }

    }
}
