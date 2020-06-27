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

namespace Sim.Sec.Governo.Legislacao.ViewModel
{
    using Model;
    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Controls;
    using Providers;


    class vmReports : NotifyProperty
    {

        #region Declarations
        private mDataSearch mdata = new mDataSearch();
        private vmDataReports Rel = new vmDataReports();

        private FlowDocument _flowdocument = new FlowDocument();

        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();

        private List<string> _filters = new List<string>();

        private DateTime _dateI;
        private DateTime _dateF;

        private string _viewfiltros;

        private bool _leis;
        private bool _leiscomp;
        private bool _decretos;
        private bool _chart_tipo;
        private bool _chart_classificacao;
        private bool _chart_situacao;
        private bool _chart_origem;
        private bool _chart_autor;
        private bool _printtext;
        private bool _startprogress;

        private int _getclassificacao;
        private int _getorigem;
        private int _getsituacao;       

        private Visibility _blackbox;
        private Visibility _printbox;
        private Visibility _mainbox;

        private ICommand _commandsearch;
        private ICommand _commandlistar;
        private ICommand _commandcancel;
        private ICommand _commandclear;
        private ICommand _commandcloseprint;
        private ICommand _commandprint;

        #endregion

        #region Commands

        public ICommand CommandSearch
        {
            get {
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

        public ICommand CommandLimpar
        {
            get
            {
                if (_commandclear == null)
                    _commandclear = new DelegateCommand(ExecuteCommandLimpar, null);

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
                    _commandprint = new DelegateCommand(ExecuteCommandPrint, null);
                return _commandprint;
            }
        }

        public ICommand CommandClosePrintBox
        {
            get
            {
                if (_commandcloseprint == null)
                    _commandcloseprint = new DelegateCommand(ExecuteCommandClosePrint, null);

                return _commandcloseprint;
            }
        }

        #endregion

        #region Properties
        public FlowDocument FlowDoc
        {
            get { return _flowdocument; }
            set
            {
                _flowdocument = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public ObservableCollection<BarChart> Charts {
            get { return _charts; }
            set
            {
                _charts = value;
                RaisePropertyChanged("Charts");
            }
        }

        public List<mTiposGenericos> Origem
        {
            get { return new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Origem_Only_Non_Blocked); }
        }

        public List<mTiposGenericos> Situacao
        {
            get { return new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Situacao_Only_Non_Blocked); }
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

        public DateTime DateI
        {
            get { return _dateI; }
            set
            {
                _dateI = value;
                RaisePropertyChanged("DateI");
            }
        }

        public DateTime DateF
        {
            get { return _dateF; }
            set
            {
                _dateF = value;
                RaisePropertyChanged("DateF");
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

        public int getClassificacao
        {
            get { return _getclassificacao; }
            set
            {
                _getclassificacao = value;
                RaisePropertyChanged("getClassificacao");
            }
        }

        public int getOrigem
        {
            get { return _getorigem; }
            set
            {
                _getorigem = value;
                RaisePropertyChanged("getOrigem");
            }
        }

        public int getSituacao
        {
            get { return _getsituacao; }
            set
            {
                _getsituacao = value;
                RaisePropertyChanged("getSituacao");
            }
        }

        public bool ChartTipo
        {
            get { return _chart_tipo; }
            set { _chart_tipo = value;
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

        public bool ChartSituacao
        {
            get { return _chart_situacao; }
            set
            {
                _chart_situacao = value;
                RaisePropertyChanged("ChartSituacao");
            }
        }

        public bool ChartOrigem
        {
            get { return _chart_origem; }
            set
            {
                _chart_origem = value;
                RaisePropertyChanged("ChartOrigem");
            }
        }

        public bool ChartAutor
        {
            get { return _chart_autor; }
            set { _chart_autor = value;
                RaisePropertyChanged("ChartAutor");
            }
        }

        public bool PrintText
        {
            get { return _printtext; }
            set { _printtext = value;
            RaisePropertyChanged("PrintText");
            }
        }

        public bool StartProgress
        {
            get { return _startprogress; }
            set { _startprogress = value;
            RaisePropertyChanged("StartProgress");
            }
        }

        public bool Leis
        {
            get { return _leis; }
            set
            {
                _leis = value;
                RaisePropertyChanged("Leis");
            }
        }

        public bool Leis_Comp
        {
            get { return _leiscomp; }
            set
            {
                _leiscomp = value;
                RaisePropertyChanged("Leis_Comp");
            }
        }

        public bool Decretos
        {
            get { return _decretos; }
            set
            {
                _decretos = value;
                RaisePropertyChanged("Decretos");
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

        #endregion

        #region Constructor

        public vmReports()
        {
            Mvvm.Observers.GlobalNavigation.Pagina = "RELATÓRIOS";
            BlackBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            StartProgress = false;
            DateI = new DateTime(DateTime.Now.Year, 1, 1);
            DateF = DateTime.Now;
            Leis = true;
            Leis_Comp = true;
            Decretos = true;
            ChartTipo = true;
            ChartSituacao = true;
            ChartOrigem = true;
            ChartClassificacao = true;
            ChartAutor = false;
            PrintText = true;       
        }

        #endregion

        #region Methods

        private void ExecuteCommandSearch(object param)
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            
            Filters.Clear();

            string _leis = Leis ? "LEI " : "";
            string _leis_c = Leis_Comp ? "LEI COMPLEMENTAR " : "";
            string _dec = Decretos ? "DECRETO " : "";

            Filters.Add("[TIPO:" + _leis + _leis_c + _dec + "]");

            if (getOrigem == 0)
                Filters.Add("[ORIGEM:TODOS]");
            else
                Filters.Add("[ORIGEM:" + Origem[getOrigem].Nome + "]");

            if (getSituacao == 0)
                Filters.Add("[SITUAÇÃO:TODOS]");
            else
                Filters.Add("[SITUAÇÃO:" + Situacao[getSituacao].Nome + "]");
            
            Filters.Add("[PERÍODO:" + DateI.ToShortDateString() + " à " + DateF.ToShortDateString() + "]");

            AsyncListarLegislacao(Parametros());
        }

        private void ExecuteCommandCancel(object param)
        {
            //bgWorker.CancelAsync();
        }

        private void ExecuteCommandListar(object param)
        {
            AsyncFlowDoc();
        }

        private void ExecuteCommandPrint(object obj)
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

        private void ExecuteCommandClosePrint(object obj)
        {
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void ExecuteCommandLimpar(object param)
        {
            Rel.Reset();
            Charts.Clear();
            DateI = new DateTime(DateTime.Now.Year, 1, 1);
            DateF = DateTime.Now;
            ViewFiltros = string.Empty;
            getSituacao = 0;
            getOrigem = 0;
            ViewFiltros = string.Empty;
            FlowDoc = null;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
        }

        #endregion

        #region Functions

        private List<object> Parametros()
        {

            List<object> _param = new List<object>() { };

            _param.Add(Leis);//0
            _param.Add(Leis_Comp);//1
            _param.Add(Decretos);//2
            _param.Add(DateI.ToShortDateString());//3
            _param.Add(DateF.ToShortDateString());//4
            if (getSituacao == 0) _param.Add("%"); else _param.Add(getSituacao.ToString()); //5
            if (getOrigem == 0) _param.Add("%"); else _param.Add(getOrigem.ToString()); //6

            return _param;
        }

        private void AsyncListarLegislacao(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            StartProgress = true;

            StringBuilder sb = new StringBuilder();
            foreach (string filtro in Filters)
                sb.Append(filtro);

            ViewFiltros = sb.ToString();

            Task.Factory.StartNew(() => Rel.GoRelatory(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Charts = new vmCharts().Create(Rel, ChartTipo, ChartSituacao, ChartOrigem, ChartClassificacao, ChartAutor, 600);
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

            Application.Current.Dispatcher.Invoke(
                            System.Windows.Threading.DispatcherPriority.Background,
                            new Action(() =>
                            {
                                if (PrintText)
                                    FlowDoc = new vmFlowDocumentEst().TextFlowDoc(Rel, ChartTipo, ChartSituacao, ChartOrigem, ChartClassificacao, ChartAutor, Filters);
                                else
                                    FlowDoc = new vmFlowDocumentEst().ChartFlowDocument(new vmCharts().Create(Rel, ChartTipo, ChartSituacao, ChartOrigem, ChartClassificacao, ChartAutor, 600), Filters);

                                BlackBox = Visibility.Collapsed;
                                MainBox = Visibility.Collapsed;
                                PrintBox = Visibility.Visible;
                                StartProgress = false;

                                //ExecuteCommandPrint(null);
                            }));
        }

        #endregion

    }
}
