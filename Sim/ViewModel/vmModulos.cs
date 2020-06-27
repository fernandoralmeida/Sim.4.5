using System;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Diagnostics;

namespace Sim.ViewModel
{
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Account.Model;

    class vmModulos : NotifyProperty
    {
        #region Declarations
        NavigationService ns;

        private bool _hasupdate;

        private bool _govon;
        private bool _deson;

        private ICommand _commandupdate;
        private ICommand _commandexecute;
        #endregion

        #region Properties
        public bool HasUpdate
        {
            get { return _hasupdate; }
            set
            {
                _hasupdate = value;
                RaisePropertyChanged("HasUpdate");
            }
        }

        public bool AcessoGoverno
        {
            get { return _govon; }
            set
            {
                _govon = value;
                RaisePropertyChanged("AcessoGoverno");
            }
        }

        public bool AcessoDesenvolvimento
        {
            get { return _deson; }
            set
            {
                _deson = value;
                RaisePropertyChanged("AcessoDesenvolvimento");
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
                         GlobalNavigation.UriModulo = p.ToString();
                     });

                return _commandexecute;
            }
        }
        #endregion

        #region Constructor
        public vmModulos()
        {
            GlobalNavigation.Reiniciar();
            ns = GlobalNavigation.NavService;
            ns.RemoveBackEntry();
            HasUpdate = false;
            GetAcesso();
        }
        #endregion

        #region Functions
        private void GetAcesso()
        {

            if (Logged.Acesso == (int)AccountAccess.Master)
            {
                AcessoGoverno = true;
                AcessoDesenvolvimento = true;
            }

            else
            {
                AcessoGoverno = false;
                AcessoDesenvolvimento = false;
                foreach (mModulos m in Logged.Modulos)
                {
                    if (m.Modulo == (int)Modulo.Governo && m.Acesso == true)
                        AcessoGoverno = true;

                    if (m.Modulo == (int)Modulo.Desenvolvimento && m.Acesso == true)
                        AcessoDesenvolvimento = true;
                }
            }
        }
        
        #endregion
    }
}
