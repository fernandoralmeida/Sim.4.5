using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.IO;

namespace Sim.ViewModel
{

    using Mvvm.Commands;
    using Controls.Functions;
    using Controls.ViewModels;

    class vmToolsDB : VMBase, IDialogBox
    {
        #region Declarations

        private bool _inbackup = false;
        private bool _inrestore = false;
        private bool _indelete = false;

        private bool _onnuvem;

        private string _connectionstring;
        private string _backuppath;
        private string _selecteditem;
        
        private ICommand _commandsavestringdb;
        private ICommand _commandbackup;
        private ICommand _commanddelbeckup;
        private ICommand _commandrestorebeckup;
        private ICommand _commanddir;

        private IEnumerable<string> _backuplist;
        #endregion

        #region Properties
        public IEnumerable<string> BackupList
        {
            get { return _backuplist; }
            set
            {
                _backuplist = value;
                RaisePropertyChanged("BackupList");
            }
        }

        public bool OnNuvem
        {
            get { return _onnuvem; }
            set
            {
                _onnuvem = value;
                RaisePropertyChanged("OnNuvem");
            }
        }

        public string ConnectionString
        {
            get { return _connectionstring; }
            set
            {
                _connectionstring = value;
                RaisePropertyChanged("ConnectionString");
            }
        }

        public string BackupPath
        {
            get { return _backuppath; }
            set
            {
                _backuppath = value;
                RaisePropertyChanged("BackupPath");
            }
        }

