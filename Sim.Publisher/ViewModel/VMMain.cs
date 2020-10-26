using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Sim.Publisher.ViewModel
{

    using Common;

    class VMMain : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BackgroundWorker bgWorker = new BackgroundWorker();
        private BackgroundWorker bgWorkerUpload = new BackgroundWorker();

        private int _pgbValue;
        private string _fileName;
        private int _packProgress;
        private string _packName;
        int valor;

        public VMMain()
        {
            try
            {
                if (!System.IO.Directory.Exists(Directories.Publisher))
                    System.IO.Directory.CreateDirectory(Directories.Publisher);

                bgWorker.WorkerSupportsCancellation = true;
                bgWorker.WorkerReportsProgress = true;
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

                bgWorkerUpload.WorkerSupportsCancellation = true;
                bgWorkerUpload.WorkerReportsProgress = true;
                bgWorkerUpload.DoWork += BgWorkerUpload_DoWork;
                bgWorkerUpload.ProgressChanged += BgWorkerUpload_ProgressChanged;
                bgWorkerUpload.RunWorkerCompleted += BgWorkerUpload_RunWorkerCompleted;

                valor = Convert.ToInt32(Version.Listar("sim_update", "System", "Version", "Update")[0]);
                Version.Major(System.Reflection.AssemblyName.GetAssemblyName(Files.Sim_Exec_Path).Version.Major.ToString());
                Version.Minor(System.Reflection.AssemblyName.GetAssemblyName(Files.Sim_Exec_Path).Version.Minor.ToString());
                Version.Build(System.Reflection.AssemblyName.GetAssemblyName(Files.Sim_Exec_Path).Version.Build.ToString());
                Version.Revision(System.Reflection.AssemblyName.GetAssemblyName(Files.Sim_Exec_Path).Version.Revision.ToString());
                FileName = "Transferir";
                PackName = "Compactar";
                PublisherName = "Publicar";
            }
            catch { }
        }

        public List<string> Listar
        {
            get
            {
                return System.IO.Directory.GetFiles(Directories.Sim_Dir, @"*.*", System.IO.SearchOption.AllDirectories).ToList();
            }
        }

        public ICommand CreatePackageCommand => new RelayCommand(p => 
        {
            bgWorker.RunWorkerAsync();
        });

        public int PgbValue
        {
            get { return _pgbValue; }
            set
            {
                if (_pgbValue != value)
                {
                    _pgbValue = value;
                    RaisePropertyChanged("PgbValue");
                }
            }
        }

        public int PackProgress
        {
            get { return _packProgress; }
            set
            {
                if (_packProgress != value)
                {
                    _packProgress = value;
                    RaisePropertyChanged("PackProgress");
                }
            }
        }

        private int _uploadProgress;
        public int UploadProgress
        {
            get { return _uploadProgress; }
            set
            {
                if (_uploadProgress != value)
                {
                    _uploadProgress = value;
                    RaisePropertyChanged("UploadProgress");
                }
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    RaisePropertyChanged("FileName");
                }
            }
        }

        public string PackName
        {
            get { return _packName; }
            set
            {
                if (_packName != value)
                {
                    _packName = value;
                    RaisePropertyChanged("PackName");
                }
            }
        }

        private string _publisherName;
        public string PublisherName
        {
            get { return _publisherName; }
            set
            {
                if (_publisherName != value)
                {
                    _publisherName = value;
                    RaisePropertyChanged("PublisherName");
                }
            }
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //string _sim_Temp = 
                string _root = string.Format(@"{0}\{1}", Directories.SimTEMP, Folders.Temp);
                string _publish = string.Format(@"{0}\{1}", Directories.SimTEMP, Folders.Publish);
                string _local = string.Format(@"{0}\{1}", Directories.SimTEMP, Folders.LocalInstaller);

                if (!System.IO.Directory.Exists(_root))
                    System.IO.Directory.CreateDirectory(_root);
                /*
                if (!System.IO.Directory.Exists(_cache))
                    System.IO.Directory.CreateDirectory(_cache);
                    *//*
                if (!System.IO.Directory.Exists(_xml))
                    System.IO.Directory.CreateDirectory(_xml);                
                
                if (!System.IO.Directory.Exists(_database))
                    System.IO.Directory.CreateDirectory(_database);
                
                if (!System.IO.Directory.Exists(_wallpaper))
                    System.IO.Directory.CreateDirectory(_wallpaper);
                    */
                int progresso = 0;
                int contador = 1;

                if (Listar.Count > 0)
                {
                    foreach (string file in Listar)
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);

                        if (fi.Extension == ".dll")
                            System.IO.File.Copy(file, _root + @"\" + fi.Name, true);

                        if (fi.Name == Files.Sim_Exe_Name)
                            System.IO.File.Copy(file, _root + @"\" + fi.Name, true);

                        System.IO.File.Copy(string.Format(@"{0}\source\repos\fernandoralmeida\Sim.4.5\Sim.Update\bin\Release\{1}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Sim.Updater.exe"), _root + @"\Sim.New.Updater.exe", true);

                        if (fi.Extension == ".ico")
                            System.IO.File.Copy(file, _root + @"\" + fi.Name, true);

                        if (fi.Extension == ".xml")
                        {
                            if (fi.Name == "sim_update.xml")
                                System.IO.File.Copy(file, _root + @"\" + fi.Name, true);
                        }
                        /*
                        if (fi.Extension == ".mdb")
                            System.IO.File.Copy(file, _database + @"\" + fi.Name, true);
                        
                        if (fi.Extension == ".jpg")
                            System.IO.File.Copy(file, _wallpaper + @"\" + fi.Name, true);
                            */
                        if (fi.Extension == ".config")
                            if (fi.Name == "Sim.App.exe.config")
                                System.IO.File.Copy(file, _root + @"\" + fi.Name, true);

                        System.Threading.Thread.Sleep(10);

                        FileName = "Transferindo " + fi.Name;
                        progresso = (contador * 100) / Listar.Count;
                        bgWorker.ReportProgress(progresso);
                        contador++;
                    }

                }

                //System.IO.File.Copy(string.Format(@"{0}\CSharp\Projetos\Sim\Pdf\{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "libmupdf.dll"), _root + @"\libmupdf.dll", true);

                contador = 1;
                FileName = "Transferência Finalizada...";

                if (!System.IO.Directory.Exists(_publish))
                    System.IO.Directory.CreateDirectory(_publish);

                if (!System.IO.Directory.Exists(_local))
                    System.IO.Directory.CreateDirectory(_local);

                string[] sFiles = System.IO.Directory.GetFiles(_root, "*.*", System.IO.SearchOption.AllDirectories);
                int iDirLen = _root[_root.Length - 1] == System.IO.Path.DirectorySeparatorChar ? _root.Length : _root.Length + 1;

                valor++;

                Files.Sim_Package_Name = @"\sim_update.gz"; //@"\sim_build_" + System.Reflection.AssemblyName.GetAssemblyName(Files.Sim_Exec_Path).Version.Build.ToString() + "_update_" + valor + @".gz";

                using (System.IO.FileStream outFile = new System.IO.FileStream(_publish + Files.Sim_Package_Name, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                using (System.IO.Compression.GZipStream str = new System.IO.Compression.GZipStream(outFile, System.IO.Compression.CompressionMode.Compress))
                    foreach (string sFilePath in sFiles)
                    {

                        System.IO.FileInfo fi = new System.IO.FileInfo(sFilePath);

                        string sRelativePath = sFilePath.Substring(iDirLen);
                        PackName = "Incluindo " + sRelativePath;
                        System.Threading.Thread.Sleep(10);

                        if (fi.Extension == ".jpg" || fi.Extension == ".ico" || fi.Extension == ".exe" || fi.Extension == ".dll" || fi.Name == "sim_update.xml" || fi.Extension == ".config")
                            Zip.CompressFile(_root, sRelativePath, str);

                        PackProgress = (contador * 100) / sFiles.Count();
                        contador++;
                    }

                sFiles = System.IO.Directory.GetFiles(_root, "*.*", System.IO.SearchOption.AllDirectories);

                Files.Sim_Install_Name = @"\sim_install.gz";

                using (System.IO.FileStream outFile = new System.IO.FileStream(_publish + Files.Sim_Install_Name, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                using (System.IO.Compression.GZipStream str = new System.IO.Compression.GZipStream(outFile, System.IO.Compression.CompressionMode.Compress))
                    foreach (string sFilePath in sFiles)
                    {
                        string sRelativePath = sFilePath.Substring(iDirLen);
                        PackName = "Incluindo " + sRelativePath;
                        System.Threading.Thread.Sleep(10);
                        Zip.CompressFile(_root, sRelativePath, str);
                        PackProgress = (contador * 100) / sFiles.Count();
                        contador++;
                    }

                //System.IO.File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"\sim_update.xml", _publish + @"\sim_update.xml", true);
                System.IO.File.Copy(string.Format(@"{0}\source\repos\fernandoralmeida\Sim.4.5\Sim.Update\bin\Release\{1}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Sim.Updater.exe"), _publish + @"\Sim.Installer.exe", true);
                System.IO.File.Copy(string.Format(@"{0}\source\repos\fernandoralmeida\Sim.4.5\Sim.Update\bin\Release\{1}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Sim.Updater.exe"), _local + @"\Sim.Installer.exe", true);
                System.IO.File.Copy(string.Format(@"{0}{1}", _publish, Files.Sim_Install_Name), _local + @"\sim_install.gz", true);
                System.IO.File.Copy(string.Format(@"{0}{1}", _publish, Files.Sim_Package_Name), _local + @"\sim_update.gz", true);
                Version.Update(valor.ToString());
                System.IO.File.Copy(string.Format(@"{0}\{1}", _publish, "sim_update.xml"), _local + @"\sim_update.xml", true);
                //System.IO.Directory.Delete(_root, true);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                System.Windows.MessageBox.Show(ex.Message,
                    "Sim.Publisher",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PgbValue = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PackName = "Compactação Finalizada...";

            if (System.Windows.MessageBox.Show("Instalador gerado com sucesso!\nDeseja Publicar?",
                "Sim.App.Installer",
                System.Windows.MessageBoxButton.OKCancel,
                System.Windows.MessageBoxImage.Information) == System.Windows.MessageBoxResult.OK)
                bgWorkerUpload.RunWorkerAsync();

        }

        private void BgWorkerUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            //new Ftp().UploadFileFTP(Folders.Publish_Path + @"\sim_update.xml", Directories.Sim_FTP_Install + @"sim_update.xml", bgWorkerUpload);
            //new Ftp().UploadFileFTP(Folders.Publish_Path + @"\Sim.App.Installer.exe", Directories.Sim_FTP_Files + @"Sim.App.Installer.exe", bgWorkerUpload);
            //new Ftp().UploadFileFTP(Folders.Publish_Path + @"\" + Files.Sim_Package_Name, Directories.Sim_FTP_Install + Files.Sim_Package_Name, bgWorkerUpload);
            //new Ftp().UploadFileFTP(Folders.Publish_Path + "\\" + Files.Sim_Install_Name, Directories.Sim_FTP_Install + Files.Sim_Install_Name, bgWorkerUpload);

            System.IO.File.Copy(string.Format(@"{0}\source\repos\fernandoralmeida\Sim.4.5\Sim.Update\bin\Release\{1}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Sim.Updater.exe"), Directories.Net_Work_Path + @"\Sim.Installer.exe", true);
            System.IO.File.Copy(string.Format(@"{0}{1}", Directories.Publisher, Files.Sim_Install_Name), Directories.Net_Work_Path + @"\sim_install.gz", true);
            System.IO.File.Copy(string.Format(@"{0}{1}", Directories.Publisher, Files.Sim_Package_Name), Directories.Net_Work_Path + @"\sim_update.gz", true);
            System.IO.File.Copy(string.Format(@"{0}\{1}", Directories.Publisher, "sim_update.xml"), Directories.Net_Work_Path + @"\sim_update.xml", true);
        }

        private void BgWorkerUpload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UploadProgress = e.ProgressPercentage;
            PublisherName = string.Format("Publicando... {0}%", e.ProgressPercentage);
        }

        private void BgWorkerUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PublisherName = "Publicação Finalizada...";
            System.Windows.MessageBox.Show("Publicado com sucesso!", "Sim.App.Publisher", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            System.Windows.Application.Current.Shutdown();
        }
    }
}
