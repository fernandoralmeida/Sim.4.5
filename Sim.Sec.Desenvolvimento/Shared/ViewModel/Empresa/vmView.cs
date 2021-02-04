using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Empresa
{
    using Model;
    using Mvvm.Commands;
    using Services.Correios;
    using Services.ReceitaFederal;
    using Mvvm.Observers;
    using Account;

    class vmView : NotifyProperty
    {
        #region Declarations
        NavigationService ns;
        private BitmapImage _captcha;

        private mPJ _pessoa_juridica = new mPJ();
        private mFormalizada _formalizacao = new mFormalizada();
        private mSegmentos _segmento = new mSegmentos();

        private string _stringcaptcha;
        private string _getcnpj;

        private bool _starprogress;

        private Visibility _cabecalho;
        private Visibility _dadosempresa;
        private Visibility _sincronizarbrf;
        private Visibility _isenabled;

        private Visibility _blackbox;

        private ICommand _commandsyncbrf;
        private ICommand _commandmanual;
        private ICommand _commandiniciarsync;
        private ICommand _commandretorno;
        private ICommand _commandgravar;
        private ICommand _commandcancelar;
        private ICommand _commandrecaptcha;
        private ICommand _commandsynccep;
        private ICommand _commandresync;

        private Registro _regtipo;

        #endregion

        #region Properties

        public string GetCNPJ
        {
            get { return _getcnpj; }
            set
            {
                _getcnpj = value;
                RaisePropertyChanged("GetCNPJ");
            }
        }

        public string GetStringCaptcha
        {
            get { return _stringcaptcha; }
            set
            {
                _stringcaptcha = value;
                RaisePropertyChanged("GetStringCaptcha");
            }
        }

        public BitmapImage GetCaptcha
        {
            get { return _captcha; }
            set
            {
                _captcha = value;
                RaisePropertyChanged("GetCaptcha");
            }
        }

        public mPJ PessoaJuridica
        {
            get { return _pessoa_juridica; }
            set
            {
                _pessoa_juridica = value;
                RaisePropertyChanged("PessoaJuridica");
            }
        }

        public mFormalizada Formalizacao
        {
            get { return _formalizacao; }
            set
            {
                _formalizacao = value;
                RaisePropertyChanged("Formalizacao");
            }
        }

        public mSegmentos Segmento
        {
            get { return _segmento; }
            set
            {
                _segmento = value;
                RaisePropertyChanged("Segmento");
            }
        }

        public ObservableCollection<mTiposGenericos> ListaPorte
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PJ_Porte WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mTiposGenericos> ListaUsoLocal
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_PJ_UsoLocal WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
        }

        public Visibility Cabecalho
        {
            get { return _cabecalho; }
            set
            {
                _cabecalho = value;
                RaisePropertyChanged("Cabecalho");
            }
        }

        public Visibility DadosEmpresa
        {
            get { return _dadosempresa; }
            set
            {
                _dadosempresa = value;
                RaisePropertyChanged("DadosEmpresa");
            }
        }

        public Visibility SincronizarBRF
        {
            get { return _sincronizarbrf; }
            set
            {
                _sincronizarbrf = value;
                RaisePropertyChanged("SincronizarBRF");
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

        public Visibility IsEnabled
        {
            get { return _isenabled; }
            set
            {
                _isenabled = value;
                RaisePropertyChanged("IsEnabled");
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

        public Registro RegTipo
        {
            get { return _regtipo; }
            set
            {
                _regtipo = value;
                RaisePropertyChanged("RegTipo");
            }
        }

        #endregion

        #region Commands
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

        public ICommand CommandSyncBRF
        {
            get
            {
                if (_commandsyncbrf == null)
                    _commandsyncbrf = new DelegateCommand(ExecuteCommandSyncBRF, null);

                return _commandsyncbrf;
            }
        }

        private void ExecuteCommandSyncBRF(object obj)
        {
            BlackBox = Visibility.Visible;
            StartProgress = true;
            SincronizarBRF = Visibility.Collapsed;
            DadosEmpresa = Visibility.Visible;
            Cabecalho = Visibility.Collapsed;
            //RegTipo = Registro.Novo;
            AsyncCNPJ();
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
                if (GetCNPJ.Length >= 14)
                    ApresentarDados(new mData().ExistPessoaJuridica(GetCNPJ), false);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public ICommand CommandReCaptcha
        {
            get
            {
                if (_commandrecaptcha == null)
                    _commandrecaptcha = new DelegateCommand(ExecuteCommandReCaptcha, null);

                return _commandrecaptcha;
            }
        }

        private void ExecuteCommandReCaptcha(object obj)
        {
            AsyncCaptcha();
        }

        public ICommand CommandReSync
        {
            get
            {
                if (_commandresync == null)
                    _commandresync = new DelegateCommand(ExecuteCommandReSync, null);
                return _commandresync;
            }
        }

        private void ExecuteCommandReSync(object obj)
        {
            RegTipo = Registro.Alteracao;
            SincronizarBRF = Visibility.Visible;
            AsyncCaptcha();
        }

        public ICommand CommandManual
        {
            get
            {
                if (_commandmanual == null)
                    _commandmanual = new DelegateCommand(ExecuteCommandManual, null);
                return _commandmanual;
            }
        }

        private void ExecuteCommandManual(object obj)
        {
            try
            {
                ApresentarDados(new mData().ExistPessoaJuridica(GetCNPJ), true);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public ICommand CommandRetorno
        {
            get
            {
                if (_commandretorno == null)
                    _commandretorno = new DelegateCommand(ExecuteCommandRetorno, null);
                return _commandretorno;
            }
        }

        private void ExecuteCommandRetorno(object obj)
        {
            SincronizarBRF = Visibility.Collapsed;
            DadosEmpresa = Visibility.Collapsed;
            Cabecalho = Visibility.Visible;
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
            SincronizarBRF = Visibility.Collapsed;
            DadosEmpresa = Visibility.Collapsed;
            Cabecalho = Visibility.Visible;

            ns.GoBack();
        }

        #endregion

        #region Constructor

        public vmView()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "EMPRESA";
            RegTipo = Registro.Novo;
            SincronizarBRF = Visibility.Collapsed;
            DadosEmpresa = Visibility.Collapsed;
            Cabecalho = Visibility.Visible;
            BlackBox = Visibility.Collapsed;
            IsEnabled = Visibility.Collapsed;

            if (Logged.Acesso == (int)AccountAccess.Master)
                IsEnabled = Visibility.Visible;

            else
            {
                foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                {
                    if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                    {
                        if (m.Acesso > (int)SubModuloAccess.Consulta)
                        {
                            IsEnabled = Visibility.Visible;
                        }
                    }
                }
            }

            if (AreaTransferencia.CNPJ != string.Empty)
            {
                GetCNPJ = AreaTransferencia.CNPJ;
                CommandIniciarSync.Execute(null);
            }
        }

        #endregion

        #region Functions

        private void AsyncCaptcha() // METODO ASYNC (ASSINCRONO) // PARA NAO TRAVAR O FORM QUANDO DEMORAR
        {
            GetCaptcha = null;
            GetStringCaptcha = string.Empty;
            // EM MEUS TESTES, APÓS ALGUMAS CONSULTAS A RECEITA BLOQUEOU O IP, MESMO ACESSANDO PELO SITE DA RECEITA. NAO FUNCIONOU ATÉ REINICIAR A INTERNET.
            // DESSA FORMA CONSEGUI IMPLEMENTAR ROTINAS DE QUANDO O SERVIÇO FICA LENTO OU É BLOQUEADO

            // PARA BLOQUEAR O SEU IP NA RECEITA, BASTA CLICAR DIVERSAS VEZES NO BOTAO TROCAR IMAGEM. A CONSULTA VAI DEMORAR PARA DAR RETORNO DE ERRO DEVIDO AO BLOQUEIO.
            // PORÉM, CONFORME O MODO ASSINCRONO IMPLEMENTADO ABAIXO, O SISTEMA NÃO FICARÁ TRAVADO ENQUANTO A CONSULTA NAO RETORNA NADA.

            Task<byte[]>.Factory.StartNew(() => CNPJ.Captcha())
                .ContinueWith(task =>
                {
                    if (task.Result != null)
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.StreamSource = new System.IO.MemoryStream(task.Result);
                        bi.EndInit();
                        GetCaptcha = bi;
                    }
                    else
                        MessageBox.Show(CNPJ.Mensagem);
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncCNPJ()
        {

            Task<bool>.Factory.StartNew(() => CNPJ.Consultar(GetCNPJ, GetStringCaptcha))
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {

                        DadosEmpresa = Visibility.Visible;
                        Cabecalho = Visibility.Visible;
                        SincronizarBRF = Visibility.Collapsed;
                        PessoaJuridica.CNPJ = CNPJ.Empresa.Cnpj.ToUpper();
                        PessoaJuridica.MatrizFilial = CNPJ.Empresa.MatrizFilial.ToUpper();
                        PessoaJuridica.Abertura = Convert.ToDateTime(CNPJ.Empresa.DataAbertura);
                        PessoaJuridica.RazaoSocial = CNPJ.Empresa.RazaoSocial.ToUpper();
                        PessoaJuridica.NomeFantasia = CNPJ.Empresa.NomeFantasia.ToUpper();
                        PessoaJuridica.NaturezaJuridica = CNPJ.Empresa.NaturezaJuridica.ToUpper();
                        PessoaJuridica.AtividadePrincipal = CNPJ.Empresa.AtividadeEconomicaPrimaria.ToUpper();
                        PessoaJuridica.AtividadeSecundaria = CNPJ.Empresa.AtividadeEconomicaSecundaria.ToUpper();
                        PessoaJuridica.Logradouro = CNPJ.Empresa.Endereco.ToUpper();
                        PessoaJuridica.Numero = CNPJ.Empresa.Numero.ToUpper();
                        PessoaJuridica.Complemento = CNPJ.Empresa.Complemento.ToUpper();
                        PessoaJuridica.Bairro = CNPJ.Empresa.Bairro.ToUpper();
                        PessoaJuridica.Municipio = CNPJ.Empresa.Cidade.ToUpper();
                        PessoaJuridica.UF = CNPJ.Empresa.UF.ToUpper();
                        PessoaJuridica.CEP = CNPJ.Empresa.CEP;
                        PessoaJuridica.Email = CNPJ.Empresa.Email.ToUpper();
                        PessoaJuridica.Telefones = CNPJ.Empresa.Telefones;
                        PessoaJuridica.SituacaoCadastral = CNPJ.Empresa.SituacaoCadastral.ToUpper();
                        PessoaJuridica.DataSituacaoCadastral = Convert.ToDateTime(CNPJ.Empresa.DataSituacaoCadastral);
                        BlackBox = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show(CNPJ.Mensagem);
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        GetStringCaptcha = string.Empty;
                        SincronizarBRF = Visibility.Collapsed;
                        DadosEmpresa = Visibility.Collapsed;
                        Cabecalho = Visibility.Visible;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncCEP()
        {

            Task<bool>.Factory.StartNew(() => CEP.Consultar(PessoaJuridica.CEP))
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {
                        PessoaJuridica.CEP = new mMascaras().CEP(CEP.Endereco.CEP);
                        PessoaJuridica.Logradouro = CEP.Endereco.Logradouro;
                        PessoaJuridica.Bairro = CEP.Endereco.Bairro;
                        PessoaJuridica.Municipio = CEP.Endereco.Municipio;
                        PessoaJuridica.UF = CEP.Endereco.UF;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        CEP.Endereco.Clear();
                        PessoaJuridica.Logradouro = CEP.Endereco.Logradouro;
                        PessoaJuridica.Bairro = CEP.Endereco.Bairro;
                        PessoaJuridica.Municipio = CEP.Endereco.Municipio;
                        PessoaJuridica.UF = CEP.Endereco.UF;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ApresentarDados(mPJ_Ext obj, bool manual)
        {
            try
            {

                RegTipo = Registro.Novo;

                mPJ_Ext pj_extendida = new mPJ_Ext();

                if (obj == null)
                {
                    if (!manual)
                    {
                        SincronizarBRF = Visibility.Visible;
                        AsyncCaptcha();
                    }
                    else
                    {
                        DadosEmpresa = Visibility.Visible;
                        Cabecalho = Visibility.Visible;
                        PessoaJuridica.CNPJ = new mMascaras().CNPJ(GetCNPJ);
                    }
                    return;
                }

                pj_extendida = obj;
                RegTipo = Registro.Alteracao;

                PessoaJuridica.Indice = pj_extendida.Indice;
                PessoaJuridica.CNPJ = new mMascaras().CNPJ(pj_extendida.CNPJ);
                PessoaJuridica.MatrizFilial = pj_extendida.MatrizFilial;
                PessoaJuridica.Abertura = pj_extendida.Abertura;
                PessoaJuridica.RazaoSocial = pj_extendida.RazaoSocial;
                PessoaJuridica.NomeFantasia = pj_extendida.NomeFantasia;
                PessoaJuridica.NaturezaJuridica = pj_extendida.NaturezaJuridica;
                PessoaJuridica.AtividadePrincipal = pj_extendida.AtividadePrincipal;
                PessoaJuridica.AtividadeSecundaria = pj_extendida.AtividadeSecundaria;
                PessoaJuridica.SituacaoCadastral = pj_extendida.SituacaoCadastral;
                PessoaJuridica.DataSituacaoCadastral = pj_extendida.DataSituacaoCadastral;
                PessoaJuridica.Logradouro = pj_extendida.Logradouro;
                PessoaJuridica.Numero = pj_extendida.Numero;
                PessoaJuridica.Complemento = pj_extendida.Complemento;
                PessoaJuridica.CEP = pj_extendida.CEP;
                PessoaJuridica.Bairro = pj_extendida.Bairro;
                PessoaJuridica.Municipio = pj_extendida.Municipio;
                PessoaJuridica.UF = pj_extendida.UF;
                PessoaJuridica.Email = pj_extendida.Email;
                PessoaJuridica.Telefones = pj_extendida.Telefones;
                PessoaJuridica.Cadastro = pj_extendida.Cadastro;
                PessoaJuridica.Atualizado = pj_extendida.Atualizado;
                PessoaJuridica.Ativo = pj_extendida.Ativo;

                Formalizacao.Indice = pj_extendida.Formalizada.Indice;
                Formalizacao.CNPJ = pj_extendida.Formalizada.CNPJ;
                Formalizacao.Porte = pj_extendida.Formalizada.Porte;
                Formalizacao.UsoLocal = pj_extendida.Formalizada.UsoLocal;
                Formalizacao.InscricaoMunicipal = pj_extendida.Formalizada.InscricaoMunicipal;
                Formalizacao.FormalizadoSE = pj_extendida.Formalizada.FormalizadoSE;
                Formalizacao.Data = pj_extendida.Formalizada.Data;
                Formalizacao.Ativo = pj_extendida.Formalizada.Ativo;

                Segmento.Indice = pj_extendida.Segmentos.Indice;

                Segmento.CNPJ_CPF = pj_extendida.Segmentos.CNPJ_CPF;
                Segmento.Agronegocio = pj_extendida.Segmentos.Agronegocio;
                Segmento.Industria = pj_extendida.Segmentos.Industria;
                Segmento.Comercio = pj_extendida.Segmentos.Comercio;
                Segmento.Servicos = pj_extendida.Segmentos.Servicos;
                Segmento.Ativo = pj_extendida.Segmentos.Ativo;

                SincronizarBRF = Visibility.Collapsed;
                DadosEmpresa = Visibility.Visible;
                Cabecalho = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Gravar(Registro tipo)
        {
            try
            {
                mData mdt = new mData();

                if (tipo == Registro.Novo)
                    PessoaJuridica.Cadastro = DateTime.Now;

                PessoaJuridica.Atualizado = DateTime.Now;
                PessoaJuridica.Ativo = true;

                if (!mdt.GravarPJ(PessoaJuridica, tipo))
                    MessageBox.Show("Erro inesperado :( \nEmpresa não foi cadastrado!", "Sim.Alerta!");
                else
                {

                    Formalizacao.CNPJ = PessoaJuridica.CNPJ;

                    AreaTransferencia.CNPJ = PessoaJuridica.CNPJ;

                    if (tipo == Registro.Novo)
                        Formalizacao.Data = DateTime.Now;

                    Formalizacao.Ativo = true;
                    mdt.GravarFormalizacao(Formalizacao, tipo);

                    Segmento.CNPJ_CPF = PessoaJuridica.CNPJ;
                    Segmento.Ativo = true;

                    if (Segmento.Indice == 0)
                        mdt.GravarSegmentos(Segmento, Registro.Novo);
                    else
                        mdt.GravarSegmentos(Segmento, tipo);

                    MessageBox.Show(string.Format("Registro {0} gravado com sucesso!", tipo), "Sim.Alerta!");

                    if (AreaTransferencia.CadPJ == true)
                        AreaTransferencia.CadPJ = false;

                    if (AreaTransferencia.CNPJ_On == true)
                        AreaTransferencia.CNPJ_On = false;

                    if (AreaTransferencia.MEI_F == true)
                        AreaTransferencia.MEI_F = false;

                    if (AreaTransferencia.MEI_A == true)
                        AreaTransferencia.MEI_A = false;

                    if (AreaTransferencia.MEI_B == true)
                        AreaTransferencia.MEI_B = false;

                    GetCNPJ = string.Empty;
                    GetStringCaptcha = string.Empty;
                    PessoaJuridica.Clear();
                    Formalizacao.Clear();
                    Segmento.Clear();

                    SincronizarBRF = Visibility.Collapsed;
                    DadosEmpresa = Visibility.Collapsed;
                    Cabecalho = Visibility.Visible;

                    ns.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion
    }
}
