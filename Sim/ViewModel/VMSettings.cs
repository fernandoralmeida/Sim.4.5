using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.ViewModel
{
    using Mvvm.Observers;
    using Account;

    class VMSettings : NotifyProperty
    {
        #region Declarations
        private bool _isadmin;
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
        #endregion

        #region Constructor
        public VMSettings()
        {
            GetAcesso();
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
