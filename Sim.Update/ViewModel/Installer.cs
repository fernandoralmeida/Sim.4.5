using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sim.Update.ViewModel
{

    using Helpers;
    using Common;

    class Installer : NotifyPropertyChanged
    {
        #region Declarations

        private DispatcherTimer d_timer = new DispatcherTimer();

        private bool _fileexist = false;

        private string _textprogress;

        private bool _startprogress;

        private bool _buttonenabled = true;


        #endregion

        #region Properties

        public bool StartProgress
        {
            get { return _startprogress; }
            set
            {

                _startprogress = value;
                OnPropertyChanged("StartProgress");
            }
        }

        public string TextProgress
        {
            get { return _textprogress; }
            set
            {
                _textprogress = value;
                OnPropertyChanged("TextProgress");
            }
        }

        public bool ButtonEnabled
        {
            get { return _buttonenabled; }
            set
            {
                _buttonenabled = value;
                OnPropertyChanged("ButtonEnabled");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandStart => new RelayCommand(p =>
        {
            StartProgress = true;
            ButtonEnabled = false;

            if (File.Exists(@"sim_install.gz"))
                _fileexist = true;
            else
                _fileexist = false;

            SystemInstaller();
            
        });


        #endregion

        #region Constructor
        public Installer()
        {
            StartProgress = false;
            TextProgress = "";
        }
        
        #endregion
        
        #region Functions

        private async void SystemInstaller()
        {
            var t = Task.Run(() => 
            {

                if (_fileexist)
                {
                    
                    TextProgress = "Verificando";

                    Thread.Sleep(2000);
                    //StopProcess();

                    Thread.Sleep(2000);
                    //FullTextProgress = "";
                    TextProgress = "Preparando instalação, aguarde!";

                    Thread.Sleep(2000);
                    Functions.Dir.NewClearDir(Folders.AppData_Sim);

                    Thread.Sleep(2000);
                    TextProgress = "Instalando o SIM, aguarde!";
                    Functions.Zip.ToDirectory("sim_install.gz", Folders.AppData_Sim);

                    Thread.Sleep(2000);
                    Functions.ShortCut.Createlnk("Sim.App", Directories.Desktop, Folders.AppData_Sim + Files.Sim_Exe);
                    File.Copy(Files.Sim_Update_Xml, Folders.AppData_Sim + Files.Sim_Update_Xml, true);
                    _fileexist = false;
                    Thread.Sleep(2000);
                    TextProgress = "Iniciando o SIM, aguarde!";
                    //System.Media.SystemSounds.Exclamation.Play();

                    //FullTextProgress = "";
                }
                else
                {
                    //Functions.CheckUpdate.HasUpdate(Files.File_Update_Xml_FullName);

                    Thread.Sleep(2000);
                    TextProgress = "Verificando";

                    //File.Copy(Directories.Net_Work_Path + Common.Files.File_Install, Folders.AppData_Sim + Files.Sim_Update_Xml, true);
                    //new mFTP().Download(Files.File_Install_FullName, Files.File_Update_FullName_Temp, bgWorker);

                    Thread.Sleep(2000);
                    //StopProcess();

                    Thread.Sleep(2000);
                    TextProgress = "Preparando instalação, aguarde!";

                    Thread.Sleep(2000);
                    Functions.Dir.NewClearDir(Folders.AppData_Sim);

                    Thread.Sleep(2000);
                    TextProgress = "Instalando o SIM, aguarde!";
                    Functions.Zip.ToDirectory(Directories.Net_Work_Path + Files.File_Install, Folders.AppData_Sim);

                    Thread.Sleep(2000);
                    Functions.ShortCut.Createlnk("Sim.App", Directories.Desktop, Folders.AppData_Sim + Files.Sim_Exe);
                    File.Copy(Directories.Net_Work_Path + Files.Sim_Update_Xml, Folders.AppData_Sim + Files.Sim_Update_Xml, true);
                    _fileexist = false;

                    Thread.Sleep(2000);
                    TextProgress = "Iniciando o SIM, aguarde!";
                    //System.Media.SystemSounds.Exclamation.Play();
                }

            });

            await t;

            new Functions.Processos().ExecuteProcess("Sim.App");
        }

        #endregion
    }
}
