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
    using Sim.Controls.ViewModels;

    public class vmEstatisticasDIA : VMBase, IPrintBox
    {
        #region Declarations
        private NavigationService ns;
        private ObservableCollection<DIA> _listadia = new ObservableCollection<DIA>();
        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();
        private FlowDocument _flwdoc = new FlowDocument();

        private Visibility _mainbox;
        #endregion

        #region Properties
        public NavigationService NS
        {
            get { return ns; }
            set { ns = value; RaisePropertyChanged("NS"); }
        }

        public ObservableCollection<DIA> ListaDIA
        {
            get { return _listadia; }
            set { _listadia = value; RaisePropertyChanged("ListaDIA"); }
        }

        public ObservableCollection<BarChart> Charts
        {
            get { return _charts; }
            set { _charts = value; RaisePropertyChanged("Charts"); }
        }

        public FlowDocument FlowDoc
        {
            get { return _flwdoc; }
            set
            {
                _flwdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public Visibility MainBox
        {
            get { return _mainbox; }
            set { _mainbox = value; RaisePropertyChanged("MainBox"); }
        }
        #endregion

        #region Commands
        public ICommand CommandFiltrar => new RelayCommand(p => { ListarDIA(); });

        public ICommand CommandListar => new RelayCommand(p => { AsyncFlowDoc(); });

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

        public ICommand CommandClosePrintBox => new RelayCommand(p => { NS.GoBack(); });
        #endregion

        #region Factory
        public vmEstatisticasDIA()
        {
            GlobalNavigation.Pagina = "RELATÓRIO";
            NS = GlobalNavigation.NavService;
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            StartProgress = false;

        }
        #endregion

        #region Functions
        private async void ListarDIA()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            var t = Task.Run(() => new Repositorio.RDIA().SetEDIAS(new Repositorio.RDIA().EDIAs()));
            await t;

            if (t.IsCompleted)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate () {

                    Charts = FlowChart(t.Result);

                })).Wait();
                
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
            }
            else
            {
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
            }

        }

        private async void AsyncFlowDoc()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            var t = Task.Run(() => new Repositorio.RDIA().SetEDIAS(new Repositorio.RDIA().EDIAs()));
            await t;

            if (t.IsCompleted)
            {

                await Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                {

                    FlowDoc = PreviwChart(FlowChartPrint(t.Result));

                }));

                BlackBox = Visibility.Collapsed;
                MainBox = Visibility.Collapsed;
                PrintBox = Visibility.Visible;
                StartProgress = false;
            }
        }

        private ObservableCollection<BarChart> FlowChart(MReportDIA _report)
        {
            ObservableCollection<BarChart> _chart = new ObservableCollection<BarChart>();
                       

                MReportDIA _rp = new MReportDIA();

                foreach (KeyValuePair<string, int> x in _report.DIA)
                {
                    _rp.DIA.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in _report.Ativo)
                {
                    _rp.Ativo.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in _report.AtivoND)
                {
                    _rp.AtivoND.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in _report.Vencido)
                {
                    _rp.Vencido.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }

                foreach (KeyValuePair<string, int> x in _report.Baixado)
                {
                    _rp.Baixado.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
                }


                if (_rp.DIA.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("D.I.A EMITIDOS");
                    bc.ItemsSource = _rp.DIA;
                    _chart.Add(bc);
                }

                if (_rp.Ativo.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("D.I.A ATIVOS");
                    bc.ItemsSource = _rp.Ativo;

                    _chart.Add(bc);
                }

                if (_rp.AtivoND.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("D.I.A ATIVO SEM DATA DE RENOVAÇÃO");
                    bc.ItemsSource = _rp.AtivoND;

                    _chart.Add(bc);
                }

                if (_rp.Vencido.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("D.I.A VENCIDOS");
                    bc.ItemsSource = _rp.Vencido;

                    _chart.Add(bc);
                }

                if (_rp.Baixado.Count > 0)
                {

                    BarChart bc = new BarChart();

                    bc.Title = string.Format("D.I.A BAIXADOS");
                    bc.ItemsSource = _rp.Baixado;

                    _chart.Add(bc);
                }


            return _chart;
        }

        private ObservableCollection<BarChartsColor> FlowChartPrint(MReportDIA _report)
        {
            ObservableCollection<BarChartsColor> _chart = new ObservableCollection<BarChartsColor>();


            MReportDIA _rp = new MReportDIA();

            foreach (KeyValuePair<string, int> x in _report.DIA)
            {
                _rp.DIA.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in _report.Ativo)
            {
                _rp.Ativo.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in _report.AtivoND)
            {
                _rp.AtivoND.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in _report.Vencido)
            {
                _rp.Vencido.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }

            foreach (KeyValuePair<string, int> x in _report.Baixado)
            {
                _rp.Baixado.Add(new KeyValuePair<string, int>(x.Key.ToUpper(), x.Value));
            }


            if (_rp.DIA.Count > 0)
            {

                BarChartsColor bc = new BarChartsColor();

                bc.Title = string.Format("D.I.A EMITIDOS");
                bc.ItemsSource = _rp.DIA;
                _chart.Add(bc);
            }

            if (_rp.Ativo.Count > 0)
            {

                BarChartsColor bc = new BarChartsColor();

                bc.Title = string.Format("D.I.A ATIVOS");
                bc.ItemsSource = _rp.Ativo;

                _chart.Add(bc);
            }

            if (_rp.AtivoND.Count > 0)
            {

                BarChartsColor bc = new BarChartsColor();

                bc.Title = string.Format("D.I.A ATIVO SEM DATA DE RENOVAÇÃO");
                bc.ItemsSource = _rp.AtivoND;

                _chart.Add(bc);
            }

            if (_rp.Vencido.Count > 0)
            {

                BarChartsColor bc = new BarChartsColor();

                bc.Title = string.Format("D.I.A VENCIDOS");
                bc.ItemsSource = _rp.Vencido;

                _chart.Add(bc);
            }

            if (_rp.Baixado.Count > 0)
            {

                BarChartsColor bc = new BarChartsColor();

                bc.Title = string.Format("D.I.A BAIXADOS");
                bc.ItemsSource = _rp.Baixado;

                _chart.Add(bc);
            }


            return _chart;
        }

        private FlowDocument PreviwChart(ObservableCollection<BarChartsColor> obj)
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

            Paragraph body = new Paragraph();
            body.Typography.Capitals = FontCapitals.SmallCaps;
            body.FontWeight = FontWeights.Bold;
            body.Foreground = Brushes.Black;
            body.TextAlignment = TextAlignment.Center;
            body.Margin = new Thickness(5);
            body.Inlines.Add(new Run("ESTATISTICAS D.I.A"));

            flow.Blocks.Add(body);

            if (obj != null)
            {

                int alt = 1;

                foreach (BarChartsColor bc in obj)
                {
                    alt++;
                    flow.Blocks.Add(new BlockUIContainer(bc));
                }
            }

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

    }

    #endregion

}
