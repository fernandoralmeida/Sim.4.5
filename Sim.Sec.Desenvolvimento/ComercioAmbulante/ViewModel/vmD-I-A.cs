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

    public class vmD_I_A : NotifyProperty
    {

        #region Declarations
        NavigationService ns;
        private DIA _dia = new DIA();

        private Repositorio.DIA DataDIA = new Repositorio.DIA();
        private mDataCM mdatacm = new mDataCM();

        private bool _starprogress;

        private Visibility _blackbox;
        private Visibility _temlicencaview;

        private ICommand _commandprint;
        private ICommand _commandcancelar;
        #endregion

        #region Properties

        public DIA D_I_A
        {
            get { return _dia; }
            set { _dia = value; RaisePropertyChanged("D_I_A"); }
        }

        public ObservableCollection<mTiposGenericos> Situações
        {
            get { return new mData().Tipos(@"SELECT * FROM SDT_CAmbulante_Situacao WHERE (Ativo = True) ORDER BY Valor"); }
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
        public ICommand CommandSave => new RelayCommand( p=> {

            Gravar_DIA(D_I_A).Wait();

            AreaTransferencia.Objeto = D_I_A;
            ns.Navigate(new Uri("/Sim.Sec.Desenvolvimento;component/ComercioAmbulante/View/PreviewDIA.xaml", UriKind.Relative));
        });
        

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
        #endregion

        #region Construtor
        public vmD_I_A()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "D.I.A";
            BlackBox = Visibility.Collapsed;
            StartProgress = false;
            D_I_A.Emissao = DateTime.Now;
            AsyncMostrarDados(AreaTransferencia.CPF);
        }
        #endregion

        #region Functions
        private void AsyncMostrarDados(string _cca)
        {
            Task<mAmbulante>.Factory.StartNew(() => mdatacm.GetCAmbulante(_cca))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        try
                        {
                            if (task.Result != null)
                            {
                                //Ambulante = task.Result;
                                D_I_A.Titular.Nome = task.Result.Pessoa.NomeRazao;
                                D_I_A.Atividade = task.Result.Atividades;
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

        private async int Gravar_DIA(DIA obj)
        {
            var t = Task<int>.Factory.StartNew(()=> new Repositorio.DIA().Gravar(obj));
            
            t.Wait();
        }
        #endregion

    }
}
