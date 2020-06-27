using System;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Collections.Generic;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Agenda
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Controls.ViewModels;

    class vmInscricao : VMBase
    {
        #region Declarations
        NavigationService ns;
        private mData mdata = new mData();
        ObservableCollection<mAgenda> _lista = new ObservableCollection<mAgenda>();
        ObservableCollection<mCliente> _lpj = new ObservableCollection<mCliente>();
        mAgenda _agd = new mAgenda();
        mInscricao _inscricao = new mInscricao();
        mCliente _pf = new mCliente();
        mCliente _pj = new mCliente();

        private Visibility _viewlistaeventos;
        private Visibility _viewpj;
        private Visibility _vieweventos;
        private Visibility _viewbutton;
        private Visibility _viewlistapj;
        private Visibility _viewgetcpf;

        private bool _isactive;
        private bool _somentepf;
        private int _settipo;
        private int _setestado;
        private int _setsetor;

        private string _getcpf;

        private ICommand _commandsave;
        private ICommand _commandcancelar;
        private ICommand _commandshoweventos;
        private ICommand _commandselectedevento;
        private ICommand _commandgoback;
        private ICommand _commandremoveevento;
        private ICommand _commandselectedcnpj;
        private ICommand _commandsyncrefresh;
        private ICommand _commandfiltrar;
        #endregion

        #region Properties

        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public ObservableCollection<mTiposGenericos> Setores
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Setores WHERE (Ativo = True) ORDER BY Setor"); }
        }

        public ObservableCollection<mTiposGenericos> Estados
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Estado WHERE (Ativo = True) ORDER BY Valor"); }
        }

        public ObservableCollection<mAgenda> ListarEventos
        {
            get { return _lista; }
            set
            {
                _lista = value;
                RaisePropertyChanged("ListarEventos");
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

        public mInscricao Insc
        {
            get { return _inscricao; }
            set
            {
                _inscricao = value;
                RaisePropertyChanged("Inscricao");
            }
        }

        public mCliente PF
        {
            get { return _pf; }
            set
            {
                _pf = value;
                RaisePropertyChanged("PF");
            }
        }

        public mCliente PJ
        {
            get { return _pj; }
            set
            {
                _pj = value;
                RaisePropertyChanged("PJ");
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

        public Visibility ViewGetCPF
        {
            get { return _viewgetcpf; }
            set
            {
                _viewgetcpf = value;
                RaisePropertyChanged("ViewGetCPF");
            }
        }

        public int setTipo
        {
            get { return _settipo; }
            set
            {
                _settipo = value;
                RaisePropertyChanged("setTipo");
            }
        }

        public int setEstado
        {
            get { return _setestado; }
            set
            {
                _setestado = value;
                RaisePropertyChanged("setEstado");
            }
        }

        public int setSetor
        {
            get { return _setsetor; }
            set
            {
                _setsetor = value;
                RaisePropertyChanged("setSetor");
            }
        }

        public bool SomentePF
        {
            get { return _somentepf; }
            set
            {
                _somentepf = value;

                if (value == true)
                    ViewPJ = Visibility.Collapsed;
                else
                    ViewPJ = Visibility.Visible;

                RaisePropertyChanged("SomentePF");
            }
        }

        public bool IsActive
        {
            get { return _isactive; }
            set
            {
                _isactive = value;

                //AsyncListarEventos();

                RaisePropertyChanged("IsAtive");
            }
        }

        public string getCPF
        {
            get { return _getcpf; }
            set
            {
                _getcpf = value;
                RaisePropertyChanged("getCPF");
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
                        AsyncGravarInscricao();
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

        public ICommand CommandShowEventos
        {
            get
            {
                if (_commandshoweventos == null)
                    _commandshoweventos = new RelayCommand(p => {

                        ViewListaEventos = Visibility.Visible;
                        AsyncListarEventos();

                    });

                return _commandshoweventos;
            }
        }

        public ICommand CommandSelectedEvento
        {
            get
            {
                if (_commandselectedevento == null)
                    _commandselectedevento = new RelayCommand(p => {

                        AsyncSelectedEvento(p.ToString());

                    });

                return _commandselectedevento;
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
                        Agenda.Clear();
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
                                PJ = new mCliente();
                                PJ.Inscricao = task_two.Result.CNPJ;
                                PJ.NomeRazao = task_two.Result.RazaoSocial;
                                PJ.Telefones = task_two.Result.Telefones;
                                PJ.Email = task_two.Result.Email;
                                ViewListaPJ = Visibility.Collapsed;
                            }
                        },
                        System.Threading.CancellationToken.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.FromCurrentSynchronizationContext());
                    });

                return _commandselectedcnpj;
            }
        }

        public ICommand CommandSyncRefresh
        {
            get
            {
                if (_commandsyncrefresh == null)
                {
                    _commandsyncrefresh = new RelayCommand(p =>
                    {

                        AsyncListarEventos();

                    });

                }

                return _commandsyncrefresh;
            }
        }

        public ICommand CommandFiltrar
        {
            get
            {

                if (_commandfiltrar == null)
                    _commandfiltrar = new RelayCommand(p => {

                        if (getCPF != string.Empty)
                        {
                            ViewGetCPF = Visibility.Collapsed;
                            AsyncMostrarCliente(getCPF);
                        }
                    });

                return _commandfiltrar;
            }
        }
        #endregion

        #region Constructor
        public vmInscricao()
        {
            ns = GlobalNavigation.NavService;
            Agenda.Data = DateTime.Now;
            BlackBox = Visibility.Collapsed;
            ViewListaEventos = Visibility.Collapsed;
            ViewEventos = Visibility.Collapsed;
            ViewButton = Visibility.Visible;
            ViewListaPJ = Visibility.Collapsed;
            ViewGetCPF = Visibility.Collapsed;
            StartProgress = false;
            AsyncMostrarCliente(AreaTransferencia.CPF);
            SomentePF = false;
            IsActive = false;
            setTipo = 0;
            setEstado = 0;
        }
        #endregion

        #region Methods

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

            r = string.Format(@"IC{0}{1}{2}{3}{4}{5}",
                a, m, d, hs, mn, ss);

            return r;
        }

        private void AsyncGravarInscricao()
        {
            try
            {
                BlackBox = Visibility.Visible;
                StartProgress = true;

                Insc.Inscricao = Codigo();
                Insc.Evento = Agenda.Codigo;
                Insc.Atendimento = string.Empty;
                Insc.Nome = PF.NomeRazao;
                Insc.CPF = PF.Inscricao;
                Insc.CNPJ = PJ.Inscricao;

                if (SomentePF)
                    Insc.CNPJ = string.Empty;

                Insc.Telefone = PF.Telefones;
                Insc.Email = PF.Email;
                Insc.Data = DateTime.Now;
                Insc.Ativo = true;

                int r = 0;

                r = mdata.VagasEvento(Insc.Evento) - mdata.InscritosEventos(Insc.Evento);

                if (r > 0)
                {

                    if (!mdata.ExiteClienteEvento(Insc.Evento, Insc.CPF))
                    {

                        Task<bool>.Factory.StartNew(() => mdata.GravarInscricao(Insc))
                            .ContinueWith(task =>
                            {
                                if (task.Result)
                                {
                                    BlackBox = Visibility.Collapsed;
                                    StartProgress = false;
                                    AreaTransferencia.Inscricao.Add(Insc.Inscricao);
                                    AreaTransferencia.Evento = Agenda.Codigo;
                                    AreaTransferencia.InscricaoOK = true;
                                    ns.GoBack();
                                }
                                else
                                {
                                    BlackBox = Visibility.Collapsed;
                                    StartProgress = false;
                                    //MessageBox.Show(task.Exception.Message);
                                    //throw new Exception(task.Exception.Message);
                                }
                            },
                            System.Threading.CancellationToken.None,
                            TaskContinuationOptions.None,
                            TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                        throw new Exception(string.Format("Cliente {0} já encontra-se inscrito no evento,\nOu encontra-se na lista de cancelados", Insc.CPF));
                }
                else
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    throw new Exception("Evento com vagas esgotadas!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        private void AsyncMostrarCliente(string cpf)
        {
            if (cpf == string.Empty)
            {
                ViewGetCPF = Visibility.Visible;
            }

            Task<mPF_Ext>.Factory.StartNew(() => mdata.ExistPessoaFisica(cpf))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {

                        if (task.Result != null)
                        {

                            PF = new mCliente();
                            PF.Inscricao = task.Result.CPF;
                            PF.NomeRazao = task.Result.Nome;
                            PF.Telefones = task.Result.Telefones;
                            PF.Email = task.Result.Email;
                            ListaPJ.Clear();

                            if (task.Result.ColecaoVinculos.Count < 1)
                                SomentePF = true;

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
                                            PJ = new mCliente();
                                            PJ.Inscricao = task_two.Result.CNPJ;
                                            PJ.NomeRazao = task_two.Result.RazaoSocial;
                                            PJ.Telefones = task_two.Result.Telefones;
                                            PJ.Email = task_two.Result.Email;
                                        }
                                    }
                                },
                                System.Threading.CancellationToken.None,
                                TaskContinuationOptions.None,
                                TaskScheduler.FromCurrentSynchronizationContext());
                            }

                        }
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AsyncListarEventos()
        {

            List<string> _list = new List<string>();

            if (!IsActive)
            {
                _list.Add(string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));
                _list.Add("31/12/3000");
            }
            else
            {
                _list.Add("01/01/1900");
                _list.Add("31/12/3000");
            }

            if (setEstado == 0)
            {
                _list.Add("0");
                _list.Add("99");
            }
            else
            {
                _list.Add(setEstado.ToString());
                _list.Add(setEstado.ToString());
            }

            if (setTipo == 0)
            {
                _list.Add("0");
                _list.Add("99");
            }
            else
            {
                _list.Add(setTipo.ToString());
                _list.Add(setTipo.ToString());
            }

            if (setSetor == 0)
            {
                _list.Add("0");
                _list.Add("99");
            }
            else
            {
                _list.Add(setSetor.ToString());
                _list.Add(setSetor.ToString());
            }

            //ViewListaEventos = Visibility.Visible;
            Task<ObservableCollection<mAgenda>>.Factory.StartNew(() => mdata.Agenda(_list))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListarEventos = task.Result;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

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
                        Agenda.Setor = task.Result.Setor;
                        Agenda.Estado = task.Result.Estado;
                        Agenda.Hora = task.Result.Hora;
                        Agenda.Criacao = task.Result.Criacao;
                        Agenda.Ativo = task.Result.Ativo;
                        ViewListaEventos = Visibility.Collapsed;
                        ViewEventos = Visibility.Visible;
                        ViewButton = Visibility.Collapsed;
                    }
                },
                System.Threading.CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}
