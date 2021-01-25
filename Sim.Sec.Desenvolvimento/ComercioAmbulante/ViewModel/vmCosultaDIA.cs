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

    using Shared.Model;
    using Model;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Controls.ViewModels;

    public class vmCosultaDIA : VMBase, IPrintBox
    {
        #region Declarations
        NavigationService ns;

        private ObservableCollection<Model.DIA> _listadia = new ObservableCollection<Model.DIA>();
        private FlowDocument flwdoc = new FlowDocument();

        private DateTime _datai;
        private DateTime _dataf;

        private List<string> _filtros = new List<string>();

        private string _autorizacao = string.Empty;
        private string _titular = string.Empty;
        private string _atividade = string.Empty;
        private string _formaatuacao = string.Empty;
        private string _situacao = string.Empty;

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

        public ObservableCollection<Model.DIA> ListarDIA
        {
            get { return _listadia; }
            set
            {
                _listadia = value;
                RaisePropertyChanged("ListarDIA");
            }
        }

        public ObservableCollection<string> Situacoes
        {
            get { return new ObservableCollection<string>() { "", "ATIVO", "BAIXADO", "CANCELADO" }; }
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

        public string Autorizacao
        {
            get { return _autorizacao; }
            set { _autorizacao = value; RaisePropertyChanged("Autorizacao"); }
        }

        public string Titular
        {
            get { return _titular; }
            set
            {
                _titular = value;
                RaisePropertyChanged("Titular");
            }
        }

        public string Atividade
        {
            get { return _atividade; }
            set { _atividade = value; RaisePropertyChanged("Atividade"); }
        }

        public string FormaAtuacao
        {
            get { return _formaatuacao; }
            set { _formaatuacao = value; RaisePropertyChanged("FormaAtuacao"); }
        }

        public string Situacao
        {
            get { return _situacao; }
            set
            {
                _situacao = value;
                RaisePropertyChanged("Situacao");
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
        public ICommand CommandFiltrar => new RelayCommand(p => { 
            
            AsyncListarAtendimento(Parametros());

        });

        public ICommand CommandLimpar => new RelayCommand(p =>
        {
            if (ListarDIA != null)
                ListarDIA.Clear();

            Titular = string.Empty;
            Autorizacao = string.Empty;
            DataI = new DateTime(DateTime.Now.Year, 1, 1);
            DataF = DateTime.Now;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
        });


        public ICommand CommandPrint => new RelayCommand(p=> {

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

        public ICommand CommandPrintProfile => new RelayCommand(p =>
        {

            var t = Task<Model.DIA>.Run(() => new Repositorio.DIA().GetDIA((int)p));
            t.Wait();
            AreaTransferencia.Objeto = t.Result;
            ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/PreviewDIA.xaml", UriKind.RelativeOrAbsolute));

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

        public ICommand CommandAlterar => new RelayCommand(p=> {

            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/DIA_Edit.xaml", UriKind.Relative));
                    AreaTransferencia.Indice = (int)p;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/DIA_Edit.xaml", UriKind.Relative));
                                AreaTransferencia.Indice = (int)p;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }

        });
        #endregion

        #region Constructor
        public vmCosultaDIA()
        {
            GlobalNavigation.Pagina = "CONSULTA D.I.A";
            ns = GlobalNavigation.NavService;
            DataI = new DateTime(2020, 1, 1);
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

            _param.Add(DataI.ToShortDateString());
            _param.Add(DataF.ToShortDateString());

            if (Autorizacao == string.Empty || Autorizacao == null)
            {
                _param.Add("%");
                Filtros.Add("[D.I.A = TODOS]");
            }
            else
            {
                _param.Add(Autorizacao);
                Filtros.Add("[D.I.A = " + Autorizacao + "]");
            }

            if (Titular == string.Empty || Titular == null)
            {
                _param.Add("%");
                Filtros.Add("[TITULAR = TODOS]");
            }
            else
            {
                _param.Add(Titular);
                Filtros.Add("[TITULAR = ]" + Titular + "]");
            }

            if (Atividade == string.Empty || Atividade == null)
            {
                _param.Add("%");
                Filtros.Add("[ATIVIDADE = TODOS]");
            }
            else
            {
                _param.Add(Atividade);
                Filtros.Add("[ATIVIDADE = " + Atividade + "]");
            }

            if (FormaAtuacao == string.Empty || FormaAtuacao == null)
            {
                _param.Add("%");
                Filtros.Add("[ATUAÇÃO = TODOS]");
            }
            else
            {
                _param.Add(FormaAtuacao);
                Filtros.Add("[ATUAÇÃO = " + FormaAtuacao + "]");
            }

            if (Situacao == string.Empty || Situacao == null)
            {
                _param.Add("%");
                Filtros.Add("[SITUÇÃO = TODOS]");
            }
            else
            {
                _param.Add(Situacao);
                Filtros.Add("[SITUÇÃO = " + Situacao + "]");
            }

            return _param;
        }

        private async void AsyncListarAtendimento(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            var t = Task.Factory.StartNew(() => new Repositorio.DIA().Consultar(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarDIA = task.Result;
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

            var T = Task.Run(() => PreviewInTable(ListarDIA)).ContinueWith(task => {


                FlowDoc.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    FlowDoc = PreviewInTable(ListarDIA);
                }));

                if (task.IsCompleted)
                {
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                }

            });

            await T;

        }

        private FlowDocument PreviewInTable(ObservableCollection<DIA> obj)
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
            //tb.Columns.Add(new TableColumn() { Width = new GridLength(200) });

            //tb.Columns.Add(new TableColumn() { Width = new GridLength(10, GridUnitType.Star) }); 

            TableRowGroup rg = new TableRowGroup();

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro;

            TableRow pheader = new TableRow();
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("FILTROS: {0}", f))) { Padding = new Thickness(5) }) { ColumnSpan = 4, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("REGISTROS: {0}", obj.Count))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            tb.RowGroups.Add(rg);

            Paragraph p = new Paragraph();
            p.Padding = new Thickness(5);

            TableRow header = new TableRow();
            rg.Rows.Add(pheader);
            rg.Rows.Add(header);
            rg.Foreground = Brushes.Black;
            //rw2.Background = bgc1;
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(""))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("D.I.A"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("AMBULANTE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("SITUAÇÃO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("I.M."))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(1, 0, 0, 0), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (Model.DIA a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Autorizacao + "\nDE: " + a.Emissao.ToShortDateString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    Paragraph np = new Paragraph();
                    np.Padding = new Thickness(5);
                    np.Inlines.Add(new Run(string.Format("TITULAR: {0}", a.Titular.Nome)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("AUXILIAR: {0}", a.Auxiliar.Nome)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("ATIVIDADE: {0}", a.Atividade)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("VALIDADE: {0}, PROCESSO: {1}", data_string( a.Validade, a.Emissao), a.Processo)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("LOCAL: {0}", a.FormaAtuacao)));

                    row.Cells.Add(new TableCell(np) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Situacao)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.InscricaoMunicipal.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

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

        private string data_string(DateTime? dateV, DateTime dateE)
        {
            if (dateV == new DateTime(2001, 1, 1))
                return "-";
            else
            {
                DateTime d = Convert.ToDateTime(dateV);

                var dif = d.Date - dateE.Date;

                //dif.TotalDays;
                var mes = dif.TotalDays / 30;

                if (mes < 1)
                {
                    var dia = mes * 30;
                    if (dia > 1)
                        return Convert.ToInt32(dia).ToString() + " DIAS"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
                    else
                        return Convert.ToInt32(dia).ToString() + " DIA"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
                }
                else if (mes < 2)
                    return Convert.ToInt32(mes).ToString() + " MÊS"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
                else
                    return Convert.ToInt32(mes).ToString() + " MESES"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
            }
        }

        #endregion
    }
}