        public string SelectedItem
        {
            get { return _selecteditem; }
            set
            {
                _selecteditem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandCommandSaveString
        {
            get
            {
                if (_commandsavestringdb == null)
                    _commandsavestringdb = new RelayCommand(p =>
                      {
                          DataBase.Properties.Settings.Default.ConnectionString = ConnectionString;
                          DataBase.Properties.Settings.Default.Save();// = ConnectionString;
                      });

                return _commandsavestringdb;
            }
        }

        public ICommand CommandDir
        {
            get
            {
                if (_commanddir == null)
                    _commanddir = new RelayCommand(p =>
                      {

                          if (Properties.Settings.Default.BackupDB == "..."
                          || Properties.Settings.Default.BackupDB == null
                          || Properties.Settings.Default.BackupDB == string.Empty)
                              BackupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                          if (!System.IO.Directory.Exists(BackupPath + @"\sim_backups"))
                          {
                              System.IO.Directory.CreateDirectory(BackupPath + @"\sim_backups");
                              string t = BackupPath + @"\sim_backups";
                              //t = t.Replace(@"\\", @"\");
                              BackupPath = t;
                          }

                          Properties.Settings.Default.BackupDB = BackupPath;
                          Properties.Settings.Default.Save();
                      });
                return _commanddir;
            }
        }

        public ICommand CommandBackup
        {
            get
            {
                if (_commandbackup == null)
                    _commandbackup = new RelayCommand(p=>
                    {
                        _inbackup = true;
                        _indelete = false;
                        _inrestore = false;
                        SyncDialogBox("Fazer BACKUP da Base de Dados?", DialogBoxColor.Blue);                        
                    });
                return _commandbackup;
            }
        }

        public ICommand CommandRestore
        {
            get
            {
                if (_commandrestorebeckup == null)
                    _commandrestorebeckup = new RelayCommand(p =>
                      {
                          _inbackup = false;
                          _indelete = false;
                          _inrestore = true;
                          SyncDialogBox("Restaurar BACKUP da Base de Dados?", DialogBoxColor.Green);
                      });

                return _commandrestorebeckup;
            }
        }

        public ICommand CommandDelete
        {
            get
            {
                if (_commanddelbeckup == null)
                    _commanddelbeckup = new RelayCommand(p =>
                    {
                        _inbackup = false;
                        _indelete = true;
                        _inrestore = false;
                        SyncDialogBox("Apagar BACKUP da Base de Dados?", DialogBoxColor.Red);
                    });

                return _commanddelbeckup;
            }
        }

        public ICommand CommandMsgYes => new RelayCommand(p =>
        {
            ViewDialogBox = Visibility.Collapsed;

            if (_inbackup) Backup();

            if (_indelete) Delete();

            if (_inrestore) Restore();
        });

        public ICommand CommandMsgNot => new RelayCommand(p =>
        {
            ViewDialogBox = Visibility.Collapsed;
        });
        #endregion

        #region Constructor
        public vmToolsDB()
        {
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            ViewMessageBox = Visibility.Collapsed;
            ViewDialogBox = Visibility.Collapsed;
            OnNuvem = false;
            ConnectionString = DataBase.Properties.Settings.Default.ConnectionString;
            BackupPath = Properties.Settings.Default.BackupDB;
            ListarBackups();
        }
        #endregion

        #region Functions

        /// <summary>
        /// chama função assincrona
        /// </summary>
        private async void Backup()
        {
            if (await GerarBackup())
                AsyncMessageBox("Backup realizado com sucesso!", DialogBoxColor.Green, false);
            else
                AsyncMessageBox("Erro, backup não realizado!", DialogBoxColor.Red, false);

            ListarBackups();
        }

        /// <summary>
        /// Gera backup em segundo plano
        /// </summary>
        /// <returns></returns>
        private Task<bool> GerarBackup()
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;

            return Task<bool>.Factory.StartNew(() => 
            {


                string backupfolder = AppDomain.CurrentDomain.BaseDirectory + @"backup\";
                string uploadfolder = AppDomain.CurrentDomain.BaseDirectory + @"upload\";

                try
                {
                    //System.IO.Directory.Delete(_root, true);

                    //ZipUnzip.ProgressDelegate txt;

                    string file = @"" +
                        DateTime.Now.Year.ToString("0000") +
                        DateTime.Now.Month.ToString("00") +
                        DateTime.Now.Day.ToString("00") +
                        "_" +
                        DateTime.Now.Hour.ToString("00") +
                        DateTime.Now.Minute.ToString("00") +
                        DateTime.Now.Second.ToString("00") +
                        ".gz";

                    string backupfile = uploadfolder + file;

                    string db1file = ConnectionString + "SimDataBase1.mdb";
                    string dbNfile = ConnectionString + "SimDataBase1_N.mdb";
                    string db2file = ConnectionString + "SimDataBase2.mdb";
                    string db3file = ConnectionString + "SimDataBase3.mdb";
                    string db4file = ConnectionString + "SimDataBase4.mdb";

                    if (ConnectionString == @"|DataDirectory|\DataBase\")
                    {
                        db1file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase1.mdb";
                        dbNfile = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase1_N.mdb";
                        db2file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase2.mdb";
                        db3file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase3.mdb";
                        db4file = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\SimDataBase4.mdb";
                    }

                    if (!System.IO.Directory.Exists(backupfolder))
                        System.IO.Directory.CreateDirectory(backupfolder);

                    if (!System.IO.Directory.Exists(uploadfolder))
                        System.IO.Directory.CreateDirectory(uploadfolder);

                    System.IO.File.Copy(db1file, backupfolder + "SimDataBase1.mdb", true);
                    System.IO.File.Copy(dbNfile, backupfolder + "SimDataBase1_N.mdb", true);
                    System.IO.File.Copy(db2file, backupfolder + "SimDataBase2.mdb", true);
                    System.IO.File.Copy(db3file, backupfolder + "SimDataBase3.mdb", true);
                    System.IO.File.Copy(db4file, backupfolder + "SimDataBase4.mdb", true);

                    //MessageBox.Show(uploadfolder);

                    Compress.CompressDirectory(backupfolder, backupfile, null);

                    System.IO.File.Copy(backupfile, BackupPath + @"\" + file, true);

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception(ex.Message);
                    
                }
                finally
                {
                    System.IO.Directory.Delete(backupfolder, true);
                    System.IO.Directory.Delete(uploadfolder, true);
                    StartProgress = false;
                    BlackBox = Visibility.Collapsed;
                }

            });
        }

        private async void Delete()
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            var t = Task.Run(() =>
            {
                System.IO.File.Delete(BackupPath + @"\" + SelectedItem);
            });

            await t;

            StartProgress = false;
            BlackBox = Visibility.Collapsed;

            if (t.IsCompleted)
                if (!System.IO.File.Exists(BackupPath + @"\" + SelectedItem))
                    AsyncMessageBox("Backup apagado!", DialogBoxColor.Green, false);
                else
                    AsyncMessageBox("Backup NÃO apagado!", DialogBoxColor.Red, false);

            ListarBackups();

        }

        private async void Restore()
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            var t = Task.Run(() =>
            {
                string downloadfolder = AppDomain.CurrentDomain.BaseDirectory + @"download\";

                try
                {
                    string backupfile = downloadfolder + SelectedItem;

                    if (!System.IO.Directory.Exists(downloadfolder))
                        System.IO.Directory.CreateDirectory(downloadfolder);

                    string restorefolder = ConnectionString;

                    if (ConnectionString == @"|DataDirectory|\DataBase\")
                    {
                        restorefolder = AppDomain.CurrentDomain.BaseDirectory + @"DataBase\";
                    }

                    System.IO.File.Copy(BackupPath + @"\" + SelectedItem, backupfile, true);
                    Compress.DecompressDirectory(BackupPath + @"\" + SelectedItem, restorefolder, null);

                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate {

                        AsyncMessageBox(ex.Message, DialogBoxColor.Red, false);

                    }));

                }
                finally
                {
                    System.IO.Directory.Delete(downloadfolder, true);
                    StartProgress = false;
                    BlackBox = Visibility.Collapsed;
                }

            });

            await t;

            if (t.IsCompleted)
                AsyncMessageBox("Backup restaurado!", DialogBoxColor.Green, false);
            else
                AsyncMessageBox("Backup NÃO restaurado!", DialogBoxColor.Red, false);
        }

        private async void ListarBackups()
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;

            var t = Task.Run(() => {

                List<string> listfilenames = new List<string>();

                var filenames = from fullFilename
                    in Directory.EnumerateFiles(BackupPath, "*.*")
                                select Path.GetFileName(fullFilename);

                foreach (string filename in filenames)
                {
                    listfilenames.Add(filename);
                }

                BackupList = listfilenames;
            });

            await t;

            if (t.IsCompleted)
            {
                StartProgress = false;
                BlackBox = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
