using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Controls;

namespace Sim.Sec.Governo.Portarias.ViewModel
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using Sql;

    class vmConsultaAvc : vmConsulta
    {
        #region Declarations
        private string _nome;
        private int _tipo;
        private string _resumo;

        private DateTime _datei;
        private DateTime _datef;

        private ICommand _commandsearchavc;
        private ICommand _commandclearavc;
        #endregion

        #region Properties
        public List<mClassificacao> Tipos
        {
            get
            {
                return new mData().ListaGenerica(SqlCollections.Classi_Only_Non_Blocked);
            }
        }

        public int Tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }

        public string Resumo
        {
            get { return _resumo; }
            set
            {
                _resumo = value;
                RaisePropertyChanged("Resumo");
            }
        }

        public string NomeServidor
        {
            get { return _nome; }
            set
            {
                _nome = value;
                RaisePropertyChanged("NomeServidor");
            }
        }

        public DateTime DateI
        {
            get { return _datei; }
            set
            {
                _datei = value;
                RaisePropertyChanged("DateI");
            }
        }

        public DateTime DateF
        {
            get { return _datef; }
            set
            {
                _datef = value;
                RaisePropertyChanged("DateF");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandSearchAvc
        {
            get
            {
                if (_commandsearchavc == null)
                    _commandsearchavc = new DelegateCommand(ExecuteCommandSearchAvc, null);

                return _commandsearchavc;
            }
        }

        private void ExecuteCommandSearchAvc(object obj)
        {
            CommandClosePrintBox.Execute(null);
            AsyncListarDoc(Parametros());
        }

        public ICommand CommandClearAvc
        {
            get
            {
                if (_commandclearavc == null)
                    _commandclearavc = new DelegateCommand(ExecuteCommandClearAvc, null);

                return _commandclearavc;
            }
        }

        private void ExecuteCommandClearAvc(object obj)
        {
            NomeServidor = string.Empty;
            Resumo = string.Empty;
            ListaDoc.Clear();
            DateI = new DateTime(DateTime.Now.Year, 1, 1);
            DateF = DateTime.Now;
            Tipo = 0;
            ViewFiltros = string.Empty;
        }
        #endregion

        #region Constructor
        public vmConsultaAvc()
        {
            Mvvm.Observers.GlobalNavigation.Pagina = "CONSULTA";
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            DateI = new DateTime(DateTime.Now.Year, 1, 1);
            DateF = DateTime.Now;
        }
        #endregion

        #region Methods

        #endregion

        #region Functions

        private List<object> Parametros()
        {

            List<object> _param = new List<object>() { };

            _param.Add(Numero);//3

            return _param;
        }

        private void AsyncListarDoc(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Filtros.Clear();

            if (Tipo == 0)
                Filtros.Add("[TIPO:TODOS]");
            else
                Filtros.Add("[TIPO:" + Tipos[Tipo].Nome + "]");

            if (NomeServidor != string.Empty)
                Filtros.Add("[NOME:" + NomeServidor + "]");

            if (Resumo != string.Empty)
                Filtros.Add("[RESUMO:" + Resumo + "]");

            Filtros.Add("[PERÍODO:" + DateI.ToShortDateString() + " À " + DateF.ToShortDateString() + "]");

            StringBuilder sb = new StringBuilder();
            foreach (string filtro in Filtros)
                sb.Append(filtro);

            ViewFiltros = sb.ToString();

            Task.Factory.StartNew(() => mdata.ConsultaDetelhada(Tipo.ToString(),NomeServidor,Resumo,DateI,DateF))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListaDoc = task.Result;
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

        private void AsyncFlowDoc()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Task.Factory.StartNew(() => new vmFlowDocumentRel().CreateFlowDocument(ListaDoc, Filtros))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Background,
                            new Action(() =>
                            {
                                FlowDoc = new vmFlowDocumentRel().CreateFlowDocument(ListaDoc, Filtros);
                                BlackBox = Visibility.Collapsed;
                                MainBox = Visibility.Collapsed;
                                PrintBox = Visibility.Visible;
                                StartProgress = false;
                            }));

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
