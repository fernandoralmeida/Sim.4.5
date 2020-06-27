using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Empresa
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Account;

    class vmMain : NotifyProperty
    {
        #region Declarations
        private NavigationService ns;
        private mData mdata = new mData();
        private ObservableCollection<mPJ_Ext> _listapj = new ObservableCollection<mPJ_Ext>();

        private bool _starprogress;
        private bool _isenable;
        private Visibility _blackbox;

        private string _getcnpj = string.Empty;

        private ICommand _commandgetcnpj;
        #endregion

        #region Properties

        public ObservableCollection<mPJ_Ext> ListarPJ
        {
            get { return _listapj; }
            set
            {
                _listapj = value;
                RaisePropertyChanged("ListarPJ");
            }
        }

        public string GetCNPJ
        {
            get { return _getcnpj; }
            set
            {
                _getcnpj = value.Trim();
                RaisePropertyChanged("GetCNPJ");
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
        public ICommand CommandGetCNPJ
        {
            get
            {

                _commandgetcnpj = new RelayCommand(p => {

                    if (GetCNPJ != string.Empty && new mMascaras().Remove(GetCNPJ).Length == 14)
                    {
                        AreaTransferencia.CNPJ = GetCNPJ;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                    }

                });

                return _commandgetcnpj;
            }
        }
        #endregion

        #region  Constructor
        public vmMain()
        {
            ns = GlobalNavigation.NavService;

            GlobalNavigation.Pagina = "EMPRESAS";

            if (Logged.Acesso == (int)AccountAccess.Master)
                IsEnable = true;


            if (GlobalNavigation.SubModulo.ToLower() == "Sala do Empreendedor".ToLower())
            {

                foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SalaEmpreendedor)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                            IsEnable = true;
                    }
                }
            }

            if (GlobalNavigation.SubModulo.ToLower() == "Sebrae Aqui".ToLower())
            {

                foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SebraeAqui)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                            IsEnable = true;
                    }
                }
            }

            AsyncListarEmpresas();
        }
        #endregion

        #region Function
        private void AsyncListarEmpresas()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Task.Factory.StartNew(() => mdata.nEmpresasNow())
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarPJ = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion
    }
}
