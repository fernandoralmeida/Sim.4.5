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
using System.Windows.Navigation;

namespace Sim.Sec.Governo.Portarias.ViewModel
{
    using Sim.Mvvm.Commands;
    using Sim.Mvvm.Observers;
    using Model;
    using Account;

    class vmConsulta : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        public mData mdata = new mData();
        private ObservableCollection<mPortaria> _lista = new ObservableCollection<mPortaria>();
        private FlowDocument _flowDocument = new FlowDocument();

        private List<string> _filters = new List<string>();

        private Visibility _blackbox;
        private Visibility _mainbox;
        private Visibility _viewprint;

        private bool _startprogress;
        private string _viewfiltros;

        private int _numero;

        private ICommand _commandsearch;
        private ICommand _commandlistar;
        private ICommand _commandcancel;
        private ICommand _commandclear;
        private ICommand _commandshowpdf;
        private ICommand _commandEdit;
        private ICommand _commandprint;
        private ICommand _commandcloseprintbox;
        private ICommand _commandpdfjo;
        #endregion

        #region Properties
        public ObservableCollection<mPortaria> ListaDoc
        {
            get { return _lista; }
            set
            {
                _lista = value;
                RaisePropertyChanged("ListaDoc");
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
            get { return _viewfiltros; }
            set
            {
                _viewfiltros = value;
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

        #endregion

        #region Commands
        public ICommand CommandPDFJO
        {
            get
            {
                if (_commandpdfjo == null)
                    _commandpdfjo = new DelegateCommand(ExecCommandPDFJO, null);
                return _commandpdfjo;
            }
        }

        private void ExecCommandPDFJO(object obj)
        {
            try
            {
                string jornal = (string)obj;

                string sitio = Properties.Settings.Default.JOPortarias;

                string urlLINK = string.Format(@"{0}Jornal_Oficial_{1}.pdf", sitio, jornal);

                Process p = new Process();
                ProcessStartInfo s = new ProcessStartInfo(urlLINK);
                p.StartInfo = s;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

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
            ExecCommandClosePrintBox(null);
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
                    ns.Navigate(new Uri(Properties.Resources.Sec_Governo_Portarias_Edit, UriKind.RelativeOrAbsolute));
                    Mvvm.Observers.GlobalNavigation.Parametro = obj.ToString();
                }

                else
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.Portarias)
                        {
                            if (m.Acesso >= (int)SubModuloAccess.Operador)
                            {
                                ns.Navigate(new Uri(Properties.Resources.Sec_Governo_Portarias_Edit, UriKind.RelativeOrAbsolute));
                                Mvvm.Observers.GlobalNavigation.Parametro = obj.ToString();
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
            try
            {
                string link = (string)obj;

                string sitio = Properties.Settings.Default.PDFPortarias;

                string urlLINK = string.Format(@"{0}{1}", sitio, link);

                Process p = new Process();
                ProcessStartInfo s = new ProcessStartInfo(urlLINK);
                p.StartInfo = s;
                p.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
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

        public ICommand CommandClear
        {
            get
            {
                if (_commandclear == null)
                    _commandclear = new DelegateCommand(ExecuteCommandClear, null);

                return _commandclear;
            }
        }

        private void ExecuteCommandClear(object obj)
        {
            Numero = 0;
            ListaDoc.Clear();
            FlowDoc = null;
            ViewFiltros = string.Empty;
            ExecCommandClosePrintBox(null);
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
            ns = Mvvm.Observers.GlobalNavigation.NavService;
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
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

        private void AsyncListarLegislacao(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Filtros.Clear();

            Filtros.Add("[NÚMERO: " + Numero.ToString() + "]");

            StringBuilder sb = new StringBuilder();
            foreach (string filtro in Filtros)
                sb.Append(filtro);

            ViewFiltros = sb.ToString();

            Task.Factory.StartNew(() => mdata.ConsultaSimples(Numero))
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
