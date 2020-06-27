using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Sim.Account.ViewModel
{
    using Controls.ViewModels;
    using Mvvm.Observers;
    using Mvvm.Commands;
    using Model;

    class vmAdd : VMBase
    {
        #region Declarations
        public NavigationService ns;
        private mUser _user = new mUser();
        private mOpcoes _opt = new mOpcoes();
        private mContas _conta = new mContas();
        private mModulos _modulo = new mModulos();
        private mSubModulos _submodulo = new mSubModulos();
        private List<mGenerico> _contas = new List<mGenerico>();
        private List<mGenerico> _modulos = new List<mGenerico>();
        private List<mGenerico> _submodulos = new List<mGenerico>();
        private List<mGenerico> _acessosubmodulo = new List<mGenerico>();
        private ObservableCollection<mModuloGenerico> _listamodulos = new ObservableCollection<mModuloGenerico>();
        private ObservableCollection<mModuloGenerico> _listasubmodulos = new ObservableCollection<mModuloGenerico>();
        private mGenero _generos = new mGenero();

        private SolidColorBrush _msgcolor;

        private int _selectedrow;

        private ICommand _commandaddmd;
        private ICommand _commandaddsmd;
        private ICommand _commandremovemodulo;
        private ICommand _commandremovesubmodulo;
        #endregion

        #region Properties

        public List<mGenero> Generos
        {
            get { return _generos.Generos(); }
        }

        public mUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        public mOpcoes Opcoes
        {
            get { return _opt; }
            set
            {
                _opt = value;
                RaisePropertyChanged("Opcoes");
            }
        }

        public mContas Conta
        {
            get { return _conta; }
            set
            {
                _conta = value;
                RaisePropertyChanged("Conta");
            }
        }

        public mModulos Modulo
        {
            get { return _modulo; }
            set
            {
                _modulo = value;
                RaisePropertyChanged("Modulo");
            }
        }

        public mSubModulos SubModulo
        {
            get { return _submodulo; }
            set
            {
                _submodulo = value;
                RaisePropertyChanged("SubModulo");
            }
        }

        public List<mGenerico> Contas
        {
            get { return new mData().TiposContasLista(); }
        }

        public List<mGenerico> Modulos
        {
            get { return new mData().TiposModulosLista(); }
        }

        public List<mGenerico> SubModulos
        {
            get { return new mData().TiposSubModulosLista(); }
        }

        public List<mGenerico> AcessoSubModulos
        {
            get { return new mData().AcessoSubModuloLista(); }
        }

        public ObservableCollection<mModuloGenerico> ListaModulos
        {
            get { return _listamodulos; }
            set
            {
                _listamodulos = value;
                RaisePropertyChanged("ListaModulos");
            }
        }

        public ObservableCollection<mModuloGenerico> ListaSubModulos
        {
            get { return _listasubmodulos; }
            set
            {
                _listasubmodulos = value;
                RaisePropertyChanged("ListaSubModulos");
            }
        }

        public SolidColorBrush MsgColor
        {
            get { return _msgcolor; }
            set
            {
                _msgcolor = value;
                RaisePropertyChanged("MsgColor");
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
        public ICommand CommandAddMD
        {
            get
            {
                if (_commandaddmd == null)
                    _commandaddmd = new RelayCommand(p => {

                        var findvalue = ListaModulos.Where(i => i.ValorModulo == Modulo.Modulo);

                        if (findvalue.ToList().Count == 0)
                        {
                            ListaModulos.Add(new mModuloGenerico()
                            {
                                Indice = 0,
                                Identificador = User.Identificador,
                                NomeModulo = Modulos[Modulo.Modulo - 1].Nome,
                                ValorModulo = Modulo.Modulo,
                                AcessoModulo = true,
                                AcessoNome = true.ToString(),
                                Ativo = true
                            });
                        }

                    });

                return _commandaddmd;
            }
        }

        public ICommand CommandAddSMD
        {
            get
            {
                if (_commandaddsmd == null)
                    _commandaddsmd = new RelayCommand(p => {

                        var findvalue = ListaSubModulos.Where(i => i.ValorModulo == SubModulo.SubModulo);

                        if (findvalue.ToList().Count == 0)
                            ListaSubModulos.Add(new mModuloGenerico()
                            {
                                Indice = 0,
                                Identificador = User.Identificador,
                                NomeModulo = SubModulos[SubModulo.SubModulo - 1].Nome,
                                ValorModulo = SubModulo.SubModulo,
                                ValorAcesso = SubModulo.Acesso,
                                AcessoNome = AcessoSubModulos[SubModulo.Acesso].Nome,
                                Ativo = true
                            });

                    });

                return _commandaddsmd;
            }
        }

        public ICommand CommandRemoveModulo
        {
            get
            {
                if (_commandremovemodulo == null)
                    _commandremovemodulo = new RelayCommand(p => {

                        try
                        {
                            ListaModulos.RemoveAt(SelectedRow);
                        }
                        catch
                        {
                            SelectedRow = 0;
                        }

                    });

                return _commandremovemodulo;
            }
        }

        public ICommand CommandRemoveSubModulo
        {
            get
            {
                if (_commandremovesubmodulo == null)
                    _commandremovesubmodulo = new RelayCommand(p => {

                        try
                        {
                            ListaSubModulos.RemoveAt(SelectedRow);
                        }
                        catch { SelectedRow = 0; }

                    });

                return _commandremovesubmodulo;
            }
        }

        public ICommand CommandGravar => new RelayCommand(async p =>
        {
            if(await AsyncSave())
            {
                AsyncMessageBox(string.Format("Conta {1} ID {0} criada com sucesso!", User.Identificador, User.Nome), DialogBoxColor.Green, true);
            }
            else
            {
                AsyncMessageBox(string.Format("Erro, não foi possível criar uma conta para {0}!", User.Nome), DialogBoxColor.Red, false);
            }
                
        });

        public ICommand CommandReturn => new RelayCommand(p =>
        {
            if (ns.CanGoBack)
                ns.GoBack();
        });
        #endregion

        #region Constructors
        public vmAdd()
        {
            ns = GlobalNavigation.NavService;
            ViewMessageBox = Visibility.Collapsed;
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
        }
        #endregion

        #region Functions

        private Task<bool> AsyncSave()
        {
            StartProgress = true;
            BlackBox = Visibility.Visible;

            return Task<bool>.Factory.StartNew(() =>
                {
                    try
                    {
                        if (Logged.Acesso > Conta.Conta || Logged.Registro.CodigoAcesso.Contains("system_"))
                        {
                            User.Cadastro = DateTime.Now;
                            User.Atualizado = DateTime.Now;
                            User.Ativo = true;

                            if (new mData().GravarUsuario(User))
                            {
                                Opcoes = new mOpcoes()
                                {
                                    Identificador = User.Identificador,
                                    Thema = "Light",
                                    Color = "#FF3399FF"
                                };

                                Conta = new mContas()
                                {
                                    Identificador = User.Identificador,
                                    Conta = Conta.Conta,
                                    Ativo = true,
                                };

                                foreach (mModuloGenerico md in ListaModulos)
                                {
                                    Modulo.Identificador = User.Identificador;
                                    Modulo.Modulo = md.ValorModulo;
                                    Modulo.Acesso = md.AcessoModulo;
                                    new mData().GravarModulos(Modulo);
                                }

                                foreach (mModuloGenerico smd in ListaSubModulos)
                                {
                                    SubModulo.Identificador = User.Identificador;
                                    SubModulo.SubModulo = smd.ValorModulo;
                                    SubModulo.Acesso = smd.ValorAcesso;
                                    new mData().GravarSubModulos(SubModulo);
                                }

                                new mData().GravarOpcoes(Opcoes);
                                new mData().GravarConta(Conta);

                                return true;
                            }
                            else
                                return false;
                        }
                        else
                        {
                            BlackBox = Visibility.Collapsed;
                            StartProgress = false;
                            SyncMessageBox(string.Format("Conta com nível de acesso inválido!\nSelecione uma opção inferior à [{0}].", Logged.Conta), DialogBoxColor.Orange);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {                        
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
                        SyncMessageBox(ex.Message, DialogBoxColor.Red);
                        return false;
                    }
                    finally
                    {
                        StartProgress = false;
                        BlackBox = Visibility.Collapsed;
                    }
                });

        }
        #endregion
               
    }
}
