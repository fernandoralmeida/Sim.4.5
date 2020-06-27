using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Agenda
{
    using Model;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Account;
    using Controls.ViewModels;

    class vmConsulta : VMBase
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private ObservableCollection<mAgenda> _listaat = new ObservableCollection<mAgenda>();
        private FlowDocument flwdoc = new FlowDocument();

        private DateTime _datai;
        private DateTime _dataf;

        private List<string> _filtros = new List<string>();

        private string _getcpf;
        private int _gettipos;
        private int _getestados;
        private int _getsetor;

        private Visibility _mainbox;
        
        private ICommand _commandfiltrar;
        private ICommand _commandlimpar;
        private ICommand _commandprint;
        private ICommand _commandcloseprintbox;
        private ICommand _commandalterar;
        private ICommand _commandlistar;
        private ICommand _commandshowinscritos;
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

        public ObservableCollection<mAgenda> ListarAgenda
        {
            get { return _listaat; }
            set
            {
                _listaat = value;
                RaisePropertyChanged("ListarAgenda");
            }
        }

        public ObservableCollection<mTiposGenericos> Estados
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> Setores
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Valor"); }
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

        public int GetTipos
        {
            get { return _gettipos; }
            set
            {
                _gettipos = value;
                RaisePropertyChanged("GetTipos");
            }
        }

        public int GetEstados
        {
            get { return _getestados; }
            set
            {
                _getestados = value;
                RaisePropertyChanged("GetEstados");
            }
        }

        public int GetSetor
        {
            get { return _getsetor; }
            set
            {
                _getsetor = value;
                RaisePropertyChanged("GetSetor");
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
                AsyncListarAgenda(Parametros());
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
            if (ListarAgenda != null)
                ListarAgenda.Clear();

            GetCNPJCPF = string.Empty;
            DataI = new DateTime(2016, 1, 1);
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
                            pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

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

        public ICommand CommandAlterar
        {
            get
            {
                if (_commandalterar == null)
                    _commandalterar = new DelegateCommand(ExecCommandAlterar, null);
                return _commandalterar;
            }
        }

        private void ExecCommandAlterar(object obj)
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pEditar.xaml", UriKind.Relative));
                    AreaTransferencia.Parametro = (string)obj;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pEditar.xaml", UriKind.Relative));
                                AreaTransferencia.Parametro = (string)obj;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }
        }

        public ICommand CommandShowInscritos
        {
            get
            {
                if (_commandshowinscritos == null)
                    _commandshowinscritos = new RelayCommand(p => {

                        AreaTransferencia.Evento = p.ToString();
                        ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pDetalhe.xaml", UriKind.RelativeOrAbsolute));

                    });

                return _commandshowinscritos;
            }
        }
        #endregion

        #region Constructor
        public vmConsulta()
        {
            GlobalNavigation.Pagina = "CONSULTA EVENTOS";
            ns = GlobalNavigation.NavService;
            DataI = new DateTime(2016, 1, 1);
            DataF = new DateTime(DateTime.Now.Year, 12, 31);
            MainBox = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            PrintBox = Visibility.Collapsed;
            StartProgress = false;
            GetTipos = 0;
            GetEstados = 0;
        }
        #endregion

        #region Functions
        private List<string> Parametros()
        {

            List<string> _param = new List<string>() { };

            Filtros.Clear();

            _param.Add(DataI.ToShortDateString());
            _param.Add(DataF.ToShortDateString());


            if (GetEstados < 1)
            {
                _param.Add("0");
                _param.Add("99");
                Filtros.Add("[ESTADOs = TODOS]");
            }
            else
            {
                _param.Add(GetEstados.ToString());
                _param.Add(GetEstados.ToString());
                Filtros.Add("[ESTADOs = " + Estados[GetEstados].Nome + "]");
            }

            if (GetTipos < 1)
            {
                _param.Add("0");
                _param.Add("99");
                Filtros.Add("[TIPOs = TODOS]");
            }
            else
            {
                _param.Add(GetTipos.ToString());
                _param.Add(GetTipos.ToString());
                Filtros.Add("[TIPOs = " + GetTipos + "]");
            }

            if (GetSetor < 1)
            {
                _param.Add("0");
                _param.Add("99");
                Filtros.Add("[SETOR = TODOS]");
            }
            else
            {
                _param.Add(GetSetor.ToString());
                _param.Add(GetSetor.ToString());
                Filtros.Add("[SETOR = " + GetTipos + "]");
            }

            if (GetCNPJCPF == string.Empty || GetCNPJCPF == null)
            {
                _param.Add("%");
                Filtros.Add("[CLIENTE = TODOS]");
            }
            else
            {
                _param.Add(new mMascaras().Remove(GetCNPJCPF));
                Filtros.Add("[CLIENTE = " + GetCNPJCPF + "]");
            }

            return _param;
        }

        private void AsyncListarAgenda(List<string> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;

            Task.Factory.StartNew(() => mdata.AgendaInscritos(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarAgenda = task.Result;
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

            new System.Threading.Thread(() =>
            {
                System.Windows.Threading.DispatcherOperation op =
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate () {
                        FlowDoc = PreviewInTable(ListarAgenda);
                    }));

                op.Completed += (o, args) =>
                {
                    BlackBox = Visibility.Collapsed;
                    MainBox = Visibility.Collapsed;
                    PrintBox = Visibility.Visible;
                    StartProgress = false;
                };
            }).Start();
        }

        private FlowDocument PreviewPrint(ObservableCollection<mAgenda> obj)
        {
            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            flow.ColumnWidth = 8.5 * 96.0;
            flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(40, 40, 40, 60);

            Paragraph pH = new Paragraph(new Run(new mFlowHeader().NameOrg));
            pH.Typography.Capitals = FontCapitals.SmallCaps;
            pH.FontSize = 20;
            pH.FontWeight = FontWeights.Bold;
            pH.Margin = new Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(new mFlowHeader().SloganOrg));
            pH1.FontSize = 9;
            pH1.Margin = new Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(new mFlowHeader().DepOrg));
            pH2.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH2.FontSize = 14;
            pH2.Margin = new Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(new mFlowHeader().SetorOrg));
            pH3.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.FontWeight = FontWeights.Bold;
            pH3.FontSize = 12;
            pH3.Margin = new Thickness(0, 0, 0, 40);

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

            foreach (mAgenda a in obj)
            {

                Paragraph pr = new Paragraph();
                pr.Inlines.Add(new Run("ATENDIMENTO: " + a.Codigo) { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("{0}, {1}, {2} ", a.Evento, a.Data.ToShortDateString(), a.Hora.ToShortTimeString()));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CLIENTE:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(string.Format("{0}", a.Tipo));
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("ORIGEM:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.Estado.ToString());
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("TIPO:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(a.TipoString);

                flow.Blocks.Add(pr);
            }


            Figure lrodape = new Figure();
            lrodape.Width = new FigureLength(1000);
            lrodape.Height = new FigureLength(1);
            lrodape.Background = Brushes.Gray;
            lrodape.Margin = new Thickness(0, 10, 0, 0);

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

        private FlowDocument PreviewInTable(ObservableCollection<mAgenda> obj)
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
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("TIPO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("EVENTO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ESTADO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("SETOR"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(1, 0, 0, 0), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (mAgenda a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.TipoString)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Evento + "\n\n" + a.Descricao + "\n" + a.Data.ToShortDateString() + " " + a.Hora.ToShortTimeString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.EstadoString + "\n\n" + "INSCRITOS: " + a.Inscritos)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.SetorString)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    //row.Cells.Add(new TableCell(new Paragraph(new Run("")) { Padding = new Thickness(5, 20, 5, 20) }) { BorderThickness = new Thickness(1, 1, 0, 0), BorderBrush = Brushes.Black });
                    /*
                    if (alt % 2 != 0)
                        row.Background = bgc1;
                        */
                    alt++;
                }
            }

            flow.Blocks.Add(tb);
            /*
            var lrodape = new Rectangle();
            lrodape.Stroke = new SolidColorBrush(Colors.Silver);
            lrodape.StrokeThickness = 1;
            lrodape.Height = 1;
            lrodape.Width = double.NaN;
            lrodape.Margin = new Thickness(0, 10, 0, 0);

            var lineBlock = new BlockUIContainer(lrodape);
            flow.Blocks.Add(lineBlock);
            */

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            //r.Inlines.Add(lrodape);
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
