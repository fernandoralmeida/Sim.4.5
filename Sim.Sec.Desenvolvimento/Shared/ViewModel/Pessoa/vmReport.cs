using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Pessoa
{
    using Model;
    using Controls;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Providers;

    class vmReport : NotifyProperty
    {
        #region Declarations
        private mData mdata = new mData();
        private mReportPessoas mReport = new mReportPessoas();
        private vmCharts ProvedorGrafico = new vmCharts();
        private ObservableCollection<mPF_Ext> _listapf = new ObservableCollection<mPF_Ext>();
        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();

        private FlowDocument flwdoc = new FlowDocument();

        private List<string> _filtros = new List<string>();

        private DateTime _datai;
        private DateTime _dataf;

        private int _getperfil;
        private int _getsexo;

        private Visibility _blackbox;
        private Visibility _printbox;
        private Visibility _mainbox;

        private bool _starprogress;
        private bool _comnegocio;

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

        public ObservableCollection<mTiposGenericos> ListaPerfis
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> ListaSexo
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PF_Sexo WHERE (Ativo = True) ORDER BY Valor"); }
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

        public int GetSexo
        {
            get { return _getsexo; }
            set { _getsexo = value; RaisePropertyChanged("GetSexo"); }
        }

        public int GetPerfil
        {
            get { return _getperfil; }
            set { _getperfil = value; RaisePropertyChanged("GetPerfil"); }
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

        public Visibility PrintBox
        {
            get { return _printbox; }
            set
            {
                _printbox = value;
                RaisePropertyChanged("PrintBox");
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

        public bool ComNegocio
        {
            get { return _comnegocio; }
            set
            {
                _comnegocio = value;
                RaisePropertyChanged("ComNegocio");
            }
        }

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
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
                AsyncListarPessoas(Parametros());
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
            if (Charts != null)
                Charts.Clear();

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
                //AsyncListarAtendimentoOperador(Parametros());
                //ListarViabilidades = mdata.Viabilidades(Parametros());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region Constructor
        public vmReport()
        {
            GlobalNavigation.Pagina = "RELATÓRIO PESSOAS";
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            StartProgress = false;
        }
        #endregion

        #region Functions
        private List<string> Parametros()
        {
            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString()); // 0
            _param.Add(DataF.ToShortDateString()); // 1          

            if (GetSexo == 0)
            {
                _param.Add("0"); //2
                _param.Add("2"); //3 
            }

            if (GetSexo == 1)
            {
                _param.Add("1"); //2
                _param.Add("1"); //3 
            }

            if (GetSexo == 2)
            {
                _param.Add("2"); //2
                _param.Add("2"); //3 
            }

            _param.Add(GetPerfil.ToString()); // 4
            _param.Add(ComNegocio.ToString()); // 5

            return _param;
        }

        private void AsyncListarPessoas(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.R_Pessoas(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        mReport.Clear();
                        mReport.Show(task.Result);
                        mReport.Periodo.Add(DataI.ToShortDateString());
                        mReport.Periodo.Add(DataF.ToShortDateString());
                        Charts = ProvedorGrafico.Pessoas(mReport, true);
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

            Task.Factory.StartNew(() => mdata.R_Pessoas(Parametros())).ContinueWith(task =>
            {

                Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    FlowDoc = PreviewInTable(mReport);
                }));

                if (task.IsCompleted)
                {
                    mReport.Clear();
                    mReport.Show(task.Result);
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

            Figure lrodape = new Figure();
            lrodape.Width = new FigureLength(1000);
            lrodape.Height = new FigureLength(1);
            lrodape.Background = Brushes.Gray;
            lrodape.Margin = new Thickness(0, 20, 0, 0);

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

        private FlowDocument PreviewInTable(mReportPessoas obj)
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

                #region Tabela
                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                foreach (KeyValuePair<string, int> x in obj.Perfil)
                {
                    tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                }

                TableRowGroup rg = new TableRowGroup();

                string f = string.Empty;
                foreach (string filtro in Filtros)
                    f += filtro;

                TableRow pheader = new TableRow();
                pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("PERFIL DE PESSOAS ATENDIDAS ENTRE {0} {1}", Parametros()[0], Parametros()[1]))) { Padding = new Thickness(5) }) { ColumnSpan = obj.Perfil.Count, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(pheader);
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;

                foreach (KeyValuePair<string, int> x in obj.Perfil)
                    header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(x.Key))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                if (obj != null)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);

                    foreach (KeyValuePair<string, int> x in obj.Perfil)
                    {
                        row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    }

                }

                flow.Blocks.Add(tb);
                #endregion

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

                TableRow hcnae = new TableRow();
                hcnae.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("PESSOAS POR SEXO")))) { Padding = new Thickness(5) }) { ColumnSpan = 2, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                tb1.RowGroups.Add(rg1);
                rg1.Rows.Add(hcnae);

                if (obj != null)
                {

                    foreach (KeyValuePair<string, int> x in obj.Sexo)
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

                TableRow tbairro = new TableRow();
                tbairro.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("PESSOAS POR BAIRRO")))) { Padding = new Thickness(5) }) { ColumnSpan = 2, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                tb2.RowGroups.Add(rg2);

                rg2.Rows.Add(tbairro);

                if (obj != null)
                {
                    foreach (KeyValuePair<string, int> x in obj.Setor)
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
            catch
            {
                return new FlowDocument();
            }
        }
        #endregion
    }
}
