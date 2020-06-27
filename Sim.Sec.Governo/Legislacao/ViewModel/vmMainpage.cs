using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sim.Sec.Governo.Legislacao.ViewModel
{

    using Mvvm.Observers;
    using Mvvm.Commands;
    using Model;
    using Account;
    
    class vmMainpage : NotifyProperty
    {
        #region Declarations
        private ObservableCollection<mLegislacaoConsulta> _lastdocs = new ObservableCollection<mLegislacaoConsulta>();
        private mDataSearch mdata = new mDataSearch();
        private bool _acesso = false;

        private ICommand _commandnavigate;
        #endregion

        #region Properties
        public ObservableCollection<mLegislacaoConsulta> LastDocs
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

        #region Commands
        public ICommand CommandNavigate
        {
            get
            {

                if (_commandnavigate == null)
                    _commandnavigate = new DelegateCommand(ExecCommandNavigate, null);

                return _commandnavigate;
            }
        }

        private void ExecCommandNavigate(object obj)
        {
            Mvvm.Observers.GlobalNavigation.Navegar = new Uri((string)obj, UriKind.RelativeOrAbsolute);
        }
        #endregion

        #region Constructor
        public vmMainpage()
        {
            GlobalNavigation.SubModulo = "LEGISLAÇÃO";
            GlobalNavigation.Pagina = string.Empty;

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                    AcessoOK = true;

                else
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Legislacao)
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
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());

        }
        #endregion
    }
}
