using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.ViewModel
{
    using Model;
    using Shared.Model;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Controls;
    using Controls.ViewModels;


    class vmReportDIA : VMBase, IPrintBox
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private mDataCM mdatacm = new mDataCM();
        private mDataReport mdatar = new mDataReport();
        private ObservableCollection<mAmbulante> _listaat = new ObservableCollection<mAmbulante>();
        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();
        private FlowDocument flwdoc = new FlowDocument();

        private DateTime _datai;
        private DateTime _dataf;

        private List<string> _filtros = new List<string>();

        private string _getatividade = string.Empty;
        private string _getlocal = string.Empty;
        private int _getsituacao = 0;

        private Visibility _mainbox;

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

        public ObservableCollection<BarChart> Charts
        {
            get { return _charts; }
            set
            {
                _charts = value;
                RaisePropertyChanged("Charts");
            }
        }

        public ObservableCollection<mTiposGenericos> Situações
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_CAmbulante_Situacao WHERE (Ativo = True) ORDER BY Valor"); }
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

        public string GetAtividade
        {
            get { return _getatividade; }
            set { _getatividade = value; RaisePropertyChanged("GetAtividade"); }
        }

        public string GetLocal
        {
            get { return _getlocal; }
            set { _getlocal = value; RaisePropertyChanged("GetLocal"); }
        }

        public int GetSituacao
        {
            get { return _getsituacao; }
            set
            {
                _getsituacao = value;
                RaisePropertyChanged("GetSituacao");
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

        #endregion

        #region Commands
        public ICommand CommandFiltrar => new RelayCommand(p =>
        {

            AsyncListarAtendimento(Parametros());

        });

        public ICommand CommandLimpar => new RelayCommand(p =>
        {
            if (Charts != null)
                Charts.Clear();

            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        });

        public ICommand CommandPrint => new RelayCommand(p =>
        {
            try
            {
                StartProgress = true;
                BlackBox = Visibility.Visible;

                // paginador
                //FlowDoc.PageHeight = 768;
                //FlowDoc.PageWidth = 1104;
                //FlowDoc.ColumnWidth = 1104;
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

        public ICommand CommandListar => new RelayCommand(p =>
        {
            AsyncFlowDoc();
        });

        public ICommand CommandClosePrintBox => new RelayCommand(p =>
        {
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        });

        public ICommand CommandAlterar => new RelayCommand(p =>
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(@"/Sim.Modulo.cAmbulante;component/View/pCAmbulante.xaml", UriKind.Relative));
                    AreaTransferencia.CPF = (string)p;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                ns.Navigate(new Uri(@"/Sim.Modulo.cAmbulante;component/View/pCAmbulante.xaml", UriKind.Relative));
                                AreaTransferencia.CPF = (string)p;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }
        });
        #endregion

        #region Constructor
        public vmReportDIA()
        {
            GlobalNavigation.Pagina = "RELATÓRIO";
            ns = GlobalNavigation.NavService;
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            StartProgress = false;
        }
        #endregion

        #region Functions
        private List<string> Parametros()
        {

            List<string> _param = new List<string>() { };

            Filtros.Clear();

            //_param.Add(DataI.ToShortDateString());
            //_param.Add(DataF.ToShortDateString());

            if (GetAtividade == string.Empty || GetAtividade == null)
            {
                _param.Add("%");//0
                Filtros.Add("[ATIVIDADE = TODOS]");
            }
            else
            {
                _param.Add(GetAtividade);//0
                Filtros.Add("[ATIVIDADE = ]" + GetAtividade + "]");
            }

            if (GetLocal == string.Empty || GetLocal == null)
            {
                _param.Add("%");//1
                Filtros.Add("[LOCAL = TODOS]");
            }
            else
            {
                _param.Add(GetLocal);//1
                Filtros.Add("[LOCAL = " + GetLocal + "]");
            }

            if (GetSituacao < 1)
            {
                _param.Add("0");//2
                _param.Add("99");//3
                Filtros.Add("[SITUÇÃO = TODOS]");
            }
            else
            {
                _param.Add(GetSituacao.ToString());//2
                _param.Add(GetSituacao.ToString());//3
                Filtros.Add("[SITUÇÃO = " + Situações[GetSituacao].Nome + "]");
            }

            return _param;
        }

        private async void AsyncListarAtendimento(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            var t = Task.Factory.StartNew(() => mdatar.ComercioAmbulante(mdatacm.RCAmbulantes(sqlcommand)))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Charts = AsyncChart(task.Result);
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                });
            await t;
        }

        private async void AsyncFlowDoc()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            mReportCA mrca = new mReportCA();
            var t = Task.Factory.StartNew(() => mdatar.ComercioAmbulante(mdatacm.RCAmbulantes(Parametros()))).ContinueWith(task =>
            {

                Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    FlowDoc = PreviewInTable(mrca);
                }));

                if (task.IsCompleted)
                {
                    mrca = task.Result;
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                }
            });
            await t;
        }

        private ObservableCollection<BarChart> AsyncChart(mReportCA obj)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();

            Task.Factory.StartNew(new Action(delegate ()
            {
                mReportCA _rp = new mReportCA();

                foreach (KeyValuePair<string, int> x in obj.Ambulante)
                {
                    _rp.Ambulante.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in obj.Atividade)
                {
                    _rp.Atividade.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in obj.Local)
                {
                    _rp.Local.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in obj.Instalacoes)
                {
                    _rp.Instalacoes.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in obj.Periodos)
                {
                    _rp.Periodos.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in obj.Situacao)
                {
                    _rp.Situacao.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in obj.TempoAtividade)
                {
                    _rp.TempoAtividade.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                if (_rp.Ambulante.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("COMERCIO AMBULANTE");
                    bc.ItemsSource = _rp.Ambulante;
                    _chart.Add(bc);
                }

                if (_rp.Atividade.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("ATIVIDADES");
                    bc.ItemsSource = _rp.Atividade;

                    _chart.Add(bc);
                }

                if (_rp.TempoAtividade.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("TEMPO DE ATIVIDADES (MESES)");
                    bc.ItemsSource = _rp.TempoAtividade;

                    _chart.Add(bc);
                }

                if (_rp.Local.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("LOCAL");
                    bc.ItemsSource = _rp.Local;

                    _chart.Add(bc);
                }

                if (_rp.Periodos.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("PERIODO DE TRABALHO");
                    bc.ItemsSource = _rp.Periodos;

                    _chart.Add(bc);
                }

                if (_rp.Instalacoes.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("TIPOS DE INSTALAÇÕES");
                    bc.ItemsSource = _rp.Instalacoes;

                    _chart.Add(bc);
                }

                if (_rp.Situacao.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("SITUAÇÃO");
                    bc.ItemsSource = _rp.Situacao;

                    _chart.Add(bc);
                }
            }));

            return _chart;
        }

        private ObservableCollection<BarChart> Chart(mReportCA obj)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();

            mReportCA _rp = new mReportCA();

            foreach (KeyValuePair<string, int> x in obj.Ambulante)
            {
                _rp.Ambulante.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in obj.Atividade)
            {
                _rp.Atividade.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in obj.Local)
            {
                _rp.Local.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in obj.Instalacoes)
            {
                _rp.Instalacoes.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in obj.Periodos)
            {
                _rp.Periodos.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in obj.Situacao)
            {
                _rp.Situacao.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in obj.TempoAtividade)
            {
                _rp.TempoAtividade.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            if (_rp.Ambulante.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("COMERCIO AMBULANTE");
                bc.ItemsSource = _rp.Ambulante;
                _chart.Add(bc);
            }

            if (_rp.Atividade.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("ATIVIDADES");
                bc.ItemsSource = _rp.Atividade;

                _chart.Add(bc);
            }

            if (_rp.TempoAtividade.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("TEMPO DE ATIVIDADES");
                bc.ItemsSource = _rp.TempoAtividade;

                _chart.Add(bc);
            }

            if (_rp.Local.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("LOCAL");
                bc.ItemsSource = _rp.Local;

                _chart.Add(bc);
            }

            if (_rp.Periodos.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("PERIODO DE TRABALHO");
                bc.ItemsSource = _rp.Periodos;

                _chart.Add(bc);
            }

            if (_rp.Instalacoes.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("TIPOS DE INSTALAÇÕES");
                bc.ItemsSource = _rp.Instalacoes;

                _chart.Add(bc);
            }

            if (_rp.Situacao.Count > 0)
            {

                BarChart bc = new BarChart();

                bc.Title = string.Format("SITUAÇÃO");
                bc.ItemsSource = _rp.Situacao;

                _chart.Add(bc);
            }

            return _chart;
        }

        private FlowDocument PreviewInTable(mReportCA obj)
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
            pH1.Margin = new Thickness(2, 0, 0, 0);

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

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro;

            Paragraph p = new Paragraph();
            p.Padding = new Thickness(5);
            p = new Paragraph(new Run(string.Format("FILTROS: {0}", f)));
            flow.Blocks.Add(p);

            #region  Total Ambulante
            if (obj.Ambulante.Count > 0 || obj != null)
            {

                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(70) });

                flow.Blocks.Add(tb);

                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;
                header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Ambulantes"))) { Padding = new Thickness(5) }) { ColumnSpan = 3, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                int t = 0;

                foreach (KeyValuePair<string, int> x in obj.Ambulante)
                {
                    t += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Ambulante)
                {
                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    double v1 = x.Value;
                    double v2 = t;

                    double res = v1 / v2;

                    row.Cells.Add(new TableCell(new Paragraph(new Run(res.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                }

                TableRow row_t = new TableRow();
                rg.Foreground = Brushes.Black;
                rg.Rows.Add(row_t);
                row_t.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                row_t.Cells.Add(new TableCell(new Paragraph(new Run(t.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                double res_t = t / t;

                row_t.Cells.Add(new TableCell(new Paragraph(new Run(res_t.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });


                flow.Blocks.Add(tb);
            }
            #endregion

            #region  Local
            if (obj.Local.Count > 0 || obj != null)
            {

                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(70) });

                flow.Blocks.Add(tb);

                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;
                header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Local da Atividade"))) { Padding = new Thickness(5) }) { ColumnSpan = 3, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                int t = 0;

                foreach (KeyValuePair<string, int> x in obj.Local)
                {
                    t += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Local)
                {
                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    double v1 = x.Value;
                    double v2 = t;

                    double res = v1 / v2;

                    row.Cells.Add(new TableCell(new Paragraph(new Run(res.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                }

                TableRow row_t = new TableRow();
                rg.Foreground = Brushes.Black;
                rg.Rows.Add(row_t);
                row_t.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                row_t.Cells.Add(new TableCell(new Paragraph(new Run(t.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                double res_t = t / t;

                row_t.Cells.Add(new TableCell(new Paragraph(new Run(res_t.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });


                flow.Blocks.Add(tb);
            }
            #endregion

            #region  Atividades
            if (obj.Atividade.Count > 0 || obj != null)
            {

                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(70) });

                flow.Blocks.Add(tb);

                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;
                header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Atividade"))) { Padding = new Thickness(5) }) { ColumnSpan = 3, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                int t = 0;

                foreach (KeyValuePair<string, int> x in obj.Atividade)
                {
                    t += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Atividade)
                {
                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    double v1 = x.Value;
                    double v2 = t;

                    double res = v1 / v2;

                    row.Cells.Add(new TableCell(new Paragraph(new Run(res.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                }

                TableRow row_t = new TableRow();
                rg.Foreground = Brushes.Black;
                rg.Rows.Add(row_t);
                row_t.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                row_t.Cells.Add(new TableCell(new Paragraph(new Run(t.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                double res_t = t / t;

                row_t.Cells.Add(new TableCell(new Paragraph(new Run(res_t.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                flow.Blocks.Add(tb);
            }
            #endregion

            #region  Tempo de Atividade
            if (obj.TempoAtividade.Count > 0 || obj != null)
            {

                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(70) });

                flow.Blocks.Add(tb);

                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;
                header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Tempo de Atividade"))) { Padding = new Thickness(5) }) { ColumnSpan = 3, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                int t = 0;

                foreach (KeyValuePair<string, int> x in obj.TempoAtividade)
                {
                    t += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.TempoAtividade)
                {
                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key + " meses")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    double v1 = x.Value;
                    double v2 = t;

                    double res = v1 / v2;

                    row.Cells.Add(new TableCell(new Paragraph(new Run(res.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                }

                TableRow row_t = new TableRow();
                rg.Foreground = Brushes.Black;
                rg.Rows.Add(row_t);
                row_t.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                row_t.Cells.Add(new TableCell(new Paragraph(new Run(t.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                double res_t = t / t;

                row_t.Cells.Add(new TableCell(new Paragraph(new Run(res_t.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                flow.Blocks.Add(tb);
            }
            #endregion

            #region  Periodos
            if (obj.Periodos.Count > 0 || obj != null)
            {

                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(70) });

                flow.Blocks.Add(tb);

                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;
                header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Horário de trabalho"))) { Padding = new Thickness(5) }) { ColumnSpan = 3, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                int t = 0;

                foreach (KeyValuePair<string, int> x in obj.Periodos)
                {
                    t += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Periodos)
                {
                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    double v1 = x.Value;
                    double v2 = t;

                    double res = v1 / v2;

                    row.Cells.Add(new TableCell(new Paragraph(new Run(res.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                }

                TableRow row_t = new TableRow();
                rg.Foreground = Brushes.Black;
                rg.Rows.Add(row_t);
                row_t.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                row_t.Cells.Add(new TableCell(new Paragraph(new Run(t.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                double res_t = t / t;

                row_t.Cells.Add(new TableCell(new Paragraph(new Run(res_t.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                flow.Blocks.Add(tb);
            }
            #endregion

            #region  Instalações
            if (obj.Instalacoes.Count > 0 || obj != null)
            {

                Table tb = new Table();
                tb.CellSpacing = 0;
                tb.BorderThickness = new Thickness(0.5);
                tb.BorderBrush = Brushes.Black;

                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(100) });
                tb.Columns.Add(new TableColumn() { Width = new GridLength(70) });

                flow.Blocks.Add(tb);

                TableRowGroup rg = new TableRowGroup();

                tb.RowGroups.Add(rg);

                TableRow header = new TableRow();
                rg.Rows.Add(header);
                rg.Foreground = Brushes.Black;
                header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Tipo de Instalações"))) { Padding = new Thickness(5) }) { ColumnSpan = 3, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                int t = 0;

                foreach (KeyValuePair<string, int> x in obj.Instalacoes)
                {
                    t += x.Value;
                }

                foreach (KeyValuePair<string, int> x in obj.Instalacoes)
                {
                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Key)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(x.Value.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    double v1 = x.Value;
                    double v2 = t;

                    double res = v1 / v2;

                    row.Cells.Add(new TableCell(new Paragraph(new Run(res.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                }

                TableRow row_t = new TableRow();
                rg.Foreground = Brushes.Black;
                rg.Rows.Add(row_t);
                row_t.Cells.Add(new TableCell(new Paragraph(new Run("Total")) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                row_t.Cells.Add(new TableCell(new Paragraph(new Run(t.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                double res_t = t / t;

                row_t.Cells.Add(new TableCell(new Paragraph(new Run(res_t.ToString("0.00%"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                flow.Blocks.Add(tb);
            }
            #endregion

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            flow.Blocks.Add(r);

            return flow;
        }

        private FlowDocument PreviwChart(ObservableCollection<BarChart> obj)
        {
            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            flow.Background = Brushes.White;
            //flow.ColumnWidth = 8.5 * 96.0;
            flow.ColumnWidth = 96.0 * 8.5;
            flow.PageHeight = 11.5 * 96.0;
            flow.PageWidth = 8.5 * 96.0;
            //flow.PageHeight = 768;
            //flow.PageWidth = 1104;
            //flow.ColumnWidth = 1104;
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
            pH1.Margin = new Thickness(2, 0, 0, 0);

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

            /*
            var lfiltro = new Rectangle();
            lfiltro.Stroke = new SolidColorBrush(Colors.Silver);
            lfiltro.StrokeThickness = 1;
            lfiltro.Height = 1;
            lfiltro.Width = double.NaN;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            var linefiltro = new BlockUIContainer(lfiltro);
            flow.Blocks.Add(linefiltro);            
            */

            //SolidColorBrush bgc1 = Application.Current.Resources["ButtonBackgroundHover"] as SolidColorBrush;

            Table tb = new Table();
            tb.CellSpacing = 0;
            tb.BorderThickness = new Thickness(0.5);
            tb.BorderBrush = Brushes.Black;

            tb.Columns.Add(new TableColumn() { Width = new GridLength(50) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });

            TableRowGroup rg = new TableRowGroup();

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro;

            TableRow pheader = new TableRow();
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("FILTROS: {0}", f))) { Padding = new Thickness(5) }) { ColumnSpan = 4, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("GRÁFICOS: {0}", obj.Count))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            tb.RowGroups.Add(rg);

            Paragraph p = new Paragraph();
            p.Padding = new Thickness(5);

            TableRow header = new TableRow();
            rg.Rows.Add(pheader);
            rg.Rows.Add(header);
            rg.Foreground = Brushes.Black;

            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(""))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            if (obj != null)
            {

                int alt = 1;

                foreach (BarChart bc in obj)
                {
                    TableRow row = new TableRow();
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(alt.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new BlockUIContainer(bc)) { ColumnSpan = 4, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    alt++;
                }
            }

            flow.Blocks.Add(tb);

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            flow.Blocks.Add(r);

            return flow;
        }

        #endregion
    }
}
