using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Diagnostics;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.ViewModel
{
    using Model;
    using Shared.Model;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Controls.ViewModels;
    using Repositorio;

    public class vmCadAmbulante : VMBase
    {
        #region Declarations
        NavigationService ns;

        private Ambulante _ambulante = new Ambulante();

        ObservableCollection<Autorizados> _titular = new ObservableCollection<Autorizados>();
        ObservableCollection<Autorizados> _auxiliar = new ObservableCollection<Autorizados>();

        private ObservableCollection<mPeriodos> _listartimework = new ObservableCollection<mPeriodos>();

        private Visibility _viewlistaeventos;
        private Visibility _viewpj;
        private Visibility _vieweventos;
        private Visibility _viewbutton;
        private Visibility _viewlistapj;
        private Visibility _temempresa;
        private Visibility _textboxoutrosview;
        private Visibility _viewinfo;
        private Visibility _existeauxiliar;

        private bool _isactive;
        private bool _somentepf;

        private bool _istenda;
        private bool _isveiculo;
        private bool _istrailer;
        private bool _iscarrinho;
        private bool _isoutros;

        private string _getoutros = string.Empty;
        private int _selectedrow = 0;
        private int _selectedrow2 = 0;
        private string _getcpf;

        #endregion

        #region Properties

        public ObservableCollection<mPeriodos> ListarTimeWork
        {
            get { return _listartimework; }
            set
            {
                _listartimework = value;
                RaisePropertyChanged("ListarTimeWork");
            }
        }

        public Ambulante Ambulante
        {
            get { return _ambulante; }
            set
            {
                _ambulante = value;

                RaisePropertyChanged("Ambulante");
            }
        }

        public ObservableCollection<Autorizados> Titular
        {
            get { return _titular; }
            set
            {
                _titular = value;
                RaisePropertyChanged("Titular");
            }
        }

        public ObservableCollection<Autorizados> Auxiliar
        {
            get { return _auxiliar; }
            set
            {
                _auxiliar = value;
                RaisePropertyChanged("Auxiliar");
            }
        }

        public string GetCPF
        {
            get { return _getcpf; }
            set
            {
                _getcpf = value;
                RaisePropertyChanged("GetCPF");
            }
        }

        public string GetOutros
        {
            get { return _getoutros; }
            set
            {
                _getoutros = value;
                RaisePropertyChanged("GetOutros");
            }
        }

        public int SelectedRow
        {
            get { return _selectedrow; }
            set
            {
                _selectedrow = value;
                RaisePropertyChanged("SelectedRow");
            }
        }

        public int SelectedRow2
        {
            get { return _selectedrow2; }
            set
            {
                _selectedrow2 = value;
                RaisePropertyChanged("SelectedRow2");
            }
        }

        public Visibility ExisteAuxiliar
        {
            get { return _existeauxiliar; }
            set { _existeauxiliar = value;
                RaisePropertyChanged("ExisteAuxiliar");
            }
        }

        public Visibility ViewListaEventos
        {
            get { return _viewlistaeventos; }
            set
            {
                _viewlistaeventos = value;
                RaisePropertyChanged("ViewListaEventos");
            }
        }

        public Visibility ViewListaPJ
        {
            get { return _viewlistapj; }
            set
            {
                _viewlistapj = value;
                RaisePropertyChanged("ViewListaPJ");
            }
        }

        public Visibility ViewPJ
        {
            get { return _viewpj; }
            set { _viewpj = value; RaisePropertyChanged("ViewPJ"); }
        }

        public Visibility ViewEventos
        {
            get { return _vieweventos; }
            set
            {
                _vieweventos = value;
                RaisePropertyChanged("ViewEventos");
            }
        }

        public Visibility ViewButton
        {
            get { return _viewbutton; }
            set
            {
                _viewbutton = value;
                RaisePropertyChanged("ViewButton");
            }
        }

        public Visibility TemEmpresa
        {
            get { return _temempresa; }
            set
            {
                _temempresa = value;
                RaisePropertyChanged("TemEmpresa");
            }
        }

        public Visibility TextBoxOutrosView
        {
            get { return _textboxoutrosview; }
            set
            {
                _textboxoutrosview = value;
                RaisePropertyChanged("TextBoxOutrosView");
            }
        }

        public Visibility ViewInfo
        {
            get { return _viewinfo; }
            set { _viewinfo = value; RaisePropertyChanged("ViewInfo"); }
        }

        public bool SomentePF
        {
            get { return _somentepf; }
            set
            {
                _somentepf = value;

                if (value == true)
                {
                    ViewPJ = Visibility.Collapsed;

                }
                else
                {
                    ViewPJ = Visibility.Visible;
                }

                RaisePropertyChanged("SomentePF");
            }
        }

        public bool IsActive
        {
            get { return _isactive; }
            set
            {
                _isactive = value;

                RaisePropertyChanged("IsAtive");
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

                if (value == false)
                    TextBoxOutrosView = Visibility.Collapsed;
                else
                    TextBoxOutrosView = Visibility.Visible;

                RaisePropertyChanged("IsOutros");
            }
        }
        #endregion

        #region Commands
        public ICommand CommandSave => new RelayCommand(p => { AsyncGravar(); });

        public ICommand CommandCancelar => new RelayCommand(p => { ns.GoBack(); });


        public ICommand CommandAlterar => new RelayCommand(p =>
        {

            try
            {
                if (Titular.Count < 0)
                    return;

                string identificador = new mMascaras().Remove((string)p);

                switch (identificador.Length)
                {
                    case 11:
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Pessoa/pNovo.xaml", UriKind.Relative));
                        AreaTransferencia.CPF = (string)p;// Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPF = true;
                        break;

                    case 14:
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        AreaTransferencia.CNPJ = (string)p; // Atendimento.Cliente.Inscricao;
                        AreaTransferencia.CadPJ = true;
                        break;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        });

        public ICommand CommandGoBack => new RelayCommand(p =>
        {

            ViewListaEventos = Visibility.Collapsed;

        });

        public ICommand CommandRemoveEvento => new RelayCommand(p =>
        {

            ViewEventos = Visibility.Collapsed;
            ViewButton = Visibility.Visible;

        });

        public ICommand CommandGetCPF => new RelayCommand(p =>
        {

            if (GetCPF != string.Empty && new mMascaras().Remove(GetCPF.TrimEnd()).Length == 11)
            {
                if (new mData().ExistPessoaFisica(GetCPF.TrimEnd()) != null)
                {

                    AsyncAuxiliar(GetCPF.TrimEnd());
                    
                }
                else
                {
                    MessageBox.Show("CPF não encontrado!", "Sim.Alerta!");
                    AreaTransferencia.CPF = new mMascaras().Remove(GetCPF.TrimEnd());
                    ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Pessoa/pNovo.xaml", UriKind.Relative));
                }
            }

        });

        public ICommand CommandSelectedCNPJ => new RelayCommand(p =>
                    {

                        Task<mPF_Ext>.Factory.StartNew(() => new mData().ExistPessoaFisica((string)p)).ContinueWith(task_two =>
                        {
                            if (task_two.IsCompleted)
                            {
                                Auxiliar = new ObservableCollection<Autorizados>();
                                Auxiliar.Add(new Autorizados()
                                {
                                    CPF = task_two.Result.CPF,
                                    Nome = task_two.Result.Nome,
                                    RG = task_two.Result.RG,
                                    Tel = task_two.Result.Telefones
                                });

                            }
                        },
                        System.Threading.CancellationToken.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.FromCurrentSynchronizationContext());
                    });

        public ICommand CommandAddTimeWork => new RelayCommand(p =>
        {

            string _per = string.Empty;

            var cbx = (ComboBox)p;
            /*
            if (Manha) _per += "MANHÃ;";
            if (Tarde) _per += "TARDE;";
            if (Noite) _per += "NOITE;";

            ListarTimeWork.Add(new mPeriodos()
            {
                Ativo = true,
                Periodos = _per,
                Dias = cbx.Text
            });

            Manha = false;
            Tarde = false;
            Noite = false;*/
            cbx.SelectedIndex = 0;

        });

        public ICommand CommandRemoveTimeWork => new RelayCommand(p =>
        {
            ListarTimeWork.RemoveAt(SelectedRow2);
        });

        public ICommand CommandRemoveAuxiliar => new RelayCommand(p => {

            Auxiliar.Clear();
            ExisteAuxiliar = Visibility.Visible;
        
        });

        #endregion

        #region Constructor
        public vmCadAmbulante()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "CADASTRO";
            GlobalNotifyProperty.GlobalPropertyChanged += GlobalNotifyProperty_GlobalPropertyChanged;
            BlackBox = Visibility.Collapsed;
            ViewListaEventos = Visibility.Collapsed;
            ViewEventos = Visibility.Collapsed;
            ViewButton = Visibility.Visible;
            ViewListaPJ = Visibility.Collapsed;
            ViewInfo = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;
            ExisteAuxiliar = Visibility.Visible;
            IsOutros = false;

            StartProgress = false;
            Ambulante.Cadastro = Codigo();
            AsyncMostrarDados(AreaTransferencia.CPF);
            SomentePF = true;
            IsActive = true;
        }

        #endregion

        #region Methods
        private void GlobalNotifyProperty_GlobalPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CadPF")
                if (AreaTransferencia.CadPF == false)
                    AsyncTitular(AreaTransferencia.CPF);

            if (e.PropertyName == "CadPJ")
                if (AreaTransferencia.CadPJ == false)
                    AsyncAuxiliar(AreaTransferencia.CPF);
        }
        #endregion

        #region Functions
        private string Codigo()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"CCA{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
        }

        private async void AsyncGravar()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            Ambulante.Titular = Titular[0];

            if (Auxiliar.Count > 0)
                Ambulante.Auxiliar = Auxiliar[0];

            string _ativ = string.Empty;

            /*
            foreach (mCNAE a in ListaAtividades)
            {
                _ativ += string.Format("{0} - {1};", new mMascaras().CNAE(a.CNAE), a.Ocupacao);
            }*/

            Ambulante.Atividade = _ativ;

            string _instalacao = string.Empty;

            if (IsTenda) _instalacao += "TENDA;";
            if (IsVeiculo) _instalacao += "VEÍCULO;";
            if (IsCarrinho) _instalacao += "CARRINHO;";
            if (IsTrailer) _instalacao += "TRAILER;";
            if (IsOutros) _instalacao += string.Format("OUTROS[{0}]", GetOutros);

            Ambulante.FormaAtuacao = _instalacao;

            string _pwork = string.Empty;

            foreach (mPeriodos p in ListarTimeWork)
            {
                _pwork += string.Format("{0} - {1}/", p.Dias.ToUpper(), p.Periodos.ToUpper());
            }

            Ambulante.HorarioTrabalho = _pwork;

            //Ambulante.DataCadastro = DateTime.Now;
            Ambulante.UltimaAlteracao = DateTime.Now;
            Ambulante.Ativo = true;

            var t = Task<bool>.Factory.StartNew(() => new RAmbulante().GravarAmbulante(Ambulante));

            await t;

            if (t.Result)
            {
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
                AreaTransferencia.CadastroAmbulante = Ambulante.Cadastro;
                AreaTransferencia.CadAmbulanteOK = true;
                AsyncMessageBox("Comerciante Cadastrado!", DialogBoxColor.Green, true);

            }
            else
            {
                BlackBox = Visibility.Collapsed;
                StartProgress = false;
                AsyncMessageBox("Erro no cadastro!", DialogBoxColor.Red, false);
            }

        }

        private async void AsyncTitular(string cpf)
        {
            try
            {
                var t = Task<mPF_Ext>.Factory.StartNew(() => new mData().ExistPessoaFisica(cpf));

                await t;

                if (t.IsCompleted)
                {
                    Titular = new ObservableCollection<Autorizados>();
                    Titular.Add(new Autorizados()
                    {
                        CPF = t.Result.CPF,
                        Nome = t.Result.Nome,
                        RG = t.Result.RG,
                        Tel = t.Result.Telefones
                    });                    
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private async void AsyncAuxiliar(string cpf)
        {
            try
            {
                var t = Task<mPF_Ext>.Factory.StartNew(() => new mData().ExistPessoaFisica(cpf));

                await t;

                if (t.IsCompleted)
                {
                    Auxiliar = new ObservableCollection<Autorizados>();
                    Auxiliar.Add(new Autorizados()
                    {
                        CPF = t.Result.CPF,
                        Nome = t.Result.Nome,
                        RG = t.Result.RG,
                        Tel = t.Result.Telefones
                    });

                    ExisteAuxiliar = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private async void AsyncMostrarDados(string _cpf)
        {
            try
            {
                var t = Task<Ambulante>.Factory.StartNew(() => new RAmbulante().GetAmbulante(_cpf));

                await t;

                if (t.IsCompleted)
                {


                    if (t.Result != null)
                    {

                        Ambulante = t.Result;
                        Titular.Clear();
                        Auxiliar.Clear();
                        Titular.Add(t.Result.Titular);
                        Auxiliar.Add(t.Result.Auxiliar);

                        if (Auxiliar[0].CPF == string.Empty)
                        {
                            SomentePF = true;
                            TemEmpresa = Visibility.Visible;
                        }
                        else
                        {
                            SomentePF = false;
                            TemEmpresa = Visibility.Collapsed;
                        }


                        if (t.Result.Auxiliar.CPF == string.Empty)
                            Auxiliar.Clear();


                        if (t.Result.FormaAtuacao.Contains("TENDA;"))
                            IsTenda = true;

                        if (t.Result.FormaAtuacao.Contains("VEÍCULO;"))
                            IsVeiculo = true;

                        if (t.Result.FormaAtuacao.Contains("CARRINHO;"))
                            IsCarrinho = true;

                        if (t.Result.FormaAtuacao.Contains("TRAILER;"))
                            IsTrailer = true;

                        if (t.Result.FormaAtuacao.Contains("OUTROS"))
                        {
                            IsOutros = true;

                            string input = t.Result.FormaAtuacao;
                            string[] testing = Regex.Matches(input, @"\[(.+?)\]")
                                                        .Cast<Match>()
                                                        .Select(s => s.Groups[1].Value).ToArray();

                            GetOutros = testing[0];
                        }

                        string[] _twk = t.Result.HorarioTrabalho.Split('/');

                        foreach (string wk in _twk)
                        {
                            if (wk != string.Empty)
                            {
                                string[] iwk = wk.Split('-');
                                ListarTimeWork.Add(new mPeriodos() { Dias = iwk[0].TrimEnd(), Periodos = iwk[1].TrimStart() });
                            }
                        }

                        ViewInfo = Visibility.Visible;


                    }
                    else
                        AsyncTitular(AreaTransferencia.CPF);
                    

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
