using System;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Menu.ViewModel
{

    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Account.Model;

    class vmMenu : NotifyProperty
    {
        #region Declarations
        NavigationService ns;

        private bool _leg;
        private bool _port;
        private bool _denom;

        private ICommand _commandexecute;
        #endregion

        #region Properties

        public bool AcessoSebrae
        {
            get { return _leg; }
            set
            {
                _leg = value;
                RaisePropertyChanged("AcessoSebrae");
            }
        }

        public bool AcessoSalaEmp
        {
            get { return _port; }
            set
            {
                _port = value;
                RaisePropertyChanged("AcessoSalaEmp");
            }
        }

        public bool AcessoComAmbulante
        {
            get { return _denom; }
            set
            {
                _denom = value;
                RaisePropertyChanged("AcessoComAmbulante");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandExecute
        {
            get
            {
                if (_commandexecute == null)
                    _commandexecute = new RelayCommand(p =>
                    {
                        ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute));
                        GlobalNavigation.UriSubModulo = p.ToString();
                    });

                return _commandexecute;
            }
        }
        #endregion

        #region Constructor
        public vmMenu()
        {
            GlobalNavigation.Reiniciar();
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Modulo = "DESENVOLVIMENTO";
            GetAcesso();
        }
        #endregion

        #region Functions
        private void GetAcesso()
        {

            if (Logged.Acesso == (int)AccountAccess.Master)
            {
                AcessoSebrae = true;
                AcessoSalaEmp = true;
                AcessoComAmbulante = true;
            }

            else
            {
                AcessoSebrae = false;
                AcessoSalaEmp = false;
                AcessoComAmbulante = false;

                foreach (mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SebraeAqui && m.Acesso > 0)
                        AcessoSebrae = true;

                    if (m.SubModulo == (int)SubModulo.SalaEmpreendedor && m.Acesso > 0)
                        AcessoSalaEmp = true;

                    if (m.SubModulo == (int)SubModulo.ComercioAmbulante && m.Acesso > 0)
                        AcessoComAmbulante = true;
                }
            }
        }

        #endregion
    }
}
