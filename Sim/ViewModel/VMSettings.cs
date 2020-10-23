using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.ViewModel
{
    using Mvvm.Observers;
    using Account;
    using Mvvm.Commands;

    class VMSettings : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private bool _isadmin;
        private Uri _iconuser;
        #endregion

        #region Properties
        public bool IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                RaisePropertyChanged("IsAdmin");
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
        #endregion

        #region Commands
        public ICommand CommandExecute => new RelayCommand(p => {

            ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute));

        });
        #endregion


        #region Constructor
        public VMSettings()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "CENTRAL DE OPÇÕES";
            GetAcesso();

            if (Logged.Sexo == "F")
                IconUser = Model.UserIcon.Woman;

            if (Logged.Sexo == "M")
                IconUser = Model.UserIcon.Man;
        }
        #endregion

        #region Functions
        private void GetAcesso()
        {

            if (Logged.Acesso > (int)AccountAccess.Normal)
            {
                IsAdmin = true;
            }

        }

        #endregion
    }
}
