using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data;
using System.Threading.Tasks;

namespace Sim.Sec.Governo.NomesPublicos.ViewModel
{
    using Model;
    using Mvvm.Observers;
    using Account;

    class vmMain: NotifyProperty
    {
        #region Declarations
        private DataTable _lastdocs = new DataTable();
        private mData mdata = new mData();
        private bool _isenable;
        #endregion

        #region Properties
        public DataTable LastDocs
        {
            get { return _lastdocs; }
            set
            {
                _lastdocs = value;
                RaisePropertyChanged("LastDocs");
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
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public vmMain()
        {
            AsyncLastDocs();
            GlobalNavigation.SubModulo = "NOMES MUNICIPAIS";
            GlobalNavigation.Pagina = string.Empty;

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                    IsEnable = true;

                else
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Denominacoes)
                        {
                            if (m.Acesso >= (int)SubModuloAccess.Operador)
                                IsEnable = true;
                        }
                    }
                }
            }

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
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());

        }

        #endregion
    }
}
