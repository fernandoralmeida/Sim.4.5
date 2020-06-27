using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Navigation;

namespace Sim.Sec.Governo.Legislacao.ViewModel
{

    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using System.Threading.Tasks;
    using Account;
    using ViewModel.Providers;

    class vmConsulta : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private mDataSearch mdata = new mDataSearch();        
        private ObservableCollection<mLegislacaoConsulta> _listalegislativa = new ObservableCollection<mLegislacaoConsulta>();
        private FlowDocument _flowDocument = new FlowDocument();
              
        private List<string> _filters = new List<string>();

        private Visibility _blackbox;
        private Visibility _mainbox;
        private Visibility _viewprint;

        private string _viewfiltro;

        private bool _startprogress;

        private int _numero;
        private bool _leis;
        private bool _leis_comp;
        private bool _dec;

        private ICommand _commandsearch;
        private ICommand _commandlistar;
        private ICommand _commandcancel;
        private ICommand _commandlimpar;
        private ICommand _commandshowpdf;
        private ICommand _commandEdit;
        private ICommand _commandprint;
        private ICommand _commandcloseprintbox;
        #endregion

        #region Properties
        public ObservableCollection<mLegislacaoConsulta> ListaLegislativa
        {
            get { return _listalegislativa; }
            set
            {
                _listalegislativa = value;
                RaisePropertyChanged("ListaLegislativa");
            }
        }

        public FlowDocument FlowDoc
        {
            get { return _flowDocument; }
            set
            {
                _flowDocument = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public List<string> Filtros
        {
            get { return _filters; }
            set
            {
                _filters = value;
                RaisePropertyChanged("Filtros");
            }
        }

        public string ViewFiltros
        {
            get { return _viewfiltro; }
            set
            {
                _viewfiltro = value;
                RaisePropertyChanged("ViewFiltros");
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
            get { return _viewprint; }
            set
            {
                _viewprint = value;
                RaisePropertyChanged("PrintBox");
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

        public int Numero
        {
            get { return _numero; }
            set
            {
                _numero = value;
                RaisePropertyChanged("Numero");
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
            get { return _leis_comp; }
            set
            {
                _leis_comp = value;
                RaisePropertyChanged("Leis_Comp");
            }
        }

        public bool Decretos
        {
            get { return _dec; }
            set
            {
                _dec = value;
                RaisePropertyChanged("Decretos");
            }
        }
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

        private void ExecuteCommandSearch(object obj)
        {
            AsyncListarLegislacao(Parametros());
        }

        public ICommand CommandEdit
        {
            get
            {
                if (_commandEdit == null)
                    _commandEdit = new DelegateCommand(ExecuteCommandEdit, null);

                return _commandEdit;
            }
        }

        private void ExecuteCommandEdit(object obj)
        {

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(Properties.Resources.Sec_Governo_Legislacao_Edit, UriKind.RelativeOrAbsolute));
                    GlobalNavigation.Parametro = obj.ToString();
                }

                else
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Legislacao)
                        {
                            if (m.Acesso >= (int)SubModuloAccess.Operador)
                            {
                                ns.Navigate(new Uri(Properties.Resources.Sec_Governo_Legislacao_Edit, UriKind.RelativeOrAbsolute));
                                GlobalNavigation.Parametro = obj.ToString();
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("Acesso Negado!", "Sim.App.Alerta!");
        }

        public ICommand CommandShowPDF
        {
            get
            {
                if (_commandshowpdf == null)
                    _commandshowpdf = new DelegateCommand(ExecuteCommandShowPDF, null);
                return _commandshowpdf;
            }
        }

        private void ExecuteCommandShowPDF(object obj)
        {
            string link = (string)obj;

            string sitio = Properties.Settings.Default.PDFLegislacao;  //mXml.Listar("sim.apps", "App", "PDF", "Legislacao")[0];

            string urlLINK = string.Format(@"{0}{1}", sitio, link);

            Process p = new Process();
            ProcessStartInfo s = new ProcessStartInfo(urlLINK);
            p.StartInfo = s;
            p.Start();
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

        private void ExecuteCommandListar(object obj)
        {
            AsyncFlowDoc();
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

        public ICommand CommandClosePrintBox
        {
            get
            {
                if (_commandcloseprintbox == null)
                    _commandcloseprintbox = new DelegateCommand(ExecCommandClosePrintBox, null);
                return _commandcloseprintbox;
            }
        }

        private void ExecCommandClosePrintBox(object obj)
        {
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        }

        public ICommand CommandLimpar
        {
            get
            {
                if (_commandlimpar == null)
                    _commandlimpar = new DelegateCommand(ExecuteCommandLimpar, null);

                return _commandlimpar;
            }
        }

        private void ExecuteCommandLimpar(object obj)
        {
            Numero = 0;
            Leis = true;
            Leis_Comp = true;
            Decretos = true;
            ListaLegislativa.Clear();
            ViewFiltros = string.Empty;
            FlowDoc = null;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
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

        private void ExecuteCommandCancel(object obj)
        {
            
        }
        #endregion

        #region Constructor
        public vmConsulta()
        {
            Mvvm.Observers.GlobalNavigation.Pagina = "CONSULTA";
            ns = GlobalNavigation.NavService;
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;

            Leis = true;
            Decretos = true;
            Leis_Comp = true;
        }
        #endregion

        #region Methods

        #endregion

        #region Functions

        private List<object> Parametros()
        {

            List<object> _param = new List<object>() { };

            _param.Add(Leis);//0
            _param.Add(Leis_Comp);//1
            _param.Add(Decretos);//2
            _param.Add(Numero);//3

            return _param;
        }

        private void AsyncListarLegislacao(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;   
            StartProgress = true;

            string _leis = Leis ? "LEI " : "";
            string _leis_c = Leis_Comp ? "LEI COMPLEMENTAR " : "";
            string _dec = Decretos ? "DECRETO " : "";

            Filtros.Clear();

            Filtros.Add("[TIPO: " + _leis + _leis_c + _dec + "]");
            Filtros.Add("[NÚMERO: " + Numero.ToString() + "]");

            StringBuilder sb = new StringBuilder();
            foreach (string filtro in Filtros)
                sb.Append(filtro);

            ViewFiltros = sb.ToString();

            Task.Factory.StartNew(() => mdata.Legislacao_S(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListaLegislativa = task.Result;
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
          
            Task.Factory.StartNew(() => new vmFlowDocumentRel().CreateFlowDocument(ListaLegislativa, Filtros))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Background,                            
                            new Action(() =>
                            {
                                FlowDoc = new vmFlowDocumentRel().CreateFlowDocument(ListaLegislativa, Filtros);
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
