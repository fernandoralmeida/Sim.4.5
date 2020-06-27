using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Mvvm.Observers;
    using Model;
    using Account;

    class vmMainpage : NotifyProperty
    {
        #region Declarations
        private ObservableCollection<mPortaria> _lastdocs = new ObservableCollection<mPortaria>();
        private mData mdata = new mData();
        private bool _acesso = false;
        #endregion

        #region Properties
        public ObservableCollection<mPortaria> LastDocs
        {
            get { return _lastdocs; }
            set
            {
                _lastdocs = value;
                RaisePropertyChanged("LastDocs");
            }
        }

        public bool AcessoOK
        {
            get { return _acesso; }
            set
            {
                _acesso = value;
                RaisePropertyChanged("AcessoOK");
            }
        }
        #endregion

        #region Constructor
        public vmMainpage()
        {
            GlobalNavigation.SubModulo = "PORTARIAS";
            GlobalNavigation.Pagina = string.Empty;

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                    AcessoOK = true;

                else
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Portarias)
                        {
                            if (m.Acesso >= (int)SubModuloAccess.Operador)
                                AcessoOK = true;
                        }
                    }
                }
            }

            AsyncLastDocs();

        }
        #endregion

        #region Functions
        private void AsyncLastDocs()
        {

            Task.Factory.StartNew(() => mdata.LastRows())
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        LastDocs = task.Result;
                });

        }
        #endregion
    }
}
