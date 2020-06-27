using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;

namespace Sim.Update.ViewModel.Functions
{
    class Zip
    {
        public delegate void ProgressDelegate(string message);

        public static bool File(string sDir, GZipStream zipStream, ProgressDelegate progress)
        {
            //Decompress file name
            byte[] bytes = new byte[sizeof(int)];
            int Readed = zipStream.Read(bytes, 0, sizeof(int));
            if (Readed < sizeof(int))
                return false;

            int iNameLen = BitConverter.ToInt32(bytes, 0);
            bytes = new byte[sizeof(char)];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iNameLen; i++)
            {
                zipStream.Read(bytes, 0, sizeof(char));
                char c = BitConverter.ToChar(bytes, 0);
                sb.Append(c);
            }
            string sFileName = sb.ToString();
            if (progress != null)
                progress(sFileName);

            //Decompress file content
            bytes = new byte[sizeof(int)];
            zipStream.Read(bytes, 0, sizeof(int));
            int iFileLen = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[iFileLen];
            zipStream.Read(bytes, 0, bytes.Length);

            string sFilePath = Path.Combine(sDir, sFileName);
            string sFinalDir = Path.GetDirectoryName(sFilePath);
            if (!System.IO.Directory.Exists(sFinalDir))
                System.IO.Directory.CreateDirectory(sFinalDir);

            if (sFileName != "Sim.Updater.exe")
            {
                //System.Windows.MessageBox.Show(sFileName);

                using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    outFile.Write(bytes, 0, iFileLen);

            }
            return true;
        }

        public static void ToDirectory(string sCompressedFile, string sDir)
        {
            using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
            using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                while (File(sDir, zipStream, null))
                {
                    //progress.ReportProgress((int)((inFile.Position * 100) / inFile.Length));
                }
        }
    }
}
