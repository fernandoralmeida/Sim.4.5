using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Controls;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Viabilidade
{
    using Model;
    using Mvvm.Commands;
    using Services.Correios;
    using Mvvm.Observers;

    class vmView : NotifyProperty
    {
        #region Declarations
        NavigationService ns;

        private ObservableCollection<mCNAE> _listacnae = new ObservableCollection<mCNAE>();
        private ObservableCollection<mCNAE> _listaatividade = new ObservableCollection<mCNAE>();
        private mViabilidade _viabilidade = new mViabilidade();
        private ObservableCollection<mCliente> _requetente = new ObservableCollection<mCliente>();

        private Visibility _blackbox;
        private Visibility _mailbox;
        private Visibility _cnaebox;

        private ICommand _commandcnae;
        private ICommand _commandremovecnae;
        private ICommand _commandmailbox;
        private ICommand _commandcancel;
        private ICommand _commandsendmail;
        private ICommand _commandclosemailbox;
        private ICommand _commandgetcpf;
        private ICommand _commandsynccep;
        private ICommand _commandaddcnae;
        private ICommand _commandclosecnaebox;
        private ICommand _commandlistarcnaebox;
        private ICommand _commandalterar;

        private string _cpf = string.Empty;
        private string _cnae = string.Empty;
        private string _mailto = string.Empty;
        private string _mailfrom = string.Empty;
        private string _mailsubject = string.Empty;
        private string _estviabilidade = string.Empty;

        private bool _emailenviado;
        private bool _starprogress;
        private int _selectedrow = 0;
        #endregion

        #region Properties

        public ObservableCollection<mCNAE> ListaAtividades
        {
            get { return _listaatividade; }
            set { _listaatividade = value; RaisePropertyChanged("ListaAtividades"); }
        }

        public ObservableCollection<mCNAE> ListaCNAE
        {
            get { return _listacnae; }
            set { _listacnae = value; RaisePropertyChanged("ListaCNAE"); }
        }

        public mViabilidade Viabilidade
        {
            get { return _viabilidade; }
            set
            {
                _viabilidade = value;
                RaisePropertyChanged("Viabilidade");
            }
        }

        public ObservableCollection<mCliente> Requerente
        {
            get { return _requetente; }
            set
            {
                _requetente = value;
                RaisePropertyChanged("Requerente");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
        }

        public string EstudoViabilidade
        {
            get { return _estviabilidade; }
            set { _estviabilidade = value; RaisePropertyChanged("EstudoViabilidade"); }
        }

        public string MailFrom
        {
            get { return _mailfrom; }
            set { _mailfrom = value; RaisePropertyChanged("MailFrom"); }
        }

        public string MailTo
        {
            get { return _mailto; }
            set { _mailto = value; RaisePropertyChanged("MailTo"); }
        }

        public string MailSubject
        {
            get { return _mailsubject; }
            set { _mailsubject = value; RaisePropertyChanged("MailSubject"); }
        }

        public string GetCNAE
        {
            get { return _cnae; }
            set
            {
                _cnae = value;
                RaisePropertyChanged("GetCNAE");
            }
        }

        public string CPF
        {
            get { return _cpf; }
            set
            {
                _cpf = value;
                RaisePropertyChanged("CPF");
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

        public Visibility MailBox
        {
            get { return _mailbox; }
            set
            {
                _mailbox = value;
                RaisePropertyChanged("MailBox");
            }
        }


        public Visibility CnaeBox
        {
            get { return _cnaebox; }
            set
            {
                _cnaebox = value;
                RaisePropertyChanged("CnaeBox");
            }
        }
        #endregion

        #region Commands

        public ICommand CommandGetCPF
        {
            get
            {
                if (_commandgetcpf == null)
                    _commandgetcpf = new DelegateCommand(ExecuteCommandGetCPF, null);
                return _commandgetcpf;

            }
        }

        private void ExecuteCommandGetCPF(object obj)
        {
            try
            {
                if (Viabilidade.Requerente.Inscricao == string.Empty)
                    return;

                string identificador = new mMascaras().Remove(Viabilidade.Requerente.Inscricao);

                Requerente.Clear();

                switch (identificador.Length)
                {
                    case 11:
                        ClientePF(new mData().ExistPessoaFisica(identificador));
                        break;

                    case 14:
                        ClientePJ(new mData().ExistPessoaJuridica(identificador));
                        break;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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

        public ICommand CommandCNAE
        {
            get
            {
                if (_commandcnae == null)
                    _commandcnae = new DelegateCommand(ExecuteCommandCNAE, null);
                return _commandcnae;
            }
        }

        private void ExecuteCommandCNAE(object obj)
        {
            ListaAtividades.Add(new mData().ConsultaCNAE(new mMascaras().CNAE_V(GetCNAE)));
        }

        public ICommand CommandAddCNAE
        {
            get
            {
                if (_commandaddcnae == null)
                    _commandaddcnae = new DelegateCommand(ExecuteCommandAddCNAE, null);
                return _commandaddcnae;
            }
        }

        private void ExecuteCommandAddCNAE(object obj)
        {
            mCNAE _cnae = new mCNAE();

            var objetos = (object[])obj;
            _cnae.CNAE = (string)objetos[0];
            _cnae.Descricao = (string)objetos[1];
            _cnae.Ocupacao = (string)objetos[2];
            ListaAtividades.Add(_cnae);
        }

        public ICommand CommandRemoveCNAE
        {
            get
            {
                if (_commandremovecnae == null)
                    _commandremovecnae = new DelegateCommand(ExecuteCommandRemoveCNAE, null);
                return _commandremovecnae;
            }
        }

        private void ExecuteCommandRemoveCNAE(object obj)
        {
            ListaAtividades.RemoveAt(SelectedRow);
        }

        public ICommand CommandListarCnaeBox
        {
            get
            {
                if (_commandlistarcnaebox == null)
                    _commandlistarcnaebox = new DelegateCommand(ExecuteCommandListarCnaeBox, null);
                return _commandlistarcnaebox;
            }
        }

        private void ExecuteCommandListarCnaeBox(object obj)
        {
            try
            {
                CnaeBox = Visibility.Visible;
                ////AsyncCNAE();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Sim.Alerta!"); }
        }

        public ICommand CommandCloseCnaeBox
        {
            get
            {
                if (_commandclosecnaebox == null)
                    _commandclosecnaebox = new DelegateCommand(ExecuteCommandCloseCnaeBox, null);

                return _commandclosecnaebox;
            }
        }

        private void ExecuteCommandCloseCnaeBox(object obj)
        {
            CnaeBox = Visibility.Collapsed;
        }

        public ICommand CommandMailBox
        {
            get
            {
                if (_commandmailbox == null)
                    _commandmailbox = new DelegateCommand(ExecuteCommandMailBox, null);
                return _commandmailbox;
            }
        }

        private void ExecuteCommandMailBox(object obj)
        {
            CreateMail();
            MailBox = Visibility.Visible;
        }

        public ICommand CommandSendMail
        {
            get
            {
                if (_commandsendmail == null)
                    _commandsendmail = new DelegateCommand(ExecuteCommandSendMail, null);
                return _commandsendmail;
            }
        }

        private void ExecuteCommandSendMail(object obj)
        {            
            if (IsValidEmail(MailTo))
                GravarDados(Registro.Novo);
            else
                MessageBox.Show("Endereço de email inválido!\n" + MailTo, "Sim.Alerta!");

        }

        public ICommand CommandCloseMailBox
        {
            get
            {
                if (_commandclosemailbox == null)
                    _commandclosemailbox = new DelegateCommand(ExecuteCommandCloseMailBox, null);
                return _commandclosemailbox;
            }
        }

        private void ExecuteCommandCloseMailBox(object obj)
        {
            MailBox = Visibility.Collapsed;
        }

        public ICommand CommandCancel
        {
            get
            {
                if (_commandcancel == null)
                    _commandcancel = new DelegateCommand(ExecuteCommandCancel, null);
                return _commandcancel;
            }
        }

        private void ExecuteCommandCancel(object obj)
        {

            if (MessageBox.Show(string.Format("Cancelar Viabilidade {0}?", EstudoViabilidade), "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                if (ns.CanGoBack)
                    ns.GoBack();
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
                            if (Viabilidade.Requerente.Inscricao == string.Empty)
                                return;

                            string identificador = new mMascaras().Remove(Viabilidade.Requerente.Inscricao);

                            //Requerente.Clear();

                            switch (identificador.Length)
                            {
                                case 11:
                                    ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Pessoa/pNovo.xaml", UriKind.Relative));
                                    AreaTransferencia.CPF = Viabilidade.Requerente.Inscricao;
                                    AreaTransferencia.CadPF = true;
                                    break;

                                case 14:
                                    ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative));
                                    AreaTransferencia.CNPJ = Viabilidade.Requerente.Inscricao;
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
        #endregion

        #region Constructor
        public vmView()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "NOVA VIABILIDADE";
            GlobalNavigation.BrowseBack = Visibility.Collapsed;
            BlackBox = Visibility.Collapsed;
            MailBox = Visibility.Collapsed;
            CnaeBox = Visibility.Collapsed;
            MailTo = @"";
            //MailTo = @"viarapidaempresa@jau.sp.gov.br";
            MailFrom = @"viarapidaempresa@jau.sp.gov.br";
            MailSubject = @"Viabilidade MEI";
            Viabilidade.Data = DateTime.Now;
            EstudoViabilidade = Protocolo();
            AsyncCNAE();
            if (AreaTransferencia.CPF != string.Empty)
            {
                Viabilidade.Requerente.Inscricao = AreaTransferencia.CPF;
                CommandGetCPF.Execute(null);
            }
        }

        #endregion

        #region Functions
        private async void AsyncCNAE()
        {

            string sql = @"SELECT * FROM SDT_SE_CNAE_MEI WHERE (Ativo = True) ORDER BY Ocupacao";

            var t = Task.Run(() => new mData().CNAES(sql)).ContinueWith(
                task=> {

                    if (task.IsCompleted)
                    {
                        ListaCNAE = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        CnaeBox = Visibility.Collapsed;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }

                })
                ;
            await t;            
        }

        private void AsyncCEP()
        {

            Task<bool>.Factory.StartNew(() => CEP.Consultar(Viabilidade.CEP))
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {
                        Viabilidade.CEP = new mMascaras().CEP(CEP.Endereco.CEP);
                        Viabilidade.Logradouro = CEP.Endereco.Logradouro;
                        Viabilidade.Bairro = CEP.Endereco.Bairro;
                        Viabilidade.Municipio = CEP.Endereco.Municipio;
                        Viabilidade.UF = CEP.Endereco.UF;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                    else
                    {
                        CEP.Endereco.Clear();
                        Viabilidade.Logradouro = CEP.Endereco.Logradouro;
                        Viabilidade.Bairro = CEP.Endereco.Bairro;
                        Viabilidade.Municipio = CEP.Endereco.Municipio;
                        Viabilidade.UF = CEP.Endereco.UF;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GravarDados(Registro tipo)
        {
            try
            {
                mData mdt = new mData();
                StringBuilder sb = new StringBuilder();

                Viabilidade.Protocolo = EstudoViabilidade;
                Viabilidade.Requerente = Requerente[0];

                foreach (mCNAE _cnae in ListaAtividades)
                    sb.AppendLine(string.Format(@"{0} - {1}", _cnae.CNAE, _cnae.Descricao));

                Viabilidade.Atividades = sb.ToString().Trim();
                Viabilidade.Perecer = 1;
                Viabilidade.Ativo = true;
                Viabilidade.Operador = Account.Logged.Identificador;
                Viabilidade.RetornoCliente = false;

                AreaTransferencia.Viabilidade = Viabilidade.Protocolo;

                if (!mdt.GravarViabilidade(Viabilidade, tipo))
                    MessageBox.Show("Erro inesperado :( \nViabilidade não cadastrada!", "Sim.Alerta!");
                else
                {

                    if (MessageBox.Show(string.Format("Viabilidade {0} gravado com sucesso! \nEnviar email para setor de Viabilidade?", EstudoViabilidade), "Sim.Alerta!",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        BlackBox = Visibility.Visible;
                        StartProgress = true;
                        AsyncSendMail();
                    }
                    else
                    {
                        MailBox = Visibility.Collapsed;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        NovaViabilidade();
                    }

                    AreaTransferencia.ViabilidadeOK = true;

                    ns.GoBack();
                }
            }
            catch (Exception ex) { MessageBox.Show("Verifique se todos as informações foram preenchidas corretamente" + "\n" + ex.Message, "Sim.Alerta!"); }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void CreateMail()
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format(@"Viabilidade Nº {0}", EstudoViabilidade));
            sb.AppendLine();

            if (DateTime.Now.Hour < 12)
                sb.AppendLine(@"Bom dia.");

            if (DateTime.Now.Hour >= 12)
                sb.AppendLine(@"Boa tarde.");

            sb.AppendLine();
            sb.AppendLine(string.Format(@"Seguem dados para estudo de viabilidade do MEI."));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"CEP: {0}", Viabilidade.CEP));
            sb.AppendLine(string.Format(@"Endereço: {0}, {1}", Viabilidade.Logradouro, Viabilidade.Numero));
            sb.AppendLine(string.Format(@"CTM: {0}", Viabilidade.CTM));
            sb.AppendLine(string.Format(@"Atividades"));

            foreach (mCNAE _cnae in ListaAtividades)
            {
                sb.AppendLine(string.Format(@"{0} - {1} ({2})", _cnae.CNAE, _cnae.Descricao, _cnae.Ocupacao));
            }

            sb.AppendLine();
            sb.AppendLine(@"Att.");
            sb.AppendLine();
            sb.AppendLine(@"Secretaria de Desenvolvimento e Trabalho.");
            sb.AppendLine(string.Format(@"{0}", Account.Logged.Nome));
            sb.AppendLine(@"TEL. 14 3626-8429 / 3626-8430");
            sb.AppendLine();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            sb.AppendLine(string.Format(@"Email enviado do {0} V{1} [{2} {3} ]",
                AppDomain.CurrentDomain.FriendlyName.ToUpper(),
                version,
                DateTime.Now.ToShortDateString(),
                DateTime.Now.ToLongTimeString()));

            Viabilidade.TextoEmail = sb.ToString();

        }

        private void AsyncSendMail()
        {
            Task.Factory.StartNew(() => SendMail())
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {

                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;

                        if (_emailenviado == true)
                        {
                            new mData().AtualizarViabilidadeEmailOk(Viabilidade.Protocolo, true);
                            MailBox = Visibility.Collapsed;
                            NovaViabilidade();
                        }
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

        private void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage(MailFrom, MailTo);
                SmtpClient client = new SmtpClient();

                client.Port = 587;
                client.Host = @"mail.jau.sp.gov.br";
                client.EnableSsl = false;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(MailFrom, "vr62");

                mail.Subject = MailSubject;
                mail.Body = Viabilidade.TextoEmail;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.Delay;
                mail.Headers.Add("Disposition-Notification-To", MailFrom);
                client.Send(mail);

                _emailenviado = true;
            }
            catch (Exception ex)
            {
                _emailenviado = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void NovaViabilidade()
        {
            Viabilidade.Data = DateTime.Now;
            EstudoViabilidade = Protocolo();
            Viabilidade.Clear();
            GetCNAE = string.Empty;
            ListaAtividades.Clear();
            Requerente.Clear();
            Viabilidade.Operador = Account.Logged.Identificador;
        }

        private string Protocolo()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"VR{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
        }

        private void ClientePF(mPF_Ext obj)
        {
            try
            {

                string identificador = new mMascaras().Remove(Viabilidade.Requerente.Inscricao);


                if (obj == null)
                {
                    if (MessageBox.Show("CLIENTE " + Viabilidade.Requerente.Inscricao + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        GlobalNavigation.Navegar = new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative);
                        //NavigationCommands.GoToPage.Execute(new Uri("/Sim.Modulo.Empreendedor;component/View/pAddPFe.xaml", UriKind.Relative), null);
                        AreaTransferencia.CPF = Viabilidade.Requerente.Inscricao;
                    }

                    return;
                }

                Requerente.Add(new mCliente()
                {
                    Inscricao = new mMascaras().CPF(obj.CPF),
                    NomeRazao = obj.Nome,
                    Telefones = obj.Telefones,
                    Email = obj.Email
                });

                Viabilidade.Requerente.Inscricao = new mMascaras().CPF(obj.CPF);
                Viabilidade.Requerente.NomeRazao = obj.Nome;
                Viabilidade.Requerente.Telefones = obj.Telefones;
                Viabilidade.Requerente.Email = obj.Email;

                //Corpo = Visibility.Visible;
                //Cabecalho = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClientePJ(mPJ_Ext obj)
        {
            try
            {

                string identificador = new mMascaras().Remove(Viabilidade.Requerente.Inscricao);


                if (obj == null)
                {
                    if (MessageBox.Show("CLIENTE " + Viabilidade.Requerente.Inscricao + " não encontrado! Cadastrá-lo agora?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        //NavigationCommands.GoToPage.Execute(new Uri("pAddPJe.xaml", UriKind.RelativeOrAbsolute), null);
                        GlobalNavigation.Navegar = new Uri("/Sim.Sec.Desenvolvimento;component/Shared/View/Empresa/pView.xaml", UriKind.Relative);
                        AreaTransferencia.CNPJ = Viabilidade.Requerente.Inscricao;
                    }

                    return;
                }

                Requerente.Add(new mCliente()
                {
                    Inscricao = new mMascaras().CNPJ(obj.CNPJ),
                    NomeRazao = obj.RazaoSocial,
                    Telefones = obj.Telefones,
                    Email = obj.Email
                });

                Viabilidade.Requerente.Inscricao = new mMascaras().CNPJ(obj.CNPJ);
                Viabilidade.Requerente.NomeRazao = obj.RazaoSocial;
                Viabilidade.Requerente.Telefones = obj.Telefones;
                Viabilidade.Requerente.Email = obj.Email;

                //Corpo = Visibility.Visible;
                //Cabecalho = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
