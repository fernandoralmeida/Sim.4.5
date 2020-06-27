using System;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.Sec.Governo.Menu.ViewModel
{
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Account.Model;

    class vmMainContent : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        
        private bool _leg;
        private bool _port;
        private bool _denom;

        private ICommand _commandexecute;
        #endregion

        #region Properties

        public bool AcessoLegislacao
        {
            get { return _leg; }
            set
            {
                _leg = value;
                RaisePropertyChanged("AcessoLegislacao");
            }
        }

        public bool AcessoPortarias
        {
            get { return _port; }
            set
            {
                _port = value;
                RaisePropertyChanged("AcessoPortarias");
            }
        }

        public bool AcessoDenominacoes
        {
            get { return _denom; }
            set
            {
                _denom = value;
                RaisePropertyChanged("AcessoDenominacoes");
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
        public vmMainContent()
        {
            GlobalNavigation.Reiniciar();
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Modulo = "GOVERNO";
            GetAcesso();
        }
        #endregion

        #region Functions
        private void GetAcesso()
        {

            if (Logged.Acesso == (int)AccountAccess.Master)
            {
                AcessoLegislacao = true;
                AcessoPortarias = true;
                AcessoDenominacoes = true;
            }

            else
            {
                AcessoLegislacao = false;
                AcessoPortarias = false;
                AcessoDenominacoes = false;

                foreach (mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.Legislacao && m.Acesso > 0)
                        AcessoLegislacao = true;

                    if (m.SubModulo == (int)SubModulo.Portarias && m.Acesso > 0)
                        AcessoPortarias = true;

                    if (m.SubModulo == (int)SubModulo.Denominacoes && m.Acesso > 0)
                        AcessoDenominacoes = true;
                }
            }
        }

        #endregion
    }
}
