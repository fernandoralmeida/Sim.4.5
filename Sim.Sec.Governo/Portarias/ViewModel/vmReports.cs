using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sim.Sec.Governo.Portarias.ViewModel
{

    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Controls;

    class vmReports : NotifyProperty
    {

        #region Declarations
        
        private vmEstatisticsProvider Est = new vmEstatisticsProvider();

        FlowDocument _flowdocument = new FlowDocument();

        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();

        private List<string> _filters = new List<string>();

        private bool _chart_ano;
        private bool _chart_classificacao;
        private bool _chart_servidor;
        private bool _text_print;
        private bool _startprogress;

        private int _tipo;

        private string _nome;
        private string _viewfiltros;

        private DateTime _datei;
        private DateTime _datef;

        private Visibility _blackbox;
        private Visibility _printbox;
        private Visibility _mainbox;

        private ICommand _commandsearch;
        private ICommand _commandlistar;
        private ICommand _commandcancel;
        private ICommand _commandclear;
        private ICommand _commandprint;
        private ICommand _commandcloseprintbox;

        #endregion

        #region Commands

        public ICommand CommandSearch
        {
            get
            {
                if (_commandsearch == null)
                    _commandsearch = new DelegateCommand(ExecuteCommandSearch, null);

                return _commandsearch;

            }
        }

        public ICommand CommandListar
        {
            get
            {
                if (_commandlistar == null)
                    _commandlistar = new DelegateCommand(ExecuteCommandListar, null);

                return _commandlistar;
            }
        }

        public ICommand CommandClear
        {
            get
            {
                if (_commandclear == null)
                    _commandclear = new DelegateCommand(ExecuteCommandClear, null);

                return _commandclear;
            }
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCommandCancel, null);

                return _commandcancel;
            }
        }

        public ICommand CommandPrint
        {
            get
            {
                if (_commandprint == null)
                    _commandprint = new DelegateCommand(ExecCommandPrint, null);

                return _commandprint;
            }
        }
               
        public ICommand CommandClosePrintBox
        {
            get
            {
                if (_commandcloseprintbox == null)
                    _commandcloseprintbox = new DelegateCommand(ExecuteCommandClosePrintBox, null);

                return _commandcloseprintbox;
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<BarChart> Charts
        {
            get { return _charts; }
            set
            {
                _charts = value;
                RaisePropertyChanged("Charts");
            }
        }

        public FlowDocument FlowDoc
        {
            get { return _flowdocument; }
            set
            {
                _flowdocument = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public List<string> Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                RaisePropertyChanged("Filters");
            }
        }

        public string ViewFiltros
        {
            get { return _viewfiltros; }
            set
            {
                _viewfiltros = value;
                RaisePropertyChanged("ViewFiltros");
            }
        }

        public List<string> servName
        {
            get { return new mData().ListName(); }
        }

        public List<mClassificacao> Tipos
        {
            get { return new mData().ListaGenerica(Sql.SqlCollections.Classi_Only_Non_Blocked); }
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

        public string NomeServidor
        {
            get { return _nome; }
            set
            {
                _nome = value;
                RaisePropertyChanged("NomeServidor");
            }
        }

        public bool ChartAno
        {
            get { return _chart_ano; }
            set
            {
                _chart_ano = value;
                RaisePropertyChanged("ChartTipo");
            }
        }

        public bool ChartClassificacao
        {
            get { return _chart_classificacao; }
            set
            {
                _chart_classificacao = value;
                RaisePropertyChanged("ChartClassificacao");
            }
        }

        public bool ChartServidor
        {
            get { return _chart_servidor; }
            set
            {
                _chart_servidor = value;
                RaisePropertyChanged("ChartAutor");
            }
        }

        public bool TextPrint
        {
            get { return _text_print; }
            set
            {
                _text_print = value;
                RaisePropertyChanged("TextPrint");
            }
        }

        public bool StartProgress
        {
            get { return _startprogress; }
            set
            {
                _startprogress = value;
                RaisePropertyChanged("StartProgress");
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

        public Visibility MainBox
        {
            get { return _mainbox; }
            set
            {
                _mainbox = value;
                RaisePropertyChanged("MainBox");
            }
        }

        public Visibility PrintBox
        {
            get { return _printbox; }
            set
            {
                _printbox = value;
                RaisePropertyChanged("PrintBox");
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

        #region Constructor

        public vmReports()
        {
            GlobalNavigation.Pagina = "RELATÓRIOS";

            BlackBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;

            StartProgress = false;

            DateI = new DateTime(DateTime.Now.Year, 1, 1);
            DateF = DateTime.Now;

            NomeServidor = string.Empty;

            ChartAno = true;
            ChartClassificacao = true;
            ChartServidor = true;
            TextPrint = true;
        }

        #endregion

        #region Methods

        private void ExecuteCommandSearch(object param)
        {
            Filters.Clear();

            if (Tipo == 0)
                Filters.Add("[TIPO:TODOS]");
            else
                Filters.Add("[TIPO:" + Tipos[Tipo].Nome + "]");

            if (NomeServidor != string.Empty)
                Filters.Add("[NOME:" + NomeServidor + "]");
            
            Filters.Add("[PERÍODO:" + DateI.ToShortDateString() + " À " + DateF.ToShortDateString() + "]");

            AsyncReports(Parametros());
        }

        private void ExecuteCommandCancel(object param)
        {

        }

        private void ExecuteCommandListar(object param)
        {
            AsyncFlowDoc(Parametros());            
        }

        private void ExecCommandPrint(object obj)
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            //Now print using PrintDialog
            var pd = new PrintDialog();

            if (pd.ShowDialog().Value)
            {
                FlowDoc.PageHeight = pd.PrintableAreaHeight;
                FlowDoc.PageWidth = pd.PrintableAreaWidth;
                IDocumentPaginatorSource idocument = FlowDoc as IDocumentPaginatorSource;

                pd.PrintDocument(idocument.DocumentPaginator, "Printing....");
            }
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
        }

        private void ExecuteCommandClosePrintBox(object obj)
        {
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
        }

        private void ExecuteCommandClear(object param)
        {
            Est.Reset();
            Charts.Clear();
            DateI = new DateTime(DateTime.Now.Year, 1, 1);
            DateF = DateTime.Now;
            NomeServidor = string.Empty;
            Tipo = 0;
        }

        #endregion

        #region Functions

        private List<object> Parametros()
        {
            List<object> _param = new List<object>() { };

            _param.Add(DateI.ToShortDateString());//0
            _param.Add(DateF.ToShortDateString());//1
            if (Tipo == 0) _param.Add("%"); else _param.Add(Tipo.ToString()); //2
            if (NomeServidor == string.Empty) _param.Add("%"); else _param.Add(NomeServidor); //3            

            return _param;
        }

        private void AsyncReports(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            StringBuilder sb = new StringBuilder();
            foreach (string filtro in Filters)
                sb.Append(filtro);

            ViewFiltros = sb.ToString();

            Task.Factory.StartNew(() => Est.GoStatistics(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Application.Current.Dispatcher.Invoke(
                        System.Windows.Threading.DispatcherPriority.Background,
                        new Action(() =>
                        {
                            Charts.Clear();
                            Charts = new vmChartsProvider().Create(Est, ChartAno, ChartServidor, ChartClassificacao, 0);
                            BlackBox = Visibility.Collapsed;
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

        private void AsyncFlowDoc(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Task.Factory.StartNew(() => Est.GoStatistics(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Background,
                            new Action(() =>
                            {
                                //FlowDoc = new vmFlowDocumentEst().ChartFlowDoc(new vmChartsProvider().Create(Est, ChartAno, ChartServidor, ChartClassificacao, 0), Filters);
                                FlowDoc = new vmFlowDocumentEst().TextFlowDoc(Est.Relatories, ChartAno, ChartServidor, ChartClassificacao, Filters);
                                BlackBox = Visibility.Collapsed;
                                MainBox = Visibility.Collapsed;
                                PrintBox = Visibility.Visible;
                                StartProgress = false;

                                //ExecCommandPrint(null);
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
