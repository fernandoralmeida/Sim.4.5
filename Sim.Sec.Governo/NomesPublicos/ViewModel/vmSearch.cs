using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sim.Sec.Governo.NomesPublicos.ViewModel
{

    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;

    class vmSearch : NotifyProperty
    {
        #region Declarations
        private mData mdata = new mData();
        private DataTable _den = new DataTable();
        private FlowDocument flwdoc = new FlowDocument();

        private List<string> _filtros = new List<string>();

        private string _tipo;
        private string _por_nome_ant = string.Empty;
        private string _por_bairro = string.Empty;
        private string _por_nome_atl = string.Empty;

        private int _especie;
        private int _ano1;
        private int _ano2;

        private bool _bairro;
        private bool _cep;
        private bool _inicio;
        private bool _nomeant;
        private bool _origem;
        private bool _obs;

        private bool _starprogress;

        private Visibility _blackbox;
        private Visibility _printbox;
        private Visibility _gridbox;

        private ICommand _commandfiltrar;
        private ICommand _commandlimpar;
        private ICommand _commandprint;
        private ICommand _commandstartprint;
        private ICommand _commandcloseprintbox;
        private ICommand _commandlistar;
        //private ICommand _commandexport;
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

        public DataTable Denominacoes
        {
            get { return _den; }
            set { _den = value;
                RaisePropertyChanged("Denominacoes");
            }
        }

        public ObservableCollection<string> Tipos
        {
            get
            {
                var m = new mData().Tipos();
                m.Add("TODOS");
                return m;
            }
        }

        public ObservableCollection<mEspecie> Especies
        {
            get
            {
                var m = new mData().Especies();
                m.Add(new mEspecie() { Indice = -1, Ativo = true, Codigo = -1, Especie = "TODOS" });
                return m;
            }
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

        public string Tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }

        public string byNomeAtual
        {
            get { return _por_nome_atl; }
            set
            {
                _por_nome_atl = value;
                RaisePropertyChanged("byNomeAtual");
            }
        }

        public string byNomeAnterior
        {
            get { return _por_nome_ant; }
            set
            {
                _por_nome_ant = value;
                RaisePropertyChanged("byNomeAnterior");
            }
        }

        public string byBairro
        {
            get { return _por_bairro; }
            set
            {
                _por_bairro = value;
                RaisePropertyChanged("byBairro");
            }
        }

        public int Especie
        {
            get { return _especie; }
            set
            {
                _especie = value;
                RaisePropertyChanged("Especie");
            }
        }

        public int Ano1
        {
            get { return _ano1; }
            set
            {
                _ano1 = value;
                RaisePropertyChanged("Ano1");
            }
        }

        public int Ano2
        {
            get { return _ano2; }
            set
            {
                _ano2 = value;
                RaisePropertyChanged("Ano2");
            }
        }

        public bool Bairro
        {
            get { return _bairro; }
            set { _bairro = value;
                RaisePropertyChanged("Bairro");
            }
        }

        public bool CEP
        {
            get { return _cep; }
            set
            {
                _cep = value;
                RaisePropertyChanged("CEP");
            }
        }

        public bool Inicio
        {
            get { return _inicio; }
            set
            {
                _inicio = value;
                RaisePropertyChanged("Inicio");
            }
        }

        public bool NomeAnt
        {
            get { return _nomeant; }
            set
            {
                _nomeant = value;
                RaisePropertyChanged("NomeAnt");
            }
        }

        public bool Origem
        {
            get { return _origem; }
            set
            {
                _origem = value;
                RaisePropertyChanged("Origem");
            }
        }

        public bool Observacoes
        {
            get { return _obs; }
            set
            {
                _obs = value;
                RaisePropertyChanged("Observacoes");
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

        public Visibility GridBox
        {
            get { return _gridbox; }
            set
            {
                _gridbox = value;
                RaisePropertyChanged("GridBox");
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
                AsyncListarDenominacoes(Parametros());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandListar
        {
            get
            {
                if (_commandlistar == null)
                    _commandlistar = new RelayCommand(p => {

                        if (Denominacoes == null || Denominacoes.Rows.Count > 0)
                            AsyncFlowDoc(Parametros());

                    });

                return _commandlistar;
            }
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
            Ano1 = 0;
            Ano2 = 0;
            Tipo = "TODOS";
            Especie = -1;
            byNomeAnterior = string.Empty;
            byBairro = string.Empty;
            Denominacoes.Clear();
            FlowDoc = null;
            PrintBox = Visibility.Collapsed;
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
            try
            {
                if (Denominacoes == null || Denominacoes.Rows.Count > 0)
                    ExecCommandStartPrint(null);
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Sim.Alerta!");
            }

        }

        public ICommand CommandStartPrint
        {
            get
            {
                if (_commandstartprint == null)
                    _commandstartprint = new DelegateCommand(ExecCommandStartPrint, null);
                return _commandstartprint;
            }
        }

        private void ExecCommandStartPrint(object obj)
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            //Now print using PrintDialog
            var pd = new PrintDialog();

            if (pd.ShowDialog().Value)
            {
                pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
                FlowDoc.PageHeight = 768;
                FlowDoc.PageWidth = 1104;
                FlowDoc.ColumnWidth = 1104;
                IDocumentPaginatorSource idocument = FlowDoc as IDocumentPaginatorSource;
                idocument.DocumentPaginator.ComputePageCountAsync();
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
            GridBox = Visibility.Visible;
        }
        #endregion

        #region Constructor
        public vmSearch()
        {
            Tipo = "TODOS";
            Especie = -1;
            BlackBox = Visibility.Collapsed;
            GridBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            Mvvm.Observers.GlobalNavigation.Pagina = "CONSULTA";
        }
        #endregion

        #region Functions
        private string Parametros()
        {
            List<string> sql_select = new List<string>();

            Filtros.Clear();

            if (Bairro)
                sql_select.Add("Bairro");

            if (CEP)
                sql_select.Add("Cep");

            if (Inicio)
                sql_select.Add("Inicio");

            if (NomeAnt)
                sql_select.Add("Nome_Anterior");

            if (Origem)
            {
                sql_select.Add("Origem");
                sql_select.Add("Numero_Origem");
                sql_select.Add("Ano_Origem");
            }

            if (Observacoes)
                sql_select.Add("Observacoes");

            StringBuilder selects = new StringBuilder();

            foreach(string s in sql_select)
                selects.Append(", " + s);

            string _anoi = "0";
            string _anoii = DateTime.Now.Year.ToString();
            string _s_especie = "%";
            string _s_tipo = "%";
            string _s_nomeant = "%";
            string _s_bairro = "%";
            string _s_nomeatu = "%";

            if (Ano1 > 0)
                _anoi = Ano1.ToString();

            if (Ano2 > 0)
                _anoii = Ano2.ToString();

            if (Especie > -1)
            {
                _s_especie = Especie.ToString();
                Filtros.Add(Especies[Especie].Especie);
            }

            if (Tipo != "TODOS")
            {
                _s_tipo = Tipo;
                Filtros.Add(_s_tipo);
            }

            if (byNomeAtual != string.Empty)
            {
                _s_nomeatu = byNomeAtual; ;
                Filtros.Add(_s_nomeatu);
            }

            if (byNomeAnterior != string.Empty)
            {
                _s_nomeant = byNomeAnterior;
                Filtros.Add(_s_nomeant);
            }

            if (byBairro != string.Empty)
            {
                _s_bairro = byBairro;
                Filtros.Add(_s_bairro);
            }

            return string.Format("SELECT Tipo, Nome{0} FROM Denominacoes WHERE (Ano_Origem BETWEEN '{1}' AND '{2}') AND (Especie LIKE '{3}') AND (Tipo LIKE '{4}') AND (Nome_Anterior LIKE '%' + '{5}' + '%') AND (Bairro LIKE '%' + '{6}' + '%') AND (Nome LIKE '%' + '{7}' + '%') ORDER BY Nome",
                selects.ToString(),
                _anoi,
                _anoii,
                _s_especie,
                _s_tipo,
                _s_nomeant,
                _s_bairro,
                _s_nomeatu);
        }


        private void AsyncListarDenominacoes(string sqlcommand)
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;

            Task.Factory.StartNew(() => mdata.Denominacoes(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Denominacoes = task.Result;
                        BlackBox = Visibility.Collapsed;
                        GridBox = Visibility.Visible;
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

        private void AsyncFlowDoc(string sqlcommand)
        {
            try
            {

                BlackBox = Visibility.Visible;
                StartProgress = true;

                Task.Factory.StartNew(() => mdata.Denominacoes(sqlcommand))
                    .ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            Denominacoes = task.Result;
                            Application.Current.Dispatcher.BeginInvoke(
                                System.Windows.Threading.DispatcherPriority.Background,
                                new Action(() =>
                                {
                                    FlowDoc = PreviewInTable(Denominacoes);
                                    BlackBox = Visibility.Collapsed;
                                    GridBox = Visibility.Collapsed;
                                    PrintBox = Visibility.Visible;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        private FlowDocument PreviewInTable(DataTable obj)
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

            string f = string.Empty;
            foreach (string filtro in Filtros)
                f += filtro + " ";

            Paragraph ft = new Paragraph();
            ft.Foreground = Brushes.Black;
            ft.Margin = new Thickness(0, 0, 0, 0);
            ft.FontSize = 10;
            ft.Inlines.Add(new Run("FILTROS: "));
            ft.Inlines.Add(new Run(f));

            Paragraph ft1 = new Paragraph();
            ft1.Foreground = Brushes.Black;
            ft1.Margin = new Thickness(0, 0, 0, 0);
            ft1.FontSize = 10;
            ft1.Inlines.Add(new Run("[REG. ENCONTRADO(S): "));
            ft1.Inlines.Add(new Bold(new Run(obj.Rows.Count.ToString())));
            ft1.Inlines.Add(new Run("]"));

            flow.Blocks.Add(ft);
            flow.Blocks.Add(ft1);

            /*
            var lfiltro = new Rectangle();
            lfiltro.Stroke = new SolidColorBrush(Colors.Silver);
            lfiltro.StrokeThickness = 1;
            lfiltro.Height = 1;
            lfiltro.Width = double.NaN;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            var linefiltro = new BlockUIContainer(lfiltro);
            flow.Blocks.Add(linefiltro);            */


            Table tb = new Table();
            tb.CellSpacing = 0;
            tb.BorderThickness = new Thickness(0.5);
            tb.BorderBrush = Brushes.Black;

            tb.Columns.Add(new TableColumn() { Width = new GridLength(50) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            //tb.Columns.Add(new TableColumn() { Width = new GridLength(10, GridUnitType.Star) });

            if (Bairro)
                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });

            if (CEP)
                tb.Columns.Add(new TableColumn() { Width = new GridLength(80) });

            if (Origem)
                tb.Columns.Add(new TableColumn() { Width = new GridLength(150) });

            if (Inicio)
                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });

            if (NomeAnt)
                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });

            if (Observacoes)
                tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });


            TableRowGroup rg2 = new TableRowGroup();
            rg2.Foreground = Brushes.Black;

            tb.RowGroups.Add(rg2);

            TableRow rw2 = new TableRow();
            rg2.Rows.Add(rw2);
            rw2.Foreground = Brushes.Black;

            rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(""))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));
            rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("TIPO"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));
            rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("NOME"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            if (Bairro)
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("BAIRRO"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            if (CEP)
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CEP"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            if (Origem)
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ORIGEM"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            if (Inicio)
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("INICIO"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            if (NomeAnt)
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("N.ANTERIOR"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            if (Observacoes)
                rw2.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("OBS"))) { Padding = new Thickness(5), BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black }));

            int alt = 0;

            if (obj != null)
            {

                foreach (DataRow a in obj.Rows)
                {

                    TableRow row = new TableRow();
                    rg2.Rows.Add(row);
                    alt++;
                    row.Cells.Add(new TableCell(new Paragraph(new Run(alt.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a["Tipo"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a["Nome"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    /*
                    if (alt % 2 != 0)
                        row.Background = bgc1;
                        */
                    if (Bairro)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(a["Bairro"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (CEP)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(a["Cep"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (Origem)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0} {1}/{2}", a["Origem"], a["Numero_Origem"], a["Ano_Origem"]))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (Inicio)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(a["Inicio"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (NomeAnt)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(a["Nome_Anterior"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    if (Observacoes)
                        row.Cells.Add(new TableCell(new Paragraph(new Run(a["Observacoes"].ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    
                }
            }

            flow.Blocks.Add(tb);

            /*
            Paragraph t = new Paragraph();
            t.Margin = new Thickness(0, 0, 0, 0);
            t.Inlines.Add(new Run(string.Format("{0} Resgistro(os) encontrado(os)", obj.Rows.Count)));
            flow.Blocks.Add(t);
            */

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.Foreground = Brushes.Black;
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
        
        #endregion
    }
}
