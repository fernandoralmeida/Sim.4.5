using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Pessoa
{
    using Model;
    using Mvvm.Commands;
    using Services.Correios;
    using Mvvm.Observers;
    using System.ComponentModel;

    class vmNovo : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private ObservableCollection<mVinculos_Ext> _listavinculos = new ObservableCollection<mVinculos_Ext>();

        private mPF _pessoa_fisica = new mPF();
        private mPerfil _perfil = new mPerfil();
        private mDeficiencia _deficiencia = new mDeficiencia();
        private mVinculos _vinculo = new mVinculos();

        private Visibility _cabecalho;
        private Visibility _dadopessoa;
        private Visibility _deficienteview;
        private Visibility _blackbox;
        private Visibility _vincularempresa;
        private Visibility _viewdatagrid;

        private bool _starprogress;
        private bool _temdeficiencia;

        private string _cnpj = string.Empty;

        private int _selectedrow;

        private ICommand _commandiniciarsync;
        private ICommand _commandgravar;
        private ICommand _commandcancelar;
        private ICommand _commandsynccep;
        private ICommand _commandgetempresa;
        private ICommand _commandremovecnpj;

        private Registro _regtipo;
        #endregion

        #region Properties

        public ObservableCollection<mTiposGenericos> ListaVinculos
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PJPF_Vinculos_Tipos WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> ListaPerfis
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PF_Perfil_Tipos WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> ListaSexo
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PF_Sexo WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mVinculos_Ext> ListaVinculoPJ
        {
            get { return _listavinculos; }
            set
            {
                _listavinculos = value;
                RaisePropertyChanged("ListaPJ");
            }
        }

        public mPF PessoaFisica
        {
            get { return _pessoa_fisica; }
            set
            {
                _pessoa_fisica = value;
                RaisePropertyChanged("PessoaFisica");
            }
        }

        public mPerfil Perfis
        {
            get { return _perfil; }
            set
            {
                _perfil = value;
                RaisePropertyChanged("Perfis");
            }
        }

        public mVinculos Vinculos { get { return _vinculo; } set { _vinculo = value; RaisePropertyChanged("Vinculos"); } }

        public mDeficiencia Deficiente
        {
            get { return _deficiencia; }
            set
            {
                _deficiencia = value;
                RaisePropertyChanged("Deficiente");
            }
        }

        public Visibility DeficienteView { get { return _deficienteview; } set { _deficienteview = value; RaisePropertyChanged("DeficienteView"); } }

        public Visibility Cabecalho
        {
            get { return _cabecalho; }
            set
            {
                _cabecalho = value;
                RaisePropertyChanged("Cabecalho");
            }
        }

        public Visibility DadosPessoa
        {
            get { return _dadopessoa; }
            set
            {
                _dadopessoa = value;
                RaisePropertyChanged("DadosPessoa");
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

        public Visibility VincularEmpresa
        {
            get { return _vincularempresa; }
            set
            {
                _vincularempresa = value;
                RaisePropertyChanged("VincularEmpresa");
            }
        }

        public Visibility ViewDataGrid
        {
            get { return _viewdatagrid; }
            set
            {
                _viewdatagrid = value;
                RaisePropertyChanged("ViewDataGrid");
            }
        }

        public Registro RegTipo
        {
            get { return _regtipo; }
            set
            {
                _regtipo = value;
                RaisePropertyChanged("RegTipo");
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

        public bool TemDeficiencia
        {
            get { return _temdeficiencia; }
            set
            {
                _temdeficiencia = value;
                Deficiente.Deficiencia = value;

                if (value == true)
                    DeficienteView = Visibility.Visible;
                else
                {
                    DeficienteView = Visibility.Collapsed;
                    Deficiente.Fisica = false;
                    Deficiente.Visual = false;
                    Deficiente.Auditiva = false;
                    Deficiente.Intelectual = false;
                }

                RaisePropertyChanged("TemDeficiencia");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
        }

        public string CNPJ
        {
            get { return _cnpj; }
            set
            {
                _cnpj = value.Trim();
                RaisePropertyChanged("CNPJ");
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

        #endregion

        #region Commands

        public ICommand CommandGetEmpresa
        {
            get
            {
                if (_commandgetempresa == null)
                    _commandgetempresa = new DelegateCommand(ExecuteCommandGetEmpresa, null);

                return _commandgetempresa;
            }
        }

        private void ExecuteCommandGetEmpresa(object obj)
        {
            try
            {

                mPJ pj = new mPJ();
                pj = new mData().ConsultaCNPJ_N(new mMascaras().Remove(CNPJ));

                ListaVinculoPJ.Add(new mVinculos_Ext()
                {
                    CNPJ = new mMascaras().CNPJ(pj.CNPJ),
                    RazaoSocical = pj.RazaoSocial,
                    Telefones = pj.Telefones,
                    VinculoEmpresa = ListaVinculos[Vinculos.Vinculo].Nome,
                    VinculoValor = Vinculos.Vinculo,
                    Acao = 1
                });

                if (ListaVinculoPJ.Count > 0)
                    ViewDataGrid = Visibility.Visible;
                else
                    ViewDataGrid = Visibility.Collapsed;
            }
            catch
            {
                if (CNPJ.Length > 0)
                {
                    if (MessageBox.Show("CNPJ " + CNPJ + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        AreaTransferencia.CNPJ = CNPJ;
                        ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                        AreaTransferencia.CNPJ_On = true;
                    }
                }
            }
        }

        public ICommand CommandRemoverCNPJ
        {
            get
            {
                if (_commandremovecnpj == null)
                    _commandremovecnpj = new DelegateCommand(ExecuteCommandRemoverCNPJ, null);

                return _commandremovecnpj;
            }
        }

        private void ExecuteCommandRemoverCNPJ(object obj)
        {
            if (ListaVinculoPJ[SelectedRow].Acao == -1)
                ListaVinculoPJ[SelectedRow].Acao = 0;
            else
                ListaVinculoPJ[SelectedRow].Acao = -1;

            if (ListaVinculoPJ.Count > 0)
                ViewDataGrid = Visibility.Visible;
            else
                ViewDataGrid = Visibility.Collapsed;
        }

        public ICommand CommandSynCEP
        {
            get
            {
                if (_commandsynccep == null)
                    _commandsynccep = new DelegateCommand(ExecuteCommandSyncCEP, null);

                return _commandsynccep;
            }
        }

        private void ExecuteCommandSyncCEP(object obj)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            AsyncCEP();
        }

        public ICommand CommandIniciarSync
        {
            get
            {
                if (_commandiniciarsync == null)
                    _commandiniciarsync = new DelegateCommand(ExecuteCommandIniciarSync, null);
                return _commandiniciarsync;
            }
        }

        private void ExecuteCommandIniciarSync(object obj)
        {
            try
            {
                if (PessoaFisica.CPF.Length >= 11)
                    ApresentarDados(new mData().ExistPessoaFisica(PessoaFisica.CPF));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        public ICommand CommandGravar
        {
            get
            {
                if (_commandgravar == null)
                    _commandgravar = new DelegateCommand(ExecuteCommandGravar, null);
                return _commandgravar;
            }
        }

        private void ExecuteCommandGravar(object obj)
        {
            Gravar(RegTipo);
        }

        public ICommand CommandCancelar
        {
            get
            {
                if (_commandcancelar == null)
                    _commandcancelar = new DelegateCommand(ExecuteCommandCancelar, null);
                return _commandcancelar;
            }
        }

        private void ExecuteCommandCancelar(object obj)
        {
            DadosPessoa = Visibility.Collapsed;
            Cabecalho = Visibility.Visible;
            if (PessoaFisica != null)
                PessoaFisica.Clear();

            ns.GoBack();
        }

        #endregion

        #region Constructor

        public vmNovo()
        {
            ns = GlobalNavigation.NavService;

            GlobalNotifyProperty.GlobalPropertyChanged += GlobalNotifyProperty_GlobalPropertyChanged;

            GlobalNavigation.Pagina = "INCLUIR PESSOA";

            Cabecalho = Visibility.Visible;
            DadosPessoa = Visibility.Collapsed;
            BlackBox = Visibility.Collapsed;
            TemDeficiencia = false;

            if (ListaVinculoPJ.Count > 0)
                ViewDataGrid = Visibility.Visible;
            else
                ViewDataGrid = Visibility.Collapsed;

            if (AreaTransferencia.CPF != string.Empty && AreaTransferencia.CPF != null)
            {
                PessoaFisica.CPF = AreaTransferencia.CPF;
                ApresentarDados(new mData().ExistPessoaFisica(PessoaFisica.CPF));
            }
        }

        #region Methods

        private void GlobalNotifyProperty_GlobalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CNPJ_On")
            {
                if (AreaTransferencia.CNPJ_On == false)
                {
                    CommandGetEmpresa.Execute(null);
                }
            }
        }

        #endregion

        #endregion

        #region Functions
        private void AsyncCEP()
        {

            Task<bool>.Factory.StartNew(() => CEP.Consultar(PessoaFisica.CEP))
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {
                        PessoaFisica.CEP = new mMascaras().CEP(CEP.Endereco.CEP);
                        PessoaFisica.Logradouro = CEP.Endereco.Logradouro;
                        PessoaFisica.Bairro = CEP.Endereco.Bairro;
                        PessoaFisica.Municipio = CEP.Endereco.Municipio;
                        PessoaFisica.UF = CEP.Endereco.UF;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        CEP.Endereco.Clear();
                        PessoaFisica.Logradouro = CEP.Endereco.Logradouro;
                        PessoaFisica.Bairro = CEP.Endereco.Bairro;
                        PessoaFisica.Municipio = CEP.Endereco.Municipio;
                        PessoaFisica.UF = CEP.Endereco.UF;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Gravar(Registro tipo)
        {
            try
            {

                mData mdt = new mData();

                if (tipo == Registro.Novo)
                    PessoaFisica.Cadastro = DateTime.Now;


                PessoaFisica.Atualizado = DateTime.Now;
                PessoaFisica.Ativo = true;

                if (!mdt.GravarPF(PessoaFisica, tipo))
                    MessageBox.Show("Erro inesperado :( \nCliente não cadastrado!", "Sim.Alerta!");
                else
                {
                    //Vinculo
                    if (ListaVinculoPJ.Count > 0)
                        foreach (mVinculos_Ext v in ListaVinculoPJ)
                        {
                            Vinculos.Indice = v.Indice;
                            Vinculos.CNPJ = new mMascaras().Remove(v.CNPJ);
                            Vinculos.CPF = new mMascaras().Remove(PessoaFisica.CPF);
                            Vinculos.Vinculo = v.VinculoValor;
                            Vinculos.Data = DateTime.Now;
                            Vinculos.Ativo = true;
                            Vinculos.Acao = v.Acao;

                            mdt.GravarVinculos(Vinculos, RegTipo);
                        }

                    //Perfil Empreendedor, Empresario
                    Perfis.CPF = PessoaFisica.CPF;
                    Perfis.Ativo = true;
                    mdt.GravarPerfil(Perfis, RegTipo);

                    //Tem Deficiencia
                    Deficiente.CPF = PessoaFisica.CPF;
                    Deficiente.Ativo = true;
                    mdt.GravarDeficiencia(Deficiente, RegTipo);

                    MessageBox.Show(string.Format("Registro [{0}] gravado com sucesso!", tipo), "Sim.Alerta!");

                    PessoaFisica.Clear();
                    DadosPessoa = Visibility.Collapsed;
                    Cabecalho = Visibility.Visible;

                    if (AreaTransferencia.CadPF == true)
                        AreaTransferencia.CadPF = false;

                    if (AreaTransferencia.CPF_On == true)
                        AreaTransferencia.CPF_On = false;

                    ns.GoBack();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApresentarDados(mPF_Ext obj)
        {
            try
            {

                RegTipo = Registro.Novo;
                DadosPessoa = Visibility.Visible;
                Cabecalho = Visibility.Collapsed;

                mPF_Ext pf_extendida = new mPF_Ext();

                if (obj == null)
                {
                    PessoaFisica.CPF = new mMascaras().CPF(PessoaFisica.CPF);
                    PessoaFisica.Cadastro = DateTime.Now;
                    PessoaFisica.Atualizado = DateTime.Now;
                    PessoaFisica.Ativo = true;
                    return;
                }

                RegTipo = Registro.Alteracao;
                pf_extendida = obj;

                PessoaFisica.Indice = pf_extendida.Indice;
                PessoaFisica.RG = pf_extendida.RG;
                PessoaFisica.CPF = new mMascaras().CPF(pf_extendida.CPF);
                PessoaFisica.Nome = pf_extendida.Nome;
                PessoaFisica.DataNascimento = pf_extendida.DataNascimento;
                PessoaFisica.Sexo = pf_extendida.Sexo;
                PessoaFisica.Logradouro = pf_extendida.Logradouro;
                PessoaFisica.Numero = pf_extendida.Numero;
                PessoaFisica.Complemento = pf_extendida.Complemento;
                PessoaFisica.CEP = new mMascaras().CEP(pf_extendida.CEP);
                PessoaFisica.Bairro = pf_extendida.Bairro;
                PessoaFisica.Municipio = pf_extendida.Municipio;
                PessoaFisica.UF = pf_extendida.UF;
                PessoaFisica.Email = pf_extendida.Email;
                PessoaFisica.Telefones = pf_extendida.Telefones;
                PessoaFisica.Cadastro = pf_extendida.Cadastro;
                PessoaFisica.Atualizado = pf_extendida.Atualizado;
                PessoaFisica.Ativo = pf_extendida.Ativo;

                Perfis.Indice = pf_extendida.Perfil.Indice;
                Perfis.CPF = pf_extendida.Perfil.CPF;
                Perfis.Perfil = pf_extendida.Perfil.Perfil;
                Perfis.Negocio = pf_extendida.Perfil.Negocio;
                Perfis.Ativo = pf_extendida.Perfil.Ativo;

                TemDeficiencia = pf_extendida.Deficiente.Deficiencia;

                Deficiente.Indice = pf_extendida.Deficiente.Indice;
                Deficiente.CPF = pf_extendida.Deficiente.CPF;
                Deficiente.Deficiencia = pf_extendida.Deficiente.Deficiencia;
                Deficiente.Fisica = pf_extendida.Deficiente.Fisica;
                Deficiente.Visual = pf_extendida.Deficiente.Visual;
                Deficiente.Auditiva = pf_extendida.Deficiente.Auditiva;
                Deficiente.Intelectual = pf_extendida.Deficiente.Intelectual;
                Deficiente.Ativo = pf_extendida.Deficiente.Ativo;

                ListaVinculoPJ.Clear();

                foreach (mVinculos pf in pf_extendida.ColecaoVinculos)
                {

                    mPJ pj = new mPJ();
                    pj = new mData().ConsultaCNPJ_N(pf.CNPJ);

                    ListaVinculoPJ.Add(new mVinculos_Ext()
                    {
                        Indice = pf.Indice,
                        CNPJ = new mMascaras().CNPJ(pf.CNPJ),
                        RazaoSocical = pj.RazaoSocial,
                        Telefones = pj.Telefones,
                        VinculoEmpresa = ListaVinculos[pf.Vinculo].Nome,
                        VinculoValor = pf.Vinculo,
                        Acao = 0
                    });
                }

                if (ListaVinculoPJ.Count > 0)
                    ViewDataGrid = Visibility.Visible;
                else
                    ViewDataGrid = Visibility.Collapsed;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
