using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sim.Sec.Desenvolvimento.SebraeAqui.ViewModel
{

    using Shared.Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Controls.ViewModels;
    using Account;
    using Model;
    using Shared.Control;

    class vmMain : VMBase, IAgenda
    {
        #region Declarations
        NavigationService ns;
        private mDataSA mdata = new mDataSA();
        private ObservableCollection<mAtendimento> _listaat = new ObservableCollection<mAtendimento>();
        private ObservableCollection<mAgenda> _listag = new ObservableCollection<mAgenda>();

        private FlowDocument _flowdoc = new FlowDocument();
        private FlowDocument _flowtemp = new FlowDocument();
        private mAtendimentoSebrae _atsebrae = new mAtendimentoSebrae();

        private DateTime _datai = DateTime.Now;

        private Visibility _previewbox;
        private Visibility _retvisible = Visibility.Collapsed;

        private string _preview = string.Empty;

        private bool _viewcnpj = false;
        private bool _isadmin;
        private bool _isenable;

        private int _selectedrow;

        private ICommand _commandclosepreview;
        private ICommand _commandpreviewbox;
        private ICommand _commandpreviewbox2;
        private ICommand _commandrefreshdate;
        private ICommand _commandgocnpj;
        private ICommand _commandretcpf;

        private string _eventoselecionado;
        #endregion

        #region Properties
        public ObservableCollection<mTiposGenericos> Tipos
        {
            get { return new mData().Tipos("SELECT * FROM SDT_Agenda_Tipos WHERE (Ativo = True) ORDER BY Tipo"); }
        }

        public FlowDocument FlowDoc
        {
            get { return _flowdoc; }
            set
            {
                _flowdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }

        public mAtendimentoSebrae AtSebrae
        {
            get { return _atsebrae; }
            set
            {
                _atsebrae = value;
                RaisePropertyChanged("AtSebrae");
            }
        }

        public ObservableCollection<mAgenda> ListarAgenda
        {
            get { return _listag; }
            set
            {
                _listag = value;
                RaisePropertyChanged("ListarAgenda");
            }
        }

        public ObservableCollection<mAtendimento> ListarAtendimentos
        {
            get { return _listaat; }
            set
            {
                _listaat = value;
                RaisePropertyChanged("ListarAtendimentos");
            }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set
            {
                _datai = value;
                ListarAtendimentoHojeAsync(Parametros("2"));
                RaisePropertyChanged("DataI");
            }
        }

        public Visibility PreviewBox
        {
            get { return _previewbox; }
            set
            {
                _previewbox = value;
                RaisePropertyChanged("PreviewBox");
            }
        }

        public Visibility RetVisible
        {
            get { return _retvisible; }
            set
            {
                _retvisible = value;
                RaisePropertyChanged("RetVisible");
            }
        }

        public bool IsAdmin
        {
            get { return _isadmin; }
            set
            {
                _isadmin = value;
                RaisePropertyChanged("IsAdmin");
            }
        }

        public bool IsEnable
        {
            get { return _isenable; }
            set
            {
                _isenable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        public string Preview
        {
            get { return _preview; }
            set
            {
                _preview = value;
                RaisePropertyChanged("Preview");
            }
        }

        public string PageName
        {
            get { return GlobalNavigation.Pagina; }
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
        public ICommand CommandGoCNPJ
        {
            get
            {
                if (_commandgocnpj == null)
                    _commandgocnpj = new DelegateCommand(ExecCommandGoCNPJ, null);
                return _commandgocnpj;
            }
        }

        private void ExecCommandGoCNPJ(object obj)
        {
            _viewcnpj = true;
            _flowtemp = FlowDoc;
            FlowDoc = null;
            FlowDoc = FlowPJ((string)obj);
        }

        public ICommand CommandRetCPF
        {
            get
            {
                if (_commandretcpf == null)
                    _commandretcpf = new DelegateCommand(ExecCommandRetCPF, null);
                return _commandretcpf;
            }
        }

        private void ExecCommandRetCPF(object obj)
        {
            _viewcnpj = false;
            FlowDoc = null;
            FlowDoc = _flowtemp;
            RetVisible = Visibility.Collapsed;
        }

        public ICommand CommandClosePreview
        {
            get
            {
                if (_commandclosepreview == null)
                    _commandclosepreview = new DelegateCommand(ExecCommandClosePreview, null);
                return _commandclosepreview;
            }
        }

        private void ExecCommandClosePreview(object obj)
        {
            PreviewBox = Visibility.Collapsed;
            FlowDoc = null;
            FlowDoc = new FlowDocument();
            _viewcnpj = false;
        }

        public ICommand CommandPreviewBox
        {
            get
            {
                if (_commandpreviewbox == null)
                    _commandpreviewbox = new DelegateCommand(ExecCommandPreviewBox, null);
                return _commandpreviewbox;
            }
        }

        private void ExecCommandPreviewBox(object obj)
        {
            try
            {
                //var watch = System.Diagnostics.Stopwatch.StartNew();
                //AsyncApresentarDados(obj);
                ApresentarDadosAsync(obj);


                PreviewBox = Visibility.Visible;

                //watch.Stop();

                //var elapsed_ms = watch.ElapsedMilliseconds;

                //MessageBox.Show($"Tempo de execucão: {elapsed_ms}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandPreviewBox2
        {
            get
            {
                if (_commandpreviewbox2 == null)
                    _commandpreviewbox2 = new DelegateCommand(ExecCommandPreviewBox2, null);
                return _commandpreviewbox2;
            }
        }

        private void ExecCommandPreviewBox2(object obj)
        {
            try
            {
                FlowAtendimentoAsync((string)obj);
                PreviewBox = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandEventoAlterar => new RelayCommand(p =>
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pEditar.xaml", UriKind.Relative));
                    AreaTransferencia.Parametro = (string)p;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pEditar.xaml", UriKind.Relative));
                                AreaTransferencia.Parametro = (string)p;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }

        });

        public ICommand CommandAtSebrae => new RelayCommand(async p =>
        {
            if (await GravarAtSebrae())
            {
                ListarAtendimentos[SelectedRow].AtendimentoSebrae = AtSebrae.AtendimentoSAC;
                ExecCommandClosePreview(null);
            }
            else
            {
                ExecCommandClosePreview(null);
            }
        });

        public ICommand CommandRefreshDate
        {
            get
            {
                if (_commandrefreshdate == null)
                    _commandrefreshdate = new DelegateCommand(ExecCommandRefreshDate, null);

                return _commandrefreshdate;
            }
        }

        private void ExecCommandRefreshDate(object obj)
        {
            //if (GlobalNavigation.Parametro != string.Empty)
                //AsyncListarAtendimentoHoje(Parametros("2"));
        }

        public ICommand CommandEdit => new RelayCommand(p => 
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Atendimento/pEdite.xaml", UriKind.Relative));
                    AreaTransferencia.Parametro = (string)p;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Atendimento/pEdite.xaml", UriKind.Relative));
                                AreaTransferencia.Parametro = (string)p;
                            }
                        }
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sim.Alerta!"); }
        });

        public ICommand CommandAgendaNavigate => new RelayCommand(p => { ns.Navigate(new Uri(p.ToString(), UriKind.RelativeOrAbsolute)); });

        public ICommand CommandEventoAtivo => new RelayCommand(p => { AsyncListarAgendaAtivo(); });

        public ICommand CommandEventoVencido => new RelayCommand(p => { AsyncListarAgendaVencida(); });

        public ICommand CommandEventoCancelado => new RelayCommand(p => { AsyncListarAgendaCancelada(); });

        public ICommand CommandEventoFinalizado => new RelayCommand(p => { AsyncListarAgendaFinalizado(); });

        public ICommand CommandDetalharEvento => new RelayCommand(p =>
        {
            AreaTransferencia.Evento = p.ToString();
            ns.Navigate(new Uri(@"/Sim.Sec.Desenvolvimento;component/Shared/View/Agenda/pDetalhe.xaml", UriKind.RelativeOrAbsolute));
        });

        public ICommand CommandDataPrev => new RelayCommand(p => 
        {
            DataI = DataI.AddDays(-1);
        });

        public ICommand CommandDataNext => new RelayCommand(p =>
        {
            DataI = DataI.AddDays(1);
        });

        public string EventoSelecionado
        {
            get { return _eventoselecionado; }
            set
            {

                _eventoselecionado = value;

                if (_eventoselecionado == "Ativos")
                    CommandEventoAtivo.Execute(null);

                if (_eventoselecionado == "Vencidos")
                    CommandEventoVencido.Execute(null);

                if (_eventoselecionado == "Cancelados")
                    CommandEventoCancelado.Execute(null);

                if (_eventoselecionado == "Finalizados")
                    CommandEventoFinalizado.Execute(null);

                RaisePropertyChanged("EventoSelecionado");
            }
        }
        #endregion

        #region Constructor
        public vmMain()
        {
            GlobalNavigation.Pagina = string.Empty;
            ns = GlobalNavigation.NavService;
            GlobalNavigation.SubModulo = "SEBRAE AQUI";
            GlobalNavigation.Parametro = "2";

            BlackBox = Visibility.Collapsed;
            PreviewBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;
            StartProgress = false;
            AreaTransferencia.Limpar();
              //AsyncListarAtendimentoHoje(Parametros("2"));
            IsEnable = false;
            IsAdmin = false;
            EventoSelecionado = "Ativos";
            //CommandEventoAtivo.Execute(null);

            if (Logged.Acesso != 0)
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    IsEnable = true;
                    IsAdmin = true;
                }

                if (GlobalNavigation.SubModulo.ToLower() == "Sebrae Aqui".ToLower())
                {

                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                                IsEnable = true;

                            if (m.Acesso > (int)SubModuloAccess.Operador)
                                IsAdmin = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region Functions

        private async Task<bool> GravarAtSebrae()
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;
            return await Task<bool>.Run(() =>
            {
                try
                {
                    if (mdata.GravarAtendimentoSebrae(AtSebrae))
                    {

                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;                        
                        SyncMessageBox("Atendimento lançado!", DialogBoxColor.Green);
                        return true;
                    }

                    else
                    {
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        SyncMessageBox("Atendimento não lançado!", DialogBoxColor.Green);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    BlackBox = Visibility.Collapsed;
                    StartProgress = false;
                    SyncMessageBox("Atendimento não lançado!/n" + ex.Message, DialogBoxColor.Green);
                    return false;
                }
            });
        }

        private List<string> Parametros(string param)
        {

            List<string> _param = new List<string>() { };

            _param.Add(DataI.ToShortDateString());

            _param.Add(Logged.Identificador);

            _param.Add(param);

            return _param;
        }

        private async void ListarAtendimentoHojeAsync(List<string> _params)
        {
            await Task.Run(() => {

                ListarAtendimentos = mdata.AtendimentosNow(_params);

            });
        }

        public async void AsyncListarAgenda()
        {

            List<string> _list = new List<string>
            {
                DateTime.Now.ToShortDateString(),
                "1",
                "1"
            };

            await Task.Factory.StartNew(() =>
            {
                ListarAgenda = new mData().Agenda(_list);
            });
        }

        public async void AsyncListarAgendaAtivo()
        {

            List<string> _list = new List<string>
            {
                DateTime.Now.ToShortDateString(),
                "1"
            };

            await Task.Run(() =>
            {
                ListarAgenda = new mData().AgendaAtivo(_list);
            });
        }

        public async void AsyncListarAgendaVencida()
        {

            List<string> _list = new List<string>
            {
                DateTime.Now.ToShortDateString(),
                "1"
            };

            await Task.Run(() =>
            {
                ListarAgenda = new mData().AgendaVencida(_list);
            });
        }

        public async void AsyncListarAgendaCancelada()
        {

            List<string> _list = new List<string>
            {
                "3"
            };

            await Task.Run(() =>
            {
                ListarAgenda = new mData().AgendaCancelada(_list);
            });
        }

        public void AsyncListarAgendaFinalizado()
        {

            List<string> _list = new List<string>
            {
                "2"
            };

            Task.Run(() =>
            {
                ListarAgenda = new mData().AgendaFinalizado(_list);
            });
        }

        private async void ApresentarDadosAsync(object cliente)
        {
            string _cliente = new mMascaras().Remove((string)cliente);

            FlowDocument flow = new FlowDocument();

            await Task.Run(() =>
            {
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate () 
                {
                    var atdo = new mAtendimento();

                    atdo = new mData().Atendimento(_cliente);

                    switch (atdo.Cliente.Inscricao.Length)
                    {
                        case 11:
                            FlowDoc = FlowPF(atdo.Cliente.Inscricao);
                            break;

                        case 14:
                            FlowDoc = FlowPJ(atdo.Cliente.Inscricao);
                            break;
                    }
                }));
            });
        }

        private async void FlowAtendimentoAsync(string protocolo)
        {

            FlowDocument flow = new FlowDocument();

            await Task.Run(() =>
            {
                System.Windows.Threading.DispatcherOperation op =
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(delegate () {

                        FlowDoc = FlowAtendimento(protocolo);

                    }));

                op.Completed += (o, args) =>
                {
                    //BlackBox = Visibility.Collapsed;
                    //MainBox = Visibility.Collapsed;
                    //PrintBox = Visibility.Visible;
                    //StartProgress = false;
                };
            });
        }
        
        private FlowDocument FlowPF(string cliente)
        {

            mPF_Ext pessoafisica = new mPF_Ext();
            pessoafisica = new mData().ExistPessoaFisica(cliente);

            AtSebrae.Atendimento = ListarAtendimentos[SelectedRow].Protocolo;
            AtSebrae.Cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(pessoafisica.CPF),
                    pessoafisica.Nome,
                    pessoafisica.Telefones,
                    pessoafisica.Email);
            AtSebrae.Ativo = true;
            AtSebrae.AtendimentoSAC = ListarAtendimentos[SelectedRow].AtendimentoSebrae;

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("CLIENTE:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}, {2} ", pessoafisica.Nome, new mMascaras().CPF(pessoafisica.CPF), pessoafisica.DataNascimento.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CEP:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.CEP);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ENDEREÇO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}", pessoafisica.Logradouro, pessoafisica.Numero));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("BAIRRO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}-{2}", pessoafisica.Bairro, pessoafisica.Municipio, pessoafisica.UF));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.Telefones + " " + pessoafisica.Email);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("PERFIL:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoafisica.Perfil.PerfilString);
            pr.Inlines.Add(new LineBreak());

            if (pessoafisica.ColecaoVinculos.Count > 0)
                pr.Inlines.Add(new Run("EMPRESAS:") { FontSize = 10, Foreground = Brushes.Gray });


            foreach (mVinculos v in pessoafisica.ColecaoVinculos)
            {
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run("CNPJ "));
                pr.Inlines.Add(new Hyperlink(new Run(new mMascaras().CNPJ(v.CNPJ))) { Command = CommandGoCNPJ, CommandParameter = v.CNPJ });
                pr.Inlines.Add(new Run(" - " + v.VinculoString));
            }

            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("---------- INFO. ATENDIMENTO ----------") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(ListarAtendimentos[SelectedRow].Protocolo + ", " + ListarAtendimentos[SelectedRow].Data + ", " + ListarAtendimentos[SelectedRow].Hora);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SERVIÇOS REALIZADOS:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(ListarAtendimentos[SelectedRow].TipoString);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DESCRIÇÃO ATENDIMENTO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(ListarAtendimentos[SelectedRow].Historico);

            flow.Blocks.Add(pr);

            return flow;
        }

        private FlowDocument FlowPJ(string cliente)
        {

            mPJ_Ext pessoajuridica = new mPJ_Ext();

            var t = Task.Run(() => { pessoajuridica = new mData().ExistPessoaJuridica(cliente); });
            t.Wait();           

            AtSebrae.Atendimento = ListarAtendimentos[SelectedRow].Protocolo;
            AtSebrae.Cliente = string.Format(@"{0}/{1}/{2}/{3}",
                    new mMascaras().Remove(pessoajuridica.CNPJ),
                    pessoajuridica.RazaoSocial,
                    pessoajuridica.Telefones,
                    pessoajuridica.Email);

            AtSebrae.Ativo = true;

            FlowDocument flow = new FlowDocument();
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 11.5 * 96.0;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("EMPRESA:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}, {2} ", new mMascaras().CNPJ(pessoajuridica.CNPJ), pessoajuridica.MatrizFilial, pessoajuridica.Abertura.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ROZÃO SOCIAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.RazaoSocial);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("NOME FANTASIA") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.NomeFantasia);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ATIVIDADE PRINCIPAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.AtividadePrincipal);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ATIVIDADES SECUNDÁRIAS") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.AtividadeSecundaria);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("NATUREZA JURÍDICA") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.NaturezaJuridica);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SITUAÇÃO CADASTRAL") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.SituacaoCadastral + " " + pessoajuridica.DataSituacaoCadastral.ToShortDateString());
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CEP:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.CEP);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("ENDEREÇO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}", pessoajuridica.Logradouro, pessoajuridica.Numero));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("BAIRRO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}, {1}-{2} ", pessoajuridica.Bairro, pessoajuridica.Municipio, pessoajuridica.UF));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(pessoajuridica.Telefones + " " + pessoajuridica.Email);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SETOR") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());

            if (pessoajuridica.Segmentos.Agronegocio)
                pr.Inlines.Add("AGRONEGÓCIO ");

            if (pessoajuridica.Segmentos.Industria)
                pr.Inlines.Add("INDÚSTRIA ");

            if (pessoajuridica.Segmentos.Comercio)
                pr.Inlines.Add("COMÉRCIO ");

            if (pessoajuridica.Segmentos.Servicos)
                pr.Inlines.Add("SERVIÇOS ");

            if (_viewcnpj == true)
            {
                RetVisible = Visibility.Visible;
                /*
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Hyperlink(new Run("VOLTAR")) { Command = CommandRetCPF });
                */
            }

            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("---------- INFO. ATENDIMENTO ----------") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(ListarAtendimentos[SelectedRow].Protocolo + ", " + ListarAtendimentos[SelectedRow].Data + ", " + ListarAtendimentos[SelectedRow].Hora);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("SERVIÇOS REALIZADOS:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(ListarAtendimentos[SelectedRow].TipoString);
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DESCRIÇÃO ATENDIMENTO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(ListarAtendimentos[SelectedRow].Historico);

            flow.Blocks.Add(pr);

            return flow;

        }

        private FlowDocument FlowAtendimento(string protocolo)
        {
            var atdo = new mAtendimento();

            var t = Task.Run(() => { atdo = new mData().Atendimento(protocolo); });
            t.Wait();            

            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            //flow.Background = Brushes.White;
            //Get the page size of an A4 document
            //var pageSize = new System.Windows.Size(8.5 * 96.0, 11.0 * 96.0);
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.PageHeight = 768;
            //flow.PageWidth = 1104;
            //flow.ColumnWidth = 1104;
            //flow.PageHeight = 11.5 * 96.0;
            flow.Foreground = (Brush)Application.Current.Resources["WindowText"];
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 12;
            flow.PagePadding = new Thickness(10);

            Paragraph pr = new Paragraph();
            pr.Inlines.Add(new Run("ATENDIMENTO:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1} {2} ", atdo.Protocolo, atdo.Data.ToShortDateString(), atdo.Hora.ToShortTimeString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CLIENTE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Cliente.NomeRazao));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Cliente.Telefones, atdo.Cliente.Email));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("TIPO E ORIGEM DO ATENDIMENTO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.TipoString, atdo.OrigemString));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DESCRIÇÃO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Historico));
            pr.Inlines.Add(new LineBreak());

            flow.Blocks.Add(pr);

            return flow;
        }
        #endregion
    }
}
