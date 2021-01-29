using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.ViewModel
{
    using Model;
    using Shared.Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Account;
    class vmAmbulantes : NotifyProperty
    {
        #region Declarations
        private NavigationService ns;
        private ObservableCollection<Ambulante> _listacm = new ObservableCollection<Ambulante>();

        private bool _starprogress;
        private bool _isenable;
        private Visibility _blackbox;

        private string _getcpf = string.Empty;

        private ICommand _commandgetcpf;
        private ICommand _commandprintprofile;
        #endregion

        #region Properties

        public ObservableCollection<Ambulante> ListarCA
        {
            get { return _listacm; }
            set
            {
                _listacm = value;
                RaisePropertyChanged("ListarCA");
            }
        }

        public string GetCPF
        {
            get { return _getcpf; }
            set
            {
                _getcpf = value;
                RaisePropertyChanged("GetCPF");
            }
        }

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }

        public bool IsEnable
        {
            get { return _isenable; }
            set
            {
                _isenable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandGetCPF
        {
            get
            {

                _commandgetcpf = new RelayCommand(p => {

                    if (GetCPF != string.Empty && new mMascaras().Remove(GetCPF.TrimEnd()).Length == 11)
                    {
                        if (new mData().ExistPessoaFisica(GetCPF.TrimEnd()) != null)
                        {
                            AreaTransferencia.CPF = new mMascaras().Remove(GetCPF.TrimEnd());
                            ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/pNovo.xaml", UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("CPF não encontrado!", "Sim.Alerta!");
                        }
                    }

                });

                return _commandgetcpf;
            }
        }

        public ICommand CommandPrintProfile
        {
            get
            {
                if (_commandprintprofile == null)
                    _commandprintprofile = new RelayCommand(p =>
                    {
                        AreaTransferencia.CadastroAmbulante = (string)p;
                        ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/AmbulantesPrint.xaml", UriKind.Relative));                        
                    });

                return _commandprintprofile;
            }
        }
        #endregion

        #region  Constructor
        public vmAmbulantes()
        {
            ns = GlobalNavigation.NavService;

            GlobalNavigation.Pagina = "";

            if (Logged.Acesso == (int)AccountAccess.Master)
                IsEnable = true;


            if (GlobalNavigation.SubModulo.ToLower() == "COMÉRCIO AMBULANTE".ToLower())
            {

                foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.ComercioAmbulante)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                            IsEnable = true;
                    }
                }
            }

            AsyncListarCAmbulatens();
        }
        #endregion

        #region Function
        private async void AsyncListarCAmbulatens()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            var t = Task.Factory.StartNew(() => new Repositorio.RAmbulante().Top10Ambulantes());

            await t;

            if (t.IsCompleted)
            {
                ListarCA = t.Result;
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
            }
            else
            {
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
            }

        }
        #endregion
    }
}
