using System;
using System.IO;

namespace Sim.Update.ViewModel.Functions
{
    class Dir
    {
        private static bool DirExists(string path)
        {
            if (System.IO.Directory.Exists(path))
                return true;
            else
                return false;
        }

        public static void CreateDir(string path)
        {
            if (DirExists(path) == false)
                System.IO.Directory.CreateDirectory(path);
            //else
            //ClearDir(path);
        }

        public static bool ClearDir(string pathfile)
        {
            // verifica se o caminho exite
            if (System.IO.Directory.Exists(pathfile))
            {
                string[] nfiles = System.IO.Directory.GetFiles(pathfile);

                //deleta um arquivo de cada vez
                foreach (string files in nfiles)
                {
                    string arquivo = System.IO.Path.GetFullPath(files);
                    if (arquivo != Common.Folders.AppData_Sim + "\\" + AppDomain.CurrentDomain.FriendlyName)
                        System.IO.File.Delete(arquivo);
                }

                return true;
            }
            else
                return false;
        }

        public static bool NewClearDir(string dir_path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(dir_path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    if (dir.Name != "xml" && dir.Name != "DataBase")
                        dir.Delete(true);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
