using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sim.ViewModel
{

    using Model;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Account.Model;
    using Controls.ViewModels;

    class vmMainWindow : VMBase, IDialogBox
    {

        #region Declarations
        NavigationService ns;
        NavigationService _settingsframe;
        mApps _apps = new mApps();
        mMemory _mmemory = new mMemory();

        private Uri _iconuser;
        private string _title;
        private string _labeltext;
        private Uri _pageselected;

        private string _sysname;
        private string _modulo;
        private string _submodulo;
        private string _pagina;
        private string _urimodulo;
        private string _urisubmodulo;

        private Visibility _menuon;
        private Visibility _browseback;
        private Visibility _isadmin;
        private Visibility _menuonoff;

        private Visibility _modview;
        private Visibility _submodview;
        private Visibility _pageview;
        private Visibility _viewinfo;
        private Visibility _startclosed = Visibility.Visible;
        private Visibility _settingsvisible = Visibility.Collapsed;
        private Visibility _windowchromebuttonsvisible = Visibility.Collapsed;

        private ICommand _commandbrowseback;
        private ICommand _commandgopage;
        private ICommand _commandlogoff;
        private ICommand _commandmenuonoff;
        #endregion

        #region Properties

        public mApps Apps
        {
            get { return _apps; }
            set
            {
                _apps = value;
                RaisePropertyChanged("Apps");
            }
        }

        public mMemory Memory
        {
            get { return _mmemory; }
            set
            {
                _mmemory = value;
                RaisePropertyChanged("Memory");
            }
        }

        public Uri SelectedPage
        {
            get { return _pageselected; }
            set
            {
                _pageselected = value;
                RaisePropertyChanged("SelectedPage");
            }
        }

        public Uri IconUser
        {
            get { return _iconuser; }
            set
            {
                _iconuser = value;
                RaisePropertyChanged("IconUser");
            }
        }

        public string Operador
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Operador");
            }
        }

        public string LabelText
        {
            get { return _labeltext; }
            set
            {
                _labeltext = value;
                RaisePropertyChanged("LabelText");
            }
        }

        public string SysName
        {
            get { return _sysname; }
            set
            {
                _sysname = value;
                RaisePropertyChanged("SysName");
            }
        }

        public string Modulo
        {
            get { return _modulo; }
            set
            {
                _modulo = value;

                if (_modulo == string.Empty)
                    ModView = Visibility.Collapsed;
                else
                    ModView = Visibility.Visible;


                RaisePropertyChanged("Modulo");
            }
        }

        public string SubModulo
        {
            get { return _submodulo; }
            set
            {
                _submodulo = value;

                if (_submodulo == string.Empty)
                    SubModView = Visibility.Collapsed;
                else
                    SubModView = Visibility.Visible;

                RaisePropertyChanged("SubModulo");
            }
        }

        public string Pagina
        {
            get { return _pagina; }
            set
            {
                _pagina = value;

                if (_pagina == string.Empty)
                    PageView = Visibility.Collapsed;
                else
                    PageView = Visibility.Visible;

                RaisePropertyChanged("Pagina");
            }
        }

        public string UriModulo
        {
            get { return _urimodulo; }
            set
            {
                _urimodulo = value;
                RaisePropertyChanged("UriModulo");
            }
        }

        public string UriSubModulo
        {
            get { return _urisubmodulo; }
            set
            {
                _urisubmodulo = value;
                RaisePropertyChanged("UriSubModulo");
            }
        }

        public Visibility IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                RaisePropertyChanged("IsAdmin");
            }
        }

        public Visibility MenuOn
        {
            get { return _menuon; }
            set
            {
                _menuon = value;
                RaisePropertyChanged("MenuOn");
            }
        }

        public Visibility MenuOnOff
        {
            get
            {
                return _menuonoff;
            }
            set
            {
                _menuonoff = value;
                RaisePropertyChanged("MenuOnOff");
            }
        }

        public Visibility BrowseBack
        {
            get { return _browseback; }
            set
            {
                _browseback = value;
                RaisePropertyChanged("BrowseBack");
            }
        }

        public Visibility ViewInfo
        {
            get { return _viewinfo; }
            set
            {
                _viewinfo = value;
                RaisePropertyChanged("ViewInfo");
            }
        }

        public Visibility ModView
        {
            get { return _modview; }
            set { _modview = value;
                RaisePropertyChanged("ModView");
            }
        }

        public Visibility SubModView
        {
            get { return _submodview; }
            set
            {
                _submodview = value;
                RaisePropertyChanged("SubModView");
            }
        }

        public Visibility PageView
        {
            get { return _pageview; }
            set
            {
                _pageview = value;
                RaisePropertyChanged("PageView");
            }
        }

        public Visibility StartClosed
        {
            get { return _startclosed; }
            set { _startclosed = value;RaisePropertyChanged("StartClosed"); }
        }

        public Visibility SettingsVisible
        {
            get { return _settingsvisible; }
            set
            {
                _settingsvisible = value;RaisePropertyChanged("SettingsVisible");
            }
        }

        public Visibility WindowChomeButtonsVisible
        {
            get { return _windowchromebuttonsvisible; } 
            set { _windowchromebuttonsvisible = value; RaisePropertyChanged("WindowChomeButtonsVisible"); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Commando dispara ou interrompe informações do SIM
        /// </summary>
        public ICommand CommandShowInfoSys => new RelayCommand(p => {

            ViewInfo = Visibility.Visible;
            ShowInfoSys();            
        });

        public ICommand CommandCloseInfoSys => new RelayCommand(p => 
        {
            ViewInfo = Visibility.Collapsed;
            new Functions.GetMemoryUse().OutMemory();
        });

        public ICommand CommandLogOff
        {
            get
            {
                if (_commandlogoff == null)
                    _commandlogoff = new RelayCommand(p =>
                    {
                        SyncDialogBox("Deseja trocar Operador?", DialogBoxColor.Red);
                    });

                return _commandlogoff;
            }
        }

        public ICommand CommandBrowseBack
        {
            get
            {
                if (_commandbrowseback == null)
                    _commandbrowseback = new RelayCommand(p => {
                        if (ns.CanGoBack == true)
                            ns.GoBack();
                    });
                return _commandbrowseback;
            }
        }

        public ICommand CommandMsgYes => new RelayCommand(p =>
        {
            new Logged().LogOut();
            ViewDialogBox = Visibility.Collapsed;
            ns = GlobalNavigation.NavService;
            ns.RemoveBackEntry();
        });


        public ICommand CommandMsgNot => new RelayCommand(p =>
        {
            ViewDialogBox = Visibility.Collapsed;
        });

        public ICommand CommandNavigate => new RelayCommand(p => {
            ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute));
        });

        public ICommand CommandGoPage
        {
            get
            {
                _commandgopage = new RelayCommand(p =>
                {
                    if ((p != null) && (Logged.Autenticado == true))
                    {
                        ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute));
                    }
                });

                return _commandgopage;
            }
        }

        public ICommand CommandOnOff
        {
            get
            {
                if (_commandmenuonoff == null)
                    _commandmenuonoff = new RelayCommand(p =>
                    {

                        if (MenuOnOff == Visibility.Collapsed)
                        {
                            MenuOnOff = Visibility.Visible;
                        }
                        else
                        {
                            MenuOnOff = Visibility.Collapsed;
                        }

                    });

                return _commandmenuonoff;
            }
        }

        public ICommand CommandViewSettings => new RelayCommand(p => {

            SettingsVisible = Visibility.Visible;

        });

        public ICommand CommandCloseSettings => new RelayCommand(p => {

            SettingsVisible = Visibility.Collapsed;
        });
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public vmMainWindow()
        {

            mStarted.SimStarted = true;
            ns = GlobalNavigation.NavService;
            _settingsframe = GlobalNavigation.ChildFrame;
            ns.Navigated += Ns_Navigated;
            ViewDialogBox = Visibility.Collapsed;
            GlobalNotifyProperty.GlobalPropertyChanged += new PropertyChangedEventHandler(OnGlobalPropertyChanged);
            Logged.Autenticado = false;
            ViewInfo = Visibility.Collapsed;
            SysName = "INICIO";            

            Task.Factory.StartNew(() =>
            {
                //Folders.Create(Folders.SimApp, Folders.Pdf);

                string simnewupadate = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Sim.New.Updater.exe");
                string simoldupadate = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Sim.Updater.exe");

                if (System.IO.File.Exists(simnewupadate))
                {
                    System.IO.File.Delete(simoldupadate);
                    System.IO.File.Copy(simnewupadate, simoldupadate);
                    System.IO.File.Delete(simnewupadate);
                }               

            });
            
            //NavigationService.Navigate(new Uri("../View/pToolsTheme.xaml", UriKind.Relative));
            //SelectedPage = new Uri("../View/pToolsTheme.xaml", UriKind.Relative);
            //ExecuteUpdate();
            //DebugLogin();
        }

        private void Ns_Navigated(object sender, NavigationEventArgs e)
        {

            //MessageBox.Show(e.Uri.ToString() + "\n" + SysName);

            if ((e.Uri.ToString() == Properties.Resources.Sim_View_Root) ||
                (e.Uri.ToString() == "Sim.Sec.Desenvolvimento;component/Menu/View/pMenu.xaml") ||
                (@"/" + e.Uri.ToString() == Sec.Governo.Properties.Resources.Sec_Governo_Menu))
            {
                while (ns.CanGoBack)
                    ns.RemoveBackEntry();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Observa Propriedades Globais
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGlobalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Autenticado")
            {
                if (Logged.Autenticado == false)
                {
                    GlobalNavigation.Reiniciar();
                    MenuOn = Visibility.Collapsed;
                    IsAdmin = Visibility.Collapsed;
                    MenuOnOff = Visibility.Collapsed;
                    //Application.Current.Resources["WindowBackgroundColor"] = Color.FromRgb(0x1b, 0xa1, 0xe2);
                    if (mStarted.SimStarted)
                        ns.Navigate(new Uri(Properties.Resources.Sim_View_Load, UriKind.Relative));
                    else
                    {
                        ns.Navigate(new Uri(Account.Properties.Resources.Account_Login_User, UriKind.Relative));
                        WindowChomeButtonsVisible = Visibility.Visible;
                        StartClosed = Visibility.Collapsed;
                    }

                    Color paleta = new Color();
                    //paleta = (Color)ColorConverter.ConvertFromString("##00FFFF");
                    paleta = Color.FromRgb(0x1b, 0xa1, 0xe2);
                    new UI.Presentation.ThemeManager().ApplyTheme(paleta, "Light");                    
                }

                else
                {

                    Color paleta = new Color();
                    paleta = (Color)ColorConverter.ConvertFromString(Logged.Color);
                    new UI.Presentation.ThemeManager().ApplyTheme(paleta, Logged.Thema);
                    MenuOn = Visibility.Visible;
                    
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new System.Action(delegate
                    {
                        ns.Navigate(new Uri(Properties.Resources.Sim_View_Root, UriKind.Relative));
                    }));
                    if (Logged.Acesso >= (int)AccountAccess.Administrador)
                        IsAdmin = Visibility.Visible;

                }

                if (Logged.Sexo == "F")
                    IconUser = UserIcon.Woman;

                if (Logged.Sexo == "M")
                    IconUser = UserIcon.Man;

                Operador = Logged.Nome;

            }

            if (e.PropertyName == "Modulo")
                Modulo = GlobalNavigation.Modulo;

            if (e.PropertyName == "UriModulo")
                UriModulo = GlobalNavigation.UriModulo;

            if (e.PropertyName == "UriSubModulo")
                UriSubModulo = GlobalNavigation.UriSubModulo;

            if (e.PropertyName == "SubModulo")
                SubModulo = GlobalNavigation.SubModulo;


            if (e.PropertyName == "Pagina")
                Pagina = GlobalNavigation.Pagina;

            if (e.PropertyName == "BrowseBack")
                BrowseBack = GlobalNavigation.BrowseBack;

        }

        #endregion

        #region Functions
        private void ShowInfoSys()
        {
            System.Reflection.Assembly assemblyname = System.Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName);
            Apps = new Functions.GetSIMInfo(assemblyname.Location).Apps;
            Memory = new Functions.GetMemoryUse().Memory;
        }

        /// <summary>
        /// Login automatico somente para o modo debug
        /// </summary>
        [Conditional("DEBUG")]
        private void DebugLogin()
        {
            new mData().AutenticarUsuario("sim.master", "sim.master");
            SysName = "Sim.App.Desktop [MODE:BETA.TESTER]";
        }
        #endregion

    }
}
