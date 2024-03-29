﻿using System;
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

    public class vmReportAmbulante : VMBase, IPrintBox
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private mDataCM mdatacm = new mDataCM();
        private mDataReport mdatar = new mDataReport();
        private ObservableCollection<Ambulante> _dia = new ObservableCollection<Ambulante>();
        private ObservableCollection<BarChart> _charts = new ObservableCollection<BarChart>();
        private FlowDocument flwdoc = new FlowDocument();

        private DateTime _datai;
        private DateTime _dataf;

        private List<string> _filtros = new List<string>();

        private string _getatividade = string.Empty;
        private string _getlocal = string.Empty;
        private string _getforma = string.Empty;
        private string _gethorario = string.Empty;
        private int _getsituacao = 0;
        private string _report_type;

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

        public ObservableCollection<Ambulante> Lista_DIA
        {
            get { return _dia; }
            set { _dia = value; RaisePropertyChanged("Lista_DIA"); }
        }

        public List<string> Report_Types { get { return new List<string>() { "ATIVOS", "VENCIDOS", "SEM DATA DE VENCIMENTO", "BAIXADOS" }; } }

        public string Report_Type
        {
            get { return _report_type; }
            set { _report_type = value; RaisePropertyChanged("Report_Type"); }
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

        public string FormaAtuacao
        {
            get { return _getforma; }
            set
            {
                _getforma = value; RaisePropertyChanged("FormaAtuacao");
            }
        }

        public string HorarioAtuacao
        {
            get { return _gethorario; }
            set
            {
                _gethorario = value; RaisePropertyChanged("HorarioAtuacao");
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
            Report_Ambulantes();
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
            Report_Ambulantes();
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
        public vmReportAmbulante()
        {
            GlobalNavigation.Pagina = "RELATÓRIO AMBULANTES";
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

            if (FormaAtuacao == string.Empty || FormaAtuacao == null)
            {
                _param.Add("%");//3
                Filtros.Add("[FORMTA ATUAÇÃO = TODOS]");
            }
            else
            {
                _param.Add(FormaAtuacao);
                Filtros.Add("[ORMTA ATUAÇÃO = " + FormaAtuacao + "]");
            }

            if (HorarioAtuacao == string.Empty || HorarioAtuacao == null)
            {
                _param.Add("%");
                Filtros.Add("[HORARIO ATUAÇÃO = TODOS]");
            }
            else
            {
                _param.Add(HorarioAtuacao);
                Filtros.Add("[HORARIO ATUAÇÃO = " + HorarioAtuacao + "]");
            }


            return _param;
        }

        private async void Report_Ambulantes()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            PrintBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            Filtros.Clear();

            var t = Task.Run(() => new Repositorio.RAmbulante().RAmbulantes(Parametros()));

            await t;

            if (t.IsCompleted)
            {
                await FlowDoc.Dispatcher.BeginInvoke(new Action(delegate () { FlowDoc = PreviewInTable(t.Result); }));
                BlackBox = Visibility.Collapsed;
                MainBox = Visibility.Collapsed;
                PrintBox = Visibility.Visible;
                StartProgress = false;
            }
        }

        private FlowDocument PreviewInTable(ObservableCollection<Ambulante> obj)
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
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("RELATÓRIO: {0}", f))) { Padding = new Thickness(5) }) { ColumnSpan = 4, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
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
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CADASTRO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("AMBULANTE"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("LOCAL"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("DATA CADASTRO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(1, 0, 0, 0), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (Ambulante a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Cadastro + "\nDE: " + a.DataCadastro.ToShortDateString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

                    Paragraph np = new Paragraph();
                    np.Padding = new Thickness(5);
                    np.Inlines.Add(new Run(string.Format("TITULAR: {0}", a.Titular.Nome)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("AUXILIAR: {0}", a.Auxiliar.Nome)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("ATIVIDADE: {0}", a.Atividade)));
                    np.Inlines.Add(new LineBreak());
                    np.Inlines.Add(new Run(string.Format("FORMA ATUAÇÃO: {0} - {1}", a.FormaAtuacao, a.UsaVeiculo)));
                    
                    row.Cells.Add(new TableCell(np) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Local)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.DataCadastro.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

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
