using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sim.Sec.Desenvolvimento.Shared.ViewModel.Viabilidade
{
    using Model;
    using Mvvm.Commands;
    using Mvvm.Observers;
    using Account;

    class vmMain : NotifyProperty
    {
        #region Declarations
        private mData mdata = new mData();
        private ObservableCollection<mViabilidade> _listaat = new ObservableCollection<mViabilidade>();

        private FlowDocument _flowdoc = new FlowDocument();

        private DateTime _datai = DateTime.Now;
        private Visibility _blackbox;
        private Visibility _previewbox;
        private Visibility _veratualizando;

        private string _preview = string.Empty;
        private string _protocolo = string.Empty;
        private string _motivo = string.Empty;

        private bool _starprogress;
        private bool _minhasviabilidades;

        private int _selectedparecer;
        private int _selectedrow;

        private ICommand _commandclosepreview;
        private ICommand _commandpreviewbox;
        private ICommand _commandpreviewbox2;
        private ICommand _commandatsebrae;
        private ICommand _commandrefreshdate;
        private ICommand _commandatualizarparecer;
        private ICommand _commandpreviewparacer;
        private ICommand _commandretornocliente;
        private ICommand _commandsemretorno;
        private ICommand _commandatualizarviabilidades;
        #endregion

        #region Properties
        public FlowDocument FlowDoc
        {
            get { return _flowdoc; }
            set
            {
                _flowdoc = value;
                RaisePropertyChanged("FlowDoc");
            }
        }
        /*
        public ObservableCollection<mTiposGenericos> Situacoes
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_SE_Viabilidade_Situacao WHERE (Ativo = True) ORDER BY Valor"); }
        }
        */
        public ObservableCollection<mViabilidade> ListarViabilidades
        {
            get { return _listaat; }
            set
            {
                _listaat = value;
                RaisePropertyChanged("ListarViabilidades");
            }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set
            {
                _datai = value;
                CommandRefreshDate.Execute(null);
                RaisePropertyChanged("DataI");
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

        public Visibility PreviewBox
        {
            get { return _previewbox; }
            set
            {
                _previewbox = value;
                RaisePropertyChanged("PreviewBox");
            }
        }

        public Visibility PreviewBoxInterna
        {
            get { return _veratualizando; }
            set
            {
                _veratualizando = value;
                RaisePropertyChanged("PreviewBoxInterna");
            }
        }

        public bool MinhasViabilidades
        {
            get { return _minhasviabilidades; }
            set
            {
                _minhasviabilidades = value;
                //AsyncViabilidades(Parametros(GlobalNavigation.Parametro));
                RaisePropertyChanged("MinhasViabilidades");
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

        public int SelectedParecer
        {
            get { return _selectedparecer; }
            set
            {
                _selectedparecer = value;

                RaisePropertyChanged("SelectedParecer");
            }
        }

        public string Protocolo
        {
            get { return _protocolo; }
            set
            {
                _protocolo = value;
                RaisePropertyChanged("Protocolo");
            }
        }

        public string Motivo
        {
            get { return _motivo; }
            set
            {
                _motivo = value;
                RaisePropertyChanged("Motivo");
            }
        }
        #endregion

        #region Commands
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
            PreviewBoxInterna = Visibility.Collapsed;
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
                ApresentarDados(obj);
                PreviewBox = Visibility.Visible;
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
                FlowDoc = FlowViabilidade((string)obj);
                PreviewBox = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }
        public ICommand CommandAtSebrae
        {
            get
            {
                if (_commandatsebrae == null)
                    _commandatsebrae = new DelegateCommand(ExecCommandAtSebrae, null);
                return _commandatsebrae;
            }
        }

        private void ExecCommandAtSebrae(object obj)
        {
            //GravarAtendimentoSebrae();
        }

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
            if (GlobalNavigation.Parametro != string.Empty)
                AsyncViabilidades(Parametros(GlobalNavigation.Parametro));
        }

        public ICommand CommandAtualizarParecer
        {
            get
            {
                if (_commandatualizarparecer == null)
                    _commandatualizarparecer = new DelegateCommand(ExecCommandAtualizarParecer, null);
                return _commandatualizarparecer;
            }
        }

        private void ExecCommandAtualizarParecer(object obj)
        {
            try
            {
                if (mdata.AtualizarViabilidade(obj))
                {
                    MessageBox.Show("Viabilidade Atualizada!", "Sim.Alerta!");

                    AsyncViabilidades(Parametros(GlobalNavigation.Parametro));

                    PreviewBox = Visibility.Collapsed;
                    BlackBox = Visibility.Collapsed;
                    PreviewBoxInterna = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand CommandRetornoCliente
        {
            get
            {
                if (_commandretornocliente == null)
                    _commandretornocliente = new DelegateCommand(ExecCommandRetornoCliente, null);
                return _commandretornocliente;
            }
        }

        private void ExecCommandRetornoCliente(object obj)
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    if (MessageBox.Show("Confirmar aviso ao requerente?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (mdata.AtualizarViabilidadeClienteRetorno((string)obj, true, false))
                            AsyncViabilidades(Parametros(GlobalNavigation.Parametro));
                    }
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                if (MessageBox.Show("Confirmar aviso ao requerente?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                {
                                    if (mdata.AtualizarViabilidadeClienteRetorno((string)obj, true, false))
                                        AsyncViabilidades(Parametros(GlobalNavigation.Parametro));
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand CommandSemRetorno
        {
            get
            {
                if (_commandsemretorno == null)
                    _commandsemretorno = new DelegateCommand(ExecCommandSemRetorno, null);
                return _commandsemretorno;
            }
        }

        private void ExecCommandSemRetorno(object obj)
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    if (MessageBox.Show("Confirmar não retorno ao requerente?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (mdata.AtualizarViabilidadeClienteRetorno((string)obj, false, true))
                            AsyncViabilidades(Parametros(GlobalNavigation.Parametro));
                    }
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                if (MessageBox.Show("Confirmar não retorno ao requerente?", "Sim.Alerta!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                {
                                    if (mdata.AtualizarViabilidadeClienteRetorno((string)obj, false, true))
                                        AsyncViabilidades(Parametros(GlobalNavigation.Parametro));
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ICommand CommandPreviewParecer
        {
            get
            {
                if (_commandpreviewparacer == null)
                    _commandpreviewparacer = new DelegateCommand(ExecCommandPreviewParecer, null);
                return _commandpreviewparacer;
            }
        }

        private void ExecCommandPreviewParecer(object obj)
        {
            try
            {

                if (Logged.Acesso == (int)AccountAccess.Master)
                {
                    FlowDoc = FlowViabilidade((string)obj);
                    PreviewBox = Visibility.Visible;
                    PreviewBoxInterna = Visibility.Visible;
                }
                else
                {
                    foreach (Account.Model.mSubModulos m in Logged.Submodulos)
                    {
                        if (m.SubModulo == (int)SubModulo.SalaEmpreendedor || m.SubModulo == (int)SubModulo.SebraeAqui)
                        {
                            if (m.Acesso > (int)SubModuloAccess.Consulta)
                            {
                                FlowDoc = FlowViabilidade((string)obj);
                                PreviewBox = Visibility.Visible;
                                PreviewBoxInterna = Visibility.Visible;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sim.Alerta!");
            }
        }

        public ICommand CommandAtualizarViabilidades
        {
            get
            {
                if (_commandatualizarviabilidades == null)
                    _commandatualizarviabilidades = new RelayCommand(p => {

                        AsyncViabilidades(Parametros(GlobalNavigation.Parametro));

                    });

                return _commandatualizarviabilidades;
            }
        }
        #endregion

        #region Constructor
        public vmMain()
        {
            GlobalNavigation.Pagina = string.Empty;
            BlackBox = Visibility.Collapsed;
            PreviewBox = Visibility.Collapsed;
            PreviewBoxInterna = Visibility.Collapsed;
            StartProgress = false;
            AreaTransferencia.Limpar();            
            MinhasViabilidades = false;
            AsyncViabilidades(Parametros(""));
        }
        #endregion

        #region Functions

        private List<string> Parametros(string param)
        {

            List<string> _param = new List<string>() { };

            _param.Add("5");

            if (MinhasViabilidades == true)
                _param.Add(Logged.Identificador);
            else
                _param.Add("%");

            _param.Add(param);

            return _param;
        }

        private async void AsyncViabilidades(List<string> sqlcommand)
        {
            await Task.Run(() =>
            {
                ListarViabilidades = mdata.ViabilidadesNow(sqlcommand);
            });
        }

        private async void ApresentarDados(object cliente)
        {

            string _cliente = new mMascaras().Remove((string)cliente);

            await Task.Run(() =>
            {
                FlowDoc.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate ()
                {

                    switch (_cliente.Length)
                    {
                        case 11:
                            FlowDoc = FlowPF(_cliente);
                            break;

                        case 14:
                            FlowDoc = FlowPJ(_cliente);
                            break;
                    }
                }));
            });
        }

        private FlowDocument FlowPF(string cliente)
        {

            mPF_Ext pessoafisica = new mPF_Ext();
            pessoafisica = new mData().ExistPessoaFisica(cliente);

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

            foreach (mVinculos v in pessoafisica.ColecaoVinculos)
            {
                pr.Inlines.Add(new Run("EMPRESAS:") { FontSize = 10, Foreground = Brushes.Gray });
                pr.Inlines.Add(new LineBreak());
                pr.Inlines.Add(new Run(v.VinculoString + " DO CNPJ: " + new mMascaras().CNPJ(v.CNPJ)));
            }

            flow.Blocks.Add(pr);


            return flow;
        }

        private FlowDocument FlowPJ(string cliente)
        {

            mPJ_Ext pessoajuridica = new mPJ_Ext();

            pessoajuridica = new mData().ExistPessoaJuridica(cliente);

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

            flow.Blocks.Add(pr);

            return flow;

        }

        private FlowDocument FlowViabilidade(string protocolo)
        {
            var atdo = new mViabilidade();

            atdo = new mData().Viabilidade(protocolo);

            Protocolo = atdo.Protocolo;
            SelectedParecer = atdo.Perecer;
            Motivo = atdo.Motivo;

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
            pr.Inlines.Add(new Run("VIABILIDADE:") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Protocolo, atdo.Data.ToShortDateString()));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CLIENTE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.Requerente.NomeRazao));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("CONTATO") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.Requerente.Telefones, atdo.Requerente.Email));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("DADOS DA VIABILIDADE") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0}", atdo.TextoEmail));
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(new Run("PARECER") { FontSize = 10, Foreground = Brushes.Gray });
            pr.Inlines.Add(new LineBreak());
            pr.Inlines.Add(string.Format("{0} {1}", atdo.PerecerString, atdo.Motivo));
            pr.Inlines.Add(new LineBreak());

            flow.Blocks.Add(pr);

            return flow;
        }
        #endregion
    }
}
