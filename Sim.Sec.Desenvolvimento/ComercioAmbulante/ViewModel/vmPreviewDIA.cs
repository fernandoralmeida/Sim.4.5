using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.ViewModel
{

    using Model;
    using Shared.Model;
    using Mvvm.Observers;
    using Mvvm.Commands;

    public class vmPreviewDIA : NotifyProperty
    {

        #region Declarations
        NavigationService ns;
        private DIA _ambulante = new DIA();

        private mDataCM mdatacm = new mDataCM();

        private ObservableCollection<string> _atividades = new ObservableCollection<string>();
        private ObservableCollection<string> _periodos = new ObservableCollection<string>();

        private string _outros = string.Empty;
        private string _situacao = string.Empty;
        private string _validade = string.Empty;

        private bool _istenda;
        private bool _isveiculo;
        private bool _istrailer;
        private bool _iscarrinho;
        private bool _isoutros;


        private bool _starprogress;

        private Visibility _blackbox;
        private Visibility _temlicencaview;
        
        #endregion

        #region Properties
        public DIA Ambulante
        {
            get { return _ambulante; }
            set
            {
                _ambulante = value;
                RaisePropertyChanged("Ambulante");
            }
        }

        public ObservableCollection<string> Atividades
        {
            get { return _atividades; }
            set
            {
                _atividades = value;
                RaisePropertyChanged("Atividades");
            }
        }

        public ObservableCollection<string> Periodos
        {
            get { return _periodos; }
            set
            {
                _periodos = value;
                RaisePropertyChanged("Periodos");
            }
        }

        public ObservableCollection<mTiposGenericos> Situações
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_CAmbulante_Situacao WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public string Outros
        {
            get { return _outros; }
            set
            {
                _outros = value;
                RaisePropertyChanged("Outros");
            }
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

        public string Validade
        {
            get { return _validade; }
            set
            {
                _validade = value;
                RaisePropertyChanged("Validade");
            }
        }

        public bool IsTenda
        {
            get { return _istenda; }
            set
            {
                _istenda = value;
                RaisePropertyChanged("IsTenda");
            }
        }

        public bool IsVeiculo
        {
            get { return _isveiculo; }
            set
            {
                _isveiculo = value;
                RaisePropertyChanged("IsVeiculo");
            }
        }

        public bool IsTrailer
        {
            get { return _istrailer; }
            set
            {
                _istrailer = value;
                RaisePropertyChanged("IsTrailer");
            }
        }

        public bool IsCarrinho
        {
            get { return _iscarrinho; }
            set
            {
                _iscarrinho = value;
                RaisePropertyChanged("IsCarrinho");
            }
        }

        public bool IsOutros
        {
            get { return _isoutros; }
            set
            {
                _isoutros = value;
                RaisePropertyChanged("IsOutros");
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

        public Visibility TemLicencaView
        {
            get { return _temlicencaview; }
            set
            {
                _temlicencaview = value;
                RaisePropertyChanged("TemLicencaView");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandPrint => new RelayCommand(p =>
        {

            FlowDocument _flow = (FlowDocument)p;

            try
            {
                StartProgress = true;
                BlackBox = Visibility.Visible;

                // paginador
                //_flow.PageHeight = 768;
                //_flow.PageWidth = 1104;
                //_flow.ColumnWidth = 1104;
                IDocumentPaginatorSource idocument = _flow as IDocumentPaginatorSource;
                idocument.DocumentPaginator.ComputePageCountAsync();

                //Now print using PrintDialog
                var pd = new PrintDialog();
                //pd.UserPageRangeEnabled = true;
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

        public ICommand CommandSair => new RelayCommand(p => { ns.GoBack(); });
        #endregion

        #region Construtor
        public vmPreviewDIA()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "D.I.A";
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            AsyncMostrarDados(AreaTransferencia.Objeto);
        }
        #endregion

        #region Functions
        public void AsyncMostrarDados(object obj)
        {
            Ambulante = (DIA)obj;

            if (Ambulante.Validade == new DateTime(2001, 1, 1))
                Validade = "-";
            else
            {
                DateTime d = Convert.ToDateTime(Ambulante.Validade);

                var dif = d.Date - Ambulante.Emissao.Date;

                //dif.TotalDays;
                var mes = dif.TotalDays / 30;

                if (mes < 1)
                {
                    var dia = mes * 30;
                    if (dia > 1)
                        Validade = Convert.ToInt32(dia).ToString() + " DIAS"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
                    else
                        Validade = Convert.ToInt32(dia).ToString() + " DIA"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
                }
                else if (mes < 2)
                    Validade = Convert.ToInt32(mes).ToString() + " MÊS"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();
                else
                    Validade = Convert.ToInt32(mes).ToString() + " MESES"; //Convert.ToDateTime(Ambulante.Validade).ToShortDateString();

                if (Ambulante.Validade < DateTime.Now.Date)
                {
                    Validade = "VENCIDO";
                }

                if (Ambulante.Situacao == "BAIXADO")
                    Validade = "BAIXADO - " + Ambulante.Validade;
            }
        }
        #endregion

    }
}
