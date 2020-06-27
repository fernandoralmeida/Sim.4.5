using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Controls;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Atendimento
{

    using Model;
    using Controls;
    using Controls.ViewModels;
    using Providers;
    using Mvvm.Commands;
    using Mvvm.Observers;

    class vmReports : VMBase
    {
        #region Declarations
        private mData mdata = new mData();
        private mReportAtendimentos mReport = new mReportAtendimentos();
        private vmCharts ProvedorGrafico = new vmCharts();
        private ObservableCollection<mAtendimento> _listaat = new ObservableCollection<mAtendimento>();
        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();
        private FlowDocument flwdoc = new FlowDocument();

        private List<string> _filtros = new List<string>();

        private DateTime _datai;
        private DateTime _dataf;

        private string _getcpf;
        private string _getnome;
        private string _gettipos;
        private int _getorigens;

        private Visibility _mainbox;

        private bool _isoperador;
        private bool _isorigem;

        private ICommand _commandfiltrar;
        private ICommand _commandlimpar;
        private ICommand _commandprint;
        private ICommand _commandmeusatendimentos;
        private ICommand _commandcloseprintbox;
        private ICommand _commandlistar;
        #endregion

        #region Properties

        public FlowDocument FlowDoc
        {
            get { return flwdoc; }
            set
            {
                flwdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public ObservableCollection<mAtendimento> ListarAtendimentos
        {
            get { return _listaat; }
            set
            {
                _listaat = value;
                RaisePropertyChanged("ListarAtendimentos");
            }
        }

        public ObservableCollection<BarChart> Charts
        {
            get { return _charts; }
            set
            {
                _charts = value;
                RaisePropertyChanged("Charts");
            }
        }

        public ObservableCollection<mTiposGenericos> Origens
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Origem WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_Atendimento_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public ObservableCollection<string> Servicos
        {
            get
            {
                ObservableCollection<string> _sv = new ObservableCollection<string>();
                foreach (mTiposGenericos s in Tipos)
                {
                    _sv.Add(s.Nome);
                }
                return _sv;
            }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set { _datai = value; RaisePropertyChanged("DataI"); }
        }

        public DateTime DataF
        {
            get { return _dataf; }
            set { _dataf = value; RaisePropertyChanged("DataF"); }
        }

        public List<string> Filtros
        {
            get { return _filtros; }
            set
            {
                _filtros = value;
                RaisePropertyChanged("Filtros");
            }
        }

        public string GetCNPJCPF
        {
            get { return _getcpf; }
            set
            {
                _getcpf = value;
                RaisePropertyChanged("GetCNPJCPF");
            }
        }

        public string GetProtocolo
        {
            get { return _getnome; }
            set { _getnome = value; RaisePropertyChanged("GetProtocolo"); }
        }

        public string GetTipos
        {
            get { return _gettipos; }
            set
            {
                _gettipos = value;
                RaisePropertyChanged("GetTipos");
            }
        }

        public int GetOrigens
        {
            get { return _getorigens; }
            set
            {
                _getorigens = value;
                RaisePropertyChanged("GetOrigens");
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

        public bool isOrigem
        {
            get { return _isorigem; }
            set
            {
                _isorigem = value;
                RaisePropertyChanged("isOrigem");
            }
        }

        public bool isOperador
        {
            get { return _isoperador; }
            set
            {
                _isoperador = value;
                RaisePropertyChanged("isOperador");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandFiltrar
        {
            get
            {
                if (_commandfiltrar == null)
                    _commandfiltrar = new DelegateCommand(ExecCommandFiltrar, null);
                return _commandfiltrar;
            }
        }

        private void ExecCommandFiltrar(object obj)
        {
            try
            {
                AsyncListarAtendimento(Parametros());
                //ListarViabilidades = mdata.Viabilidades(Parametros());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public ICommand CommandLimpar
        {
            get
            {
                if (_commandlimpar == null)
                    _commandlimpar = new DelegateCommand(ExecCommandLimpar, null);
                return _commandlimpar;
            }
        }

        private void ExecCommandLimpar(object obj)
        {
            if (ListarAtendimentos != null)
                ListarAtendimentos.Clear();

            GetCNPJCPF = string.Empty;
            GetProtocolo = string.Empty;
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        }

        public ICommand CommandPrint
        {
            get
            {
                if (_commandprint == null)
                    _commandprint = new RelayCommand(p => {

                        try
                        {
                            StartProgress = true;
                            BlackBox = Visibility.Visible;

                            // paginador
                            FlowDoc.PageHeight = 768;
                            FlowDoc.PageWidth = 1104;
                            FlowDoc.ColumnWidth = 1104;
                            IDocumentPaginatorSource idocument = FlowDoc as IDocumentPaginatorSource;
                            idocument.DocumentPaginator.ComputePageCountAsync();

                            //Now print using PrintDialog
                            var pd = new PrintDialog();
                            pd.UserPageRangeEnabled = true;
                            //pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                            if (pd.ShowDialog().Value)
                                pd.PrintDocument(idocument.DocumentPaginator, "Printing....");

                            StartProgress = false;
                            BlackBox = Visibility.Collapsed;
                        }
                        catch (Exception ex)
                        {
                            StartProgress = false;
                            BlackBox = Visibility.Collapsed;
                            MessageBox.Show(ex.Message);
                        }

                    });
                return _commandprint;
            }
        }

        public ICommand CommandListar
        {
            get
            {
                if (_commandlistar == null)
                    _commandlistar = new RelayCommand(p => {
                        AsyncFlowDoc();
                    });
                return _commandlistar;
            }
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

        public ICommand CommandMeusAtendimentos
        {
            get
            {
                if (_commandmeusatendimentos == null)
                    _commandmeusatendimentos = new DelegateCommand(ExecCommandMeusAtendimentos, null);
                return _commandmeusatendimentos;
            }
        }

        private void ExecCommandMeusAtendimentos(object obj)
        {
            try
            {
                AsyncListarAtendimentoOperador(Parametros());
                //ListarViabilidades = mdata.Viabilidades(Parametros());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region Constructor
        public vmReports()
        {
            GlobalNavigation.Pagina = "RELATÓRIO ATENDIMENTOS";
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            StartProgress = false;
            GetTipos = "...";
            GetOrigens = 0;
            isOrigem = true;
            isOperador = false;
        }
        #endregion

        #region Functions
        private List<string> Parametros()
        {

            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString());
            _param.Add(DataF.ToShortDateString());

            if (GetOrigens < 1)
            {
                _param.Add("0");
                _param.Add("3");
            }
            else
            {
                _param.Add(GetOrigens.ToString());
                _param.Add(GetOrigens.ToString());
            }

            if (GetTipos == "...")
            {
                _param.Add("%");
                _param.Add("%");
            }
            else
            {
                _param.Add(GetTipos);
                _param.Add(GetTipos);
            }

            _param.Add(Account.Logged.Identificador);

            return _param;
        }

        private List<string> ParametrosOperador()
        {

            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString());
            _param.Add(DataF.ToShortDateString());

            if (GetTipos == "...")
            {
                _param.Add("%");
                _param.Add("%");
            }
            else
            {
                _param.Add(GetTipos);
                _param.Add(GetTipos);
            }

            _param.Add(Account.Logged.Nome);

            return _param;
        }

        private void AsyncListarAtendimento(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.RAtendimentos(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        mReport.Clear();
                        mReport.Show(task.Result);
                        mReport.Periodo.Add(DataI.ToShortDateString());
                        mReport.Periodo.Add(DataF.ToShortDateString());
                        Charts = ProvedorGrafico.Atendimento(mReport, true);
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

        private void AsyncListarAtendimentoOperador(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.RAtendimentosOperador(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        mReport.Clear();
                        mReport.Show(task.Result);
                        mReport.Periodo.Add(DataI.ToShortDateString());
                        mReport.Periodo.Add(DataF.ToShortDateString());
                        Charts = ProvedorGrafico.Atendimento(mReport, true);
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
            /*
            new System.Threading.Thread(() =>
            {
                System.Windows.Threading.DispatcherOperation op =
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate () {
                        FlowDoc = PreviewInTable(mReport);
                    }));

                op.Completed += (o, args) =>
                {
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                };
            }).Start();*/

            Task.Factory.StartNew(() => mdata.RAtendimentos(Parametros())).ContinueWith(task =>
            {

                Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    FlowDoc = PreviewInTable(mReport);
                }));

                if (task.IsCompleted)
                {
                    //mReport.Clear();
                    //mReport.Show(task.Result);
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                }

            });
        }

        private FlowDocument PreviewPrint(ObservableCollection<BarChart> obj)
        {
            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            flow.Foreground = Brushes.Black;
            flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            flow.ColumnWidth = 8.5 * 96.0;
            flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 11;
            flow.PagePadding = new Thickness(40, 20, 40, 40);

            Paragraph pH = new Paragraph(new Run(new mFlowHeader().NameOrg));
            pH.Typography.Capitals = FontCapitals.SmallCaps;
            pH.FontSize = 16;
            pH.FontWeight = FontWeights.Bold;
            pH.Margin = new Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(new mFlowHeader().SloganOrg));
            pH1.FontSize = 9;
            pH1.Margin = new Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(new mFlowHeader().DepOrg));
            pH2.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH2.FontSize = 12;
            pH2.Margin = new Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(new mFlowHeader().SetorOrg));
            pH3.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH3.Margin = new Thickness(0, 0, 0, 20);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro;

            Figure lfiltro = new Figure();
            lfiltro.Width = new FigureLength(flow.PageHeight);
            lfiltro.Height = new FigureLength(1);
            lfiltro.Background = Brushes.Gray;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            Paragraph ft = new Paragraph();
            ft.Margin = new Thickness(0, 0, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            Paragraph ft1 = new Paragraph();
            ft1.Margin = new Thickness(0, 0, 0, 0);
            ft1.FontSize = 10;
            ft1.Inlines.Add(new Run("[REG. ENCONTRADO(S): "));
            ft1.Inlines.Add(new Bold(new Run(obj.Count.ToString())));
            ft1.Inlines.Add(new Run("]"));

            flow.Blocks.Add(ft);
            flow.Blocks.Add(ft1);
            //flow.Blocks.Add(new Paragraph(lfiltro) { Margin = new Thickness(0, 0, 0, 10) });

            foreach (BarChart charts in obj)
            {
                flow.Blocks.Add(new BlockUIContainer(charts));
            }

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            //r.Inlines.Add(lrodape);
            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Account.Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            flow.Blocks.Add(r);

            return flow;
        }

        private FlowDocument PreviewInTable(mReportAtendimentos obj)
        {
            try
            {
                FlowDocument flow = new FlowDocument();

                flow.ColumnGap = 0;
                flow.Background = Brushes.White;
                //flow.ColumnWidth = 8.5 * 96.0;
                //flow.ColumnWidth =  96.0 * 8.5;
                //flow.PageHeight = 11.5 * 96.0;
                //flow.PageWidth = 8.5 * 96.0;
                flow.PageHeight = 768;
                flow.PageWidth = 1104;
                flow.ColumnWidth = 1104;
                flow.FontFamily = new FontFamily("Segoe UI");
                flow.FontSize = 11;
                flow.PagePadding = new Thickness(40, 20, 40, 20);
                flow.TextAlignment = TextAlignment.Left;

                Paragraph pH = new Paragraph(new Run(new mFlowHeader().NameOrg));
                pH.Typography.Capitals = FontCapitals.SmallCaps;
                pH.Foreground = Brushes.Black;
                pH.FontSize = 16;
                pH.FontWeight = FontWeights.Bold;
                pH.Margin = new Thickness(0);

                Paragraph pH1 = new Paragraph(new Run(new mFlowHeader().SloganOrg));
                pH1.Foreground = Brushes.Black;
                pH1.FontSize = 9;
                pH1.Margin = new Thickness(1, 0, 0, 0);

                Paragraph pH2 = new Paragraph(new Run(new mFlowHeader().DepOrg));
                pH2.Typography.Capitals = FontCapitals.SmallCaps;
                pH2.Foreground = Brushes.Black;
                pH2.FontWeight = FontWeights.Bold;
                pH2.FontSize = 12;
                pH2.Margin = new Thickness(0, 10, 0, 0);

                Paragraph pH3 = new Paragraph(new Run(new mFlowHeader().SetorOrg));
                pH3.Typography.Capitals = FontCapitals.SmallCaps;
                pH3.FontWeight = FontWeights.Bold;
                pH3.Foreground = Brushes.Black;
                pH3.Margin = new Thickness(0, 0, 0, 20);

                flow.Blocks.Add(pH);
                flow.Blocks.Add(pH1);
                flow.Blocks.Add(pH2);
                flow.Blocks.Add(pH3);

                #region Tabela 1
                Table tb1 = new Table();
                tb1.Foreground = Brushes.Black;
                tb1.CellSpacing = 0;
                tb1.BorderThickness = new Thickness(0.5);
                tb1.BorderBrush = Brushes.Black;

                tb1.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb1.Columns.Add(new TableColumn() { Width = new GridLength(100) });

                TableRowGroup rg1 = new TableRowGroup();
                rg1.Foreground = Brushes.Black;

                tb1.RowGroups.Add(rg1);

                TableRow hcnae1 = new TableRow();
                rg1.Rows.Add(hcnae1);

                int cont = 0;

                if (obj != null)
                    foreach (KeyValuePair<string, int> x in obj.Atendimentos)
                        cont = x.Value;

                hcnae1.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ATENDIMENTOs"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                hcnae1.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(cont.ToString()))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                if (obj != null)
                {

                    foreach (KeyValuePair<string, int> x in obj.Ano)
                    {
                        TableRow row = new TableRow();
                        rg1.Foreground = Brushes.Black;
                        rg1.Rows.Add(row);
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    }
                }

                flow.Blocks.Add(tb1);
                #endregion

                #region Tabela 2
                Table tb2 = new Table();
                tb2.Foreground = Brushes.Black;
                tb2.CellSpacing = 0;
                tb2.BorderThickness = new Thickness(0.5);
                tb2.BorderBrush = Brushes.Black;

                tb2.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb2.Columns.Add(new TableColumn() { Width = new GridLength(100) });

                TableRowGroup rg2 = new TableRowGroup();
                rg2.Foreground = Brushes.Black;

                tb2.RowGroups.Add(rg2);

                TableRow hcnae2 = new TableRow();
                rg2.Rows.Add(hcnae2);

                hcnae2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("SERVIÇOS REALIZADOS"))) { Padding = new Thickness(5) }) { ColumnSpan = 2, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                if (obj != null)
                {

                    foreach (KeyValuePair<string, int> x in obj.Tipo)
                    {
                        TableRow row = new TableRow();
                        rg2.Foreground = Brushes.Black;
                        rg2.Rows.Add(row);
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    }
                }

                flow.Blocks.Add(tb2);
                #endregion

                if (isOrigem)
                {
                    #region Tabela 3
                    Table tb3 = new Table();
                    tb3.Foreground = Brushes.Black;
                    tb3.CellSpacing = 0;
                    tb3.BorderThickness = new Thickness(0.5);
                    tb3.BorderBrush = Brushes.Black;

                    tb3.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                    tb3.Columns.Add(new TableColumn() { Width = new GridLength(100) });

                    TableRowGroup rg3 = new TableRowGroup();

                    tb3.RowGroups.Add(rg3);

                    TableRow tbairro3 = new TableRow();
                    rg3.Rows.Add(tbairro3);

                    tbairro3.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ORIGEM DO ATENDIMENTO"))) { Padding = new Thickness(5) }) { ColumnSpan = 2, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (obj != null)
                    {

                        foreach (KeyValuePair<string, int> x in obj.Origem)
                        {
                            TableRow row = new TableRow();
                            rg3.Foreground = Brushes.Black;
                            rg3.Rows.Add(row);
                            row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                            row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                        }
                    }

                    flow.Blocks.Add(tb3);
                    #endregion
                }

                if (isOperador)
                {
                    #region Tabela 4
                    Table tb4 = new Table();
                    tb4.Foreground = Brushes.Black;
                    tb4.CellSpacing = 0;
                    tb4.BorderThickness = new Thickness(0.5);
                    tb4.BorderBrush = Brushes.Black;

                    tb4.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                    tb4.Columns.Add(new TableColumn() { Width = new GridLength(100) });

                    TableRowGroup rg4 = new TableRowGroup();

                    tb4.RowGroups.Add(rg4);

                    TableRow tOP = new TableRow();
                    rg4.Rows.Add(tOP);

                    tOP.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("OPERADOR DO ATENDIMENTO"))) { Padding = new Thickness(5) }) { ColumnSpan = 2, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (obj != null)
                    {

                        foreach (KeyValuePair<string, int> x in obj.Operador)
                        {
                            TableRow row = new TableRow();
                            rg4.Foreground = Brushes.Black;
                            rg4.Rows.Add(row);
                            row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                            row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                        }
                    }

                    flow.Blocks.Add(tb4);
                    #endregion
                }

                Paragraph r = new Paragraph();
                r.Margin = new Thickness(0, 0, 0, 0);
                r.FontSize = 10;

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;

                //r.Inlines.Add(lrodape);
                r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
                r.Inlines.Add(new Run(" / V" + version + " / "));
                r.Inlines.Add(new Bold(new Run(Account.Logged.Nome)));
                r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
                r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));

                flow.Blocks.Add(r);

                return flow;
            }
            catch { return new FlowDocument(); }
        }
        #endregion
    }
}
