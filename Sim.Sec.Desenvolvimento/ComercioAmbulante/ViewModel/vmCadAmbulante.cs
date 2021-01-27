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
    public class vmCadAmbulante : VMBase
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        private mDataCM mdatacm = new mDataCM();

        private mAmbulante _ambulante = new mAmbulante();

        ObservableCollection<mCliente> _lpj = new ObservableCollection<mCliente>();
        ObservableCollection<mCliente> _pf = new ObservableCollection<mCliente>();
        ObservableCollection<mCliente> _pj = new ObservableCollection<mCliente>();

        private ObservableCollection<mCNAE> _listacnae = new ObservableCollection<mCNAE>();
        private ObservableCollection<mCNAE> _listaatividade = new ObservableCollection<mCNAE>();
        private ObservableCollection<mPeriodos> _listartimework = new ObservableCollection<mPeriodos>();

        private Visibility _viewlistaeventos;
        private Visibility _viewpj;
        private Visibility _vieweventos;
        private Visibility _viewbutton;
        private Visibility _viewlistapj;
        private Visibility _cnaebox;
        private Visibility _temempresa;
        private Visibility _textboxoutrosview;
        private Visibility _viewinfo;
        private Visibility _temlicenca = Visibility.Collapsed;

        private bool _isactive;
        private bool _somentepf;

        private bool _manha;
        private bool _tarde;
        private bool _noite;

        private bool _istenda;
        private bool _isveiculo;
        private bool _istrailer;
        private bool _iscarrinho;
        private bool _isoutros;
        private bool _datalicencaview;

        private string _getoutros = string.Empty;
        private string _cnae = string.Empty;
        private int _selectedrow = 0;
        private int _selectedrow2 = 0;

        private ICommand _commandsave;
        private ICommand _commandcancelar;
        private ICommand _commandgoback;
        private ICommand _commandremoveevento;
        private ICommand _commandselectedcnpj;

        private ICommand _commandcnae;
        private ICommand _commandremovecnae;
        private ICommand _commandaddcnae;
        private ICommand _commandclosecnaebox;
        private ICommand _commandlistarcnaebox;
        private ICommand _commandaddtimework;
        private ICommand _commandremovetimework;
        private ICommand _commandalterar;
        private ICommand _commandgetcnae;
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

        public mAmbulante Ambulante
        {
            get { return _ambulante; }
            set
            {
                _ambulante = value;

                RaisePropertyChanged("Ambulante");
            }
        }

        public ObservableCollection<mCliente> Titular
        {
            get { return _pf; }
            set
            {
                _pf = value;
                RaisePropertyChanged("Titular");
            }
        }

        public ObservableCollection<mCliente> Auxiliar
        {
            get { return _pj; }
            set
            {
                _pj = value;
                RaisePropertyChanged("Auxiliar");
            }
        }

        public ObservableCollection<mCliente> ListaPJ
        {
            get
            {
                return _lpj;
            }
            set
            {
                _lpj = value;
                RaisePropertyChanged("ListaPJ");
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
        public ICommand CommandSave
        {
            get
            {
                if (_commandsave == null)
                    _commandsave = new RelayCommand(p =>
                    {
                        AsyncGravar();
                    });
                return _commandsave;
            }
        }

        public ICommand CommandCancelar
        {
            get
            {
                if (_commandcancelar == null)
                    _commandcancelar = new RelayCommand(p =>
                    {
                        ns.GoBack();
                    });
                return _commandcancelar;
            }
        }

        public ICommand CommandAlterar
        {
            get
            {
                if (_commandalterar == null)
                    _commandalterar = new RelayCommand(p => {

                        try
                        {
                            if (Titular.Count < 0)
                                return;

                            string identificador = new mMascaras().Remove((string)p);

                            //Cliente.Clear();
                            //PJ.Clear();
                            //PF.Clear();

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
                return _commandalterar;
            }
        }

        public ICommand CommandGoBack
        {
            get
            {
                if (_commandgoback == null)
                    _commandgoback = new RelayCommand(p => {

                        ViewListaEventos = Visibility.Collapsed;

                    });

                return _commandgoback;
            }
        }

        public ICommand CommandRemoveEvento
        {
            get
            {
                if (_commandremoveevento == null)
                    _commandremoveevento = new RelayCommand(p => {

                        ViewEventos = Visibility.Collapsed;
                        ViewButton = Visibility.Visible;

                    });

                return _commandremoveevento;
            }
        }

        public ICommand CommandSelectedCNPJ
        {
            get
            {
                if (_commandselectedcnpj == null)
                    _commandselectedcnpj = new RelayCommand(p =>
                    {

                        Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica((string)p)).ContinueWith(task_two =>
                        {
                            if (task_two.IsCompleted)
                            {
                                Auxiliar = new ObservableCollection<mCliente>();
                                Auxiliar.Add(new mCliente()
                                {
                                    Inscricao = task_two.Result.CNPJ,
                                    NomeRazao = task_two.Result.RazaoSocial,
                                    Telefones = task_two.Result.Telefones,
                                    Email = task_two.Result.Email
                                });

                            }
                        },
                        System.Threading.CancellationToken.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.FromCurrentSynchronizationContext());
                    });

                return _commandselectedcnpj;
            }
        }

        public ICommand CommandAddTimeWork
        {
            get
            {
                if (_commandaddtimework == null)
                    _commandaddtimework = new RelayCommand(p => {

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
                return _commandaddtimework;
            }
        }

        public ICommand CommandRemoveTimeWork
        {
            get
            {
                if (_commandremovetimework == null)
                    _commandremovetimework = new RelayCommand(p => {
                        ListarTimeWork.RemoveAt(SelectedRow2);
                    });

                return _commandremovetimework;
            }
        }

        /*
        public ICommand CommandCNAE
        {
            get
            {
                if (_commandcnae == null)
                    _commandcnae = new RelayCommand(p => {
                        ListaAtividades.Add(new mData().ConsultaCNAE(new mMascaras().CNAE_V(GetCNAE)));
                    });

                return _commandcnae;
            }
        }

        public ICommand CommandAddCNAE
        {
            get
            {
                if (_commandaddcnae == null)
                    _commandaddcnae = new RelayCommand(p =>
                    {
                        mCNAE _cnae = new mCNAE();

                        var objetos = (object[])p;
                        _cnae.CNAE = new mMascaras().CNAE((string)objetos[0]);
                        _cnae.Descricao = (string)objetos[1];
                        _cnae.Ocupacao = (string)objetos[2];

                        ListaAtividades.Add(_cnae);
                    });
                return _commandaddcnae;
            }
        }

        public ICommand CommandRemoveCNAE
        {
            get
            {
                if (_commandremovecnae == null)
                    _commandremovecnae = new RelayCommand(p => {
                        ListaAtividades.RemoveAt(SelectedRow);
                    });
                return _commandremovecnae;
            }
        }

        public ICommand CommandGetCNAE
        {
            get
            {
                if (_commandgetcnae == null)
                    _commandgetcnae = new RelayCommand(p => {

                        if (Auxiliar.Count > 0)
                        {
                            if (Auxiliar[0].Inscricao != string.Empty)
                                AsyncGetAtividades(Auxiliar[0].Inscricao);
                        }

                    });

                return _commandgetcnae;
            }
        }

        public ICommand CommandListarCnaeBox
        {
            get
            {
                if (_commandlistarcnaebox == null)
                    _commandlistarcnaebox = new RelayCommand(p =>
                    {
                        try
                        {
                            CnaeBox = Visibility.Visible;
                            AsyncCNAE();
                        }
                        catch (Exception ex)
                        { MessageBox.Show(ex.Message, "Sim.Alerta!"); }
                    });

                return _commandlistarcnaebox;
            }
        }

        public ICommand CommandCloseCnaeBox
        {
            get
            {
                if (_commandclosecnaebox == null)
                    _commandclosecnaebox = new RelayCommand(p => { CnaeBox = Visibility.Collapsed; });

                return _commandclosecnaebox;
            }
        }*/

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
            IsOutros = false;
            
            StartProgress = false;
            Ambulante.Cadastro = Codigo();
            Ambulante.Situacao = 1;
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
                    AsyncPessoa(AreaTransferencia.CPF);

            if (e.PropertyName == "CadPJ")
                if (AreaTransferencia.CadPJ == false)
                    AsyncEmpresa(AreaTransferencia.CNPJ);
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

        private void AsyncGravar()
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;

            //Ambulante.Cadastro = Codigo();
            Ambulante.Atendimento = string.Empty;
            //Ambulante.TemLicenca = DataLicencaView;

            Ambulante.Pessoa = Titular[0];

            if (Auxiliar.Count > 0)
                Ambulante.Empresa = Auxiliar[0];

            string _ativ = string.Empty;

            /*
            foreach (mCNAE a in ListaAtividades)
            {
                _ativ += string.Format("{0} - {1};", new mMascaras().CNAE(a.CNAE), a.Ocupacao);
            }*/

            Ambulante.Atividades = _ativ;

            string _instalacao = string.Empty;

            if (IsTenda) _instalacao += "TENDA;";
            if (IsVeiculo) _instalacao += "VEÍCULO;";
            if (IsCarrinho) _instalacao += "CARRINHO;";
            if (IsTrailer) _instalacao += "TRAILER;";
            if (IsOutros) _instalacao += string.Format("OUTROS[{0}]", GetOutros);

            Ambulante.TipoInstalacoes = _instalacao;

            string _pwork = string.Empty;

            foreach (mPeriodos p in ListarTimeWork)
            {
                _pwork += string.Format("{0} - {1}/", p.Dias.ToUpper(), p.Periodos.ToUpper());
            }

            Ambulante.PeridoTrabalho = _pwork;

            //Ambulante.DataCadastro = DateTime.Now;
            Ambulante.DataAlteracao = DateTime.Now;
            Ambulante.Ativo = true;

            Task<bool>.Factory.StartNew(() => mdatacm.GravarAmbulante(Ambulante))
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {

                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;

                        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate {

                            AreaTransferencia.CadastroAmbulante = Ambulante.Cadastro;
                            AreaTransferencia.CadAmbulanteOK = true;
                            AsyncMessageBox("Comerciante Cadastrado!", DialogBoxColor.Green, true);

                        }));
                    }
                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;

                        Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate {

                            AsyncMessageBox("Erro no cadastro!", DialogBoxColor.Red, false);

                        }));
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncPessoa(string cpf)
        {
            try
            {

                Task<mPF_Ext>.Factory.StartNew(() => mdata.ExistPessoaFisica(cpf))
                    .ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            Titular = new ObservableCollection<mCliente>();
                            Titular.Add(new mCliente()
                            {
                                Inscricao = task.Result.CPF,
                                NomeRazao = task.Result.Nome,
                                Telefones = task.Result.RG,
                                Email = task.Result.Telefones
                            });

                            ListaPJ.Clear();
                            Auxiliar.Clear();

                            if (task.Result.ColecaoVinculos.Count < 1)
                            {
                                SomentePF = true;
                                TemEmpresa = Visibility.Visible;
                            }
                            else
                            {
                                SomentePF = false;
                                TemEmpresa = Visibility.Collapsed;
                            }

                            foreach (mVinculos v in task.Result.ColecaoVinculos)
                            {
                                Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(v.CNPJ)).ContinueWith(task_two =>
                                {
                                    if (task_two.IsCompleted)
                                    {
                                        if (task.Result.ColecaoVinculos.Count > 1)
                                        {
                                            ListaPJ.Add(new mCliente()
                                            {
                                                Inscricao = task_two.Result.CNPJ,
                                                NomeRazao = task_two.Result.RazaoSocial,
                                                Telefones = task_two.Result.Telefones,
                                                Email = task_two.Result.Email
                                            });

                                            ViewListaPJ = Visibility.Visible;
                                        }
                                        else
                                        {

                                            Auxiliar = new ObservableCollection<mCliente>();
                                            Auxiliar.Add(new mCliente()
                                            {
                                                Inscricao = task_two.Result.CNPJ,
                                                NomeRazao = task_two.Result.RazaoSocial,
                                                Telefones = task_two.Result.Telefones,
                                                Email = task_two.Result.Email
                                            });
                                        }
                                    }
                                },
                                System.Threading.CancellationToken.None,
                                TaskContinuationOptions.None,
                                TaskScheduler.FromCurrentSynchronizationContext());
                            }

                        }
                    },
                    System.Threading.CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void AsyncEmpresa(string _cnpj)
        {
            try
            {
                if (_cnae != string.Empty)
                    Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(_cnpj))
                    .ContinueWith(task_two =>
                    {
                        if (task_two.IsCompleted)
                        {

                            if (task_two != null)
                            {
                                Auxiliar = new ObservableCollection<mCliente>();
                                Auxiliar.Add(new mCliente()
                                {
                                    Inscricao = task_two.Result.CNPJ,
                                    NomeRazao = task_two.Result.RazaoSocial,
                                    Telefones = task_two.Result.Telefones,
                                    Email = task_two.Result.Email
                                });
                            }
                        }
                    },
                    System.Threading.CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void AsyncGetAtividades(string _cnpj)
        {
            try
            {
                Task<mPJ_Ext>.Factory.StartNew(() => mdata.ExistPessoaJuridica(_cnpj))
                    .ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            try
                            {
                                string _cnae = string.Empty;

                                _cnae = task.Result.AtividadePrincipal.Substring(0, 10);

                                //ListaAtividades.Add(new mData().ConsultaCNAE(new mMascaras().CNAE_V(_cnae)));
                            }
                            catch
                            {
                                MessageBox.Show(string.Format("CNAE {0} não encontrado, informe manualmente!", _cnae), "Sim.Alerta!");
                            }
                        }
                    },
                    System.Threading.CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AsyncMostrarDados(string _cpf)
        {
            try
            {
                Task<mAmbulante>.Factory.StartNew(() => mdatacm.GetCAmbulante(_cpf))
                    .ContinueWith(task =>
                    {
                        if (task.IsCompleted)
                        {
                            try
                            {

                                if (task.Result != null)
                                {

                                    if (task.Result.Pessoa.Inscricao == _cpf || task.Result.Cadastro == _cpf)
                                    {

                                        Ambulante = task.Result;
                                        Titular.Clear();
                                        Auxiliar.Clear();
                                        Titular.Add(task.Result.Pessoa);
                                        Auxiliar.Add(task.Result.Empresa);

                                        //DataLicencaView = Ambulante.TemLicenca;

                                        if (Auxiliar[0].Inscricao == string.Empty)
                                        {
                                            SomentePF = true;
                                            TemEmpresa = Visibility.Visible;
                                        }
                                        else
                                        {
                                            SomentePF = false;
                                            TemEmpresa = Visibility.Collapsed;
                                        }


                                        if (task.Result.Empresa.Inscricao == string.Empty)
                                            Auxiliar.Clear();

                                        string[] _atv = task.Result.Atividades.Split(';');

                                        /*
                                        foreach (string atv in _atv)
                                        {
                                            if (atv != string.Empty)
                                                ListaAtividades.Add(new mCNAE() { CNAE = atv.Substring(0, 10), Ocupacao = atv.Remove(0, 13) });
                                        }*/

                                        if (task.Result.TipoInstalacoes.Contains("TENDA;"))
                                            IsTenda = true;

                                        if (task.Result.TipoInstalacoes.Contains("VEÍCULO;"))
                                            IsVeiculo = true;

                                        if (task.Result.TipoInstalacoes.Contains("CARRINHO;"))
                                            IsCarrinho = true;

                                        if (task.Result.TipoInstalacoes.Contains("TRAILER;"))
                                            IsTrailer = true;

                                        if (task.Result.TipoInstalacoes.Contains("OUTROS"))
                                        {
                                            IsOutros = true;

                                            string input = task.Result.TipoInstalacoes;
                                            string[] testing = Regex.Matches(input, @"\[(.+?)\]")
                                                                        .Cast<Match>()
                                                                        .Select(s => s.Groups[1].Value).ToArray();

                                            GetOutros = testing[0];
                                        }

                                        string[] _twk = task.Result.PeridoTrabalho.Split('/');

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
                                    {
                                        AsyncPessoa(AreaTransferencia.CPF);
                                        AsyncEmpresa(AreaTransferencia.CNPJ);
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(string.Format("Erro '{0}' inesperado, informe o Suporte!", ex.Message), "Sim.Alerta!");
                            }
                        }
                    },
                    System.Threading.CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
