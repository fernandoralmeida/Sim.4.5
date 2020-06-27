using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Agenda
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;

    class vmDetalhe : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        mAgenda _agd = new mAgenda();
        ObservableCollection<mInscricao> _inscritos = new ObservableCollection<mInscricao>();
        FlowDocument _flow = new FlowDocument();
        private mData mdata = new mData();

        private bool _starprogress;
        private bool _isnane;
        private bool _isinscricao;

        private string _textocanceladoouativo;

        private int _vagasrestantes;

        private Visibility _blackbox;
        private Visibility _viewflow;
        private Visibility _viewgrid;

        private ICommand _commandprint;
        private ICommand _commandflow;
        private ICommand _commandatualizarlista;
        private ICommand _commandgrid;
        private ICommand _commandfinalizar;
        private ICommand _commandcancelarinscricao;
        private ICommand _commandcancelados;
        private ICommand _commandispresente;
        private ICommand _commandclosepreview;
        private ICommand _commandaddinscrito;
        #endregion

        #region Properties
        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public ObservableCollection<mTiposGenericos> Estados
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mInscricao> ListaInscritos
        {
            get { return _inscritos; }
            set
            {
                _inscritos = value;
                RaisePropertyChanged("ListaInscritos");
            }
        }

        public FlowDocument FlowDoc
        {
            get { return _flow; }
            set
            {
                _flow = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public mAgenda Agenda
        {
            get { return _agd; }
            set
            {
                _agd = value;
                RaisePropertyChanged("Agenda");
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

        public Visibility ViewFlow
        {
            get { return _viewflow; }
            set
            {
                _viewflow = value;
                RaisePropertyChanged("ViewFlow");
            }
        }

        public Visibility ViewGrid
        {
            get { return _viewgrid; }
            set
            {
                _viewgrid = value;
                RaisePropertyChanged("ViewGrid");
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

        public bool IsName
        {
            get { return _isnane; }
            set
            {
                _isnane = value;
                RaisePropertyChanged("IsName");
            }
        }

        public bool IsInscricao
        {
            get { return _isinscricao; }
            set
            {
                _isinscricao = value;
                RaisePropertyChanged("IsInscricao");
            }
        }

        public string TextoCanceladoOuAtivo
        {
            get { return _textocanceladoouativo; }
            set
            {
                _textocanceladoouativo = value;
                RaisePropertyChanged("TextoCanceladoOuAtivo");
            }
        }

        public int VagasRestantes
        {
            get { return _vagasrestantes; }
            set
            {
                _vagasrestantes = value;
                RaisePropertyChanged("VagasRestantes");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandGrid
        {
            get
            {
                if (_commandgrid == null)
                    _commandgrid = new RelayCommand(p => {

                        ViewGrid = Visibility.Visible;
                        ViewFlow = Visibility.Collapsed;
                        AsyncListarInscritosI(Agenda.Codigo);

                    });

                return _commandgrid;
            }
        }

        public ICommand CommandFlow
        {
            get
            {
                if (_commandflow == null)
                    _commandflow = new RelayCommand(p => {

                        AsyncFlowDoc(Agenda.Codigo);
                    });

                return _commandflow;
            }
        }

        public ICommand CommandAtualizarLista
        {
            get
            {
                if (_commandatualizarlista == null)
                    _commandatualizarlista = new RelayCommand(p => {

                        AsyncFlowDoc(Agenda.Codigo);
                    });

                return _commandatualizarlista;
            }
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

                            //Now print using PrintDialog
                            var pd = new System.Windows.Controls.PrintDialog();
                            pd.UserPageRangeEnabled = true;
                            pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                            if (pd.ShowDialog().Value)
                            {
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
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    });

                return _commandprint;
            }
        }

        public ICommand CommandFinalizar
        {
            get
            {
                if (_commandfinalizar == null)
                    _commandfinalizar = new RelayCommand(p =>
                    {

                        AsyncFinalizar(Agenda.Codigo, (int)p);

                    });
                return _commandfinalizar;
            }
        }

        public ICommand CommandIsPresente
        {
            get
            {
                if (_commandispresente == null)
                    _commandispresente = new RelayCommand(p =>
                    {
                        var objetos = (object[])p;
                        var bloqueado = (CheckBox)objetos[1];

                        AsyncIsPresente((string)objetos[0], (bool)bloqueado.IsChecked);
                    }, null);
                return _commandispresente;
            }
        }

        public ICommand CommandCancelarInscricao
        {
            get
            {
                if (_commandcancelarinscricao == null)
                    _commandcancelarinscricao = new RelayCommand(p =>
                    {
                        var objetos = (object[])p;
                        var ativar = (bool)objetos[1];

                        if (ativar == true)
                        {
                            ativar = false;
                            if (MessageBox.Show("Deseja Cancelar esta Inscrição?", "Sim.Alerta!", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                                AsyncGerenciarInscricao((string)objetos[0], ativar);
                        }
                        else
                        {
                            int r = 0;

                            r = mdata.VagasEvento(Agenda.Codigo) - mdata.InscritosEventos(Agenda.Codigo);

                            if (r > 0)
                            {
                                ativar = true;
                                if (MessageBox.Show("Deseja Ativar esta Inscrição?", "Sim.Alerta!", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                                    AsyncGerenciarInscricao((string)objetos[0], ativar);
                            }
                            else
                                MessageBox.Show("Evento com Vagas Esgotadas!", "Sim.Alerta!");
                        }


                    }, null);
                return _commandcancelarinscricao;
            }
        }

        public ICommand CommandCancelados
        {
            get
            {
                if (_commandcancelados == null)
                    _commandcancelados = new RelayCommand(p =>
                    {
                        ViewGrid = Visibility.Visible;
                        ViewFlow = Visibility.Collapsed;
                        AsyncListarInscritosCancelados(Agenda.Codigo);

                    }, null);
                return _commandcancelados;
            }
        }

        private void ExecuteCommandIsPresente(object obj)
        {
            var objetos = (object[])obj;
            var bloqueado = (CheckBox)objetos[1];

            AsyncIsPresente((string)objetos[0], (bool)bloqueado.IsChecked);
        }

        public ICommand CommandClosePreview
        {
            get
            {
                if (_commandclosepreview == null)
                    _commandclosepreview = new RelayCommand(p => {

                        ViewGrid = Visibility.Visible;
                        ViewFlow = Visibility.Collapsed;
                        FlowDoc = null;
                        FlowDoc = new FlowDocument();
                    });

                return _commandclosepreview;
            }
        }

        public ICommand CommandAddInscricao
        {
            get
            {
                if (_commandaddinscrito == null)
                    _commandaddinscrito = new RelayCommand(p =>
                    {
                        AreaTransferencia.InscricaoOK = false;
                        AreaTransferencia.CPF = string.Empty;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pInscricao.xaml", UriKind.Relative));
                    });

                return _commandaddinscrito;
            }
        }
        #endregion

        #region Construtor
        public vmDetalhe()
        {
            ns = GlobalNavigation.NavService;

            ViewGrid = Visibility.Visible;
            ViewFlow = Visibility.Collapsed;

            IsName = true;

            Task.Factory.StartNew(() => {
                if (AreaTransferencia.Evento != string.Empty)
                    AsyncSelectedEvento(AreaTransferencia.Evento);
            });

        }
        #endregion

        #region Functions
        private void AsyncSelectedEvento(string codigo)
        {
            Task.Factory.StartNew(() => mdata.Evento(codigo))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Agenda.Clear();
                        Agenda.Indice = task.Result.Indice;
                        Agenda.Codigo = task.Result.Codigo;
                        Agenda.Evento = task.Result.Evento;
                        Agenda.Tipo = task.Result.Tipo;
                        Agenda.TipoString = task.Result.TipoString;
                        Agenda.Descricao = task.Result.Descricao;
                        Agenda.Vagas = task.Result.Vagas;
                        Agenda.Data = task.Result.Data;
                        Agenda.Hora = task.Result.Hora;
                        Agenda.Setor = task.Result.Setor;
                        Agenda.Estado = task.Result.Estado;
                        Agenda.Criacao = task.Result.Criacao;
                        Agenda.Ativo = task.Result.Ativo;

                        if (Agenda.Codigo != string.Empty)
                            AsyncListarInscritosI(Agenda.Codigo);
                    }
                });
        }

        private void AsyncListarInscritos(string codigo)
        {
            TextoCanceladoOuAtivo = "CANCELAR";
            Task.Factory.StartNew(() => mdata.Inscritos(codigo))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListaInscritos = task.Result;
                        VagasRestantes = Agenda.Vagas - ListaInscritos.Count;
                    }
                });
        }

        private void AsyncListarInscritosI(string codigo)
        {
            TextoCanceladoOuAtivo = "CANCELAR";
            Task.Factory.StartNew(() => mdata.InscritosI(codigo))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {

                        ListaInscritos = task.Result;
                        VagasRestantes = Agenda.Vagas - ListaInscritos.Count;
                    }
                });
        }

        private void AsyncListarInscritosCancelados(string codigo)
        {
            TextoCanceladoOuAtivo = "ATIVAR";
            Task.Factory.StartNew(() => mdata.InscritosCancelados(codigo))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListaInscritos = task.Result;
                    }
                });
        }

        private void AsyncFlowDoc(string evento)
        {
            try
            {
                BlackBox = Visibility.Visible;
                StartProgress = true;
                new System.Threading.Thread(() =>
                {

                    if (IsName)
                        ListaInscritos = mdata.Inscritos(evento);

                    if (IsInscricao)
                        ListaInscritos = mdata.InscritosI(evento);

                    System.Windows.Threading.DispatcherOperation op =
                    FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                        new Action(delegate () {

                            FlowDoc = PreviewInTable(ListaInscritos);
                            ViewGrid = Visibility.Collapsed;
                            ViewFlow = Visibility.Visible;

                        }));

                    op.Completed += (o, args) =>
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    };
                }).Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        private void AsyncFinalizar(string codigo, int valor)
        {
            Task.Factory.StartNew(() => mdata.AtualizarEvento(codigo, valor))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        MessageBox.Show("Estado atualizado com sucesso!", "Sim.Alerta!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
        }

        private void AsyncIsPresente(string codigo, bool valor)
        {

            Task.Factory.StartNew(() => mdata.InscricaoPresente(codigo, valor))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {

                        if (Agenda.Codigo != string.Empty)
                            AsyncListarInscritos(Agenda.Codigo);

                    }
                });
        }

        private void AsyncGerenciarInscricao(string codigo, bool valor)
        {

            Task.Factory.StartNew(() => mdata.CancelarInscricao(codigo, valor))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {

                        if (Agenda.Codigo != string.Empty)
                        {
                            if (TextoCanceladoOuAtivo == "ATIVAR")
                                AsyncListarInscritosCancelados(Agenda.Codigo);
                            else
                                AsyncListarInscritos(Agenda.Codigo);
                        }

                    }
                });
        }

        private FlowDocument PreviewInTable(ObservableCollection<mInscricao> obj)
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

            /*
            var lfiltro = new Rectangle();
            lfiltro.Stroke = new SolidColorBrush(Colors.Silver);
            lfiltro.StrokeThickness = 1;
            lfiltro.Height = 1;
            lfiltro.Width = double.NaN;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            var linefiltro = new BlockUIContainer(lfiltro);
            flow.Blocks.Add(linefiltro);            */


            //SolidColorBrush bgc1 = Application.Current.Resources["ButtonBackgroundHover"] as SolidColorBrush;

            Table tb = new Table();
            tb.CellSpacing = 0;
            tb.BorderThickness = new Thickness(0.5);
            tb.BorderBrush = Brushes.Black;

            tb.Columns.Add(new TableColumn() { Width = new GridLength(50) });
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(200) });

            //tb.Columns.Add(new TableColumn() { Width = new GridLength(10, GridUnitType.Star) }); 

            TableRowGroup rg = new TableRowGroup();

            TableRow pheader = new TableRow();
            pheader.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("EVENTO: {0} {1}", Agenda.TipoString, Agenda.Evento)))) { Padding = new Thickness(5) }) { ColumnSpan = 5, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            pheader.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(string.Format("DATA: {0}", Agenda.Data.ToShortDateString())))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            tb.RowGroups.Add(rg);

            Paragraph p = new Paragraph();
            p.Padding = new Thickness(5);

            TableRow header = new TableRow();
            rg.Rows.Add(pheader);
            rg.Rows.Add(header);
            rg.Foreground = Brushes.Black;
            //rw2.Background = bgc1;
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(""))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("NOME"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CPF"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CNPJ"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("TELEFONE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (mInscricao a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5, 8, 5, 8) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Nome.ToString())) { Padding = new Thickness(5, 8, 5, 8) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(new mMascaras().CPF(a.CPF.ToString()))) { Padding = new Thickness(5, 8, 5, 8) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(new mMascaras().CNPJ(a.CNPJ.ToString()))) { Padding = new Thickness(5, 8, 5, 8) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Telefone.ToString())) { Padding = new Thickness(5, 8, 5, 8) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run("")) { Padding = new Thickness(5, 8, 5, 8) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
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
            r.Inlines.Add(new Bold(new Run(Account.Logged.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            //flow.Blocks.Add(r);

            return flow;
        }
        #endregion
    }
}
