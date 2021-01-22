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

            D_I_A.Situacao = "ATIVO";            
            Gravar_DIA(D_I_A);
            
            ns.GoBack();

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
            D_I_A.Autorizacao = Autorizacao();
            AsyncMostrarDados(AreaTransferencia.CPF);
        }
        #endregion

        #region Functions
        private void Looping()
        {
            int i = 0;

            while(i < 99)
            {
                i++;
            }
        }     

        private int Contador()
        {
            int v = 0;

            var t = Task<int>.Run(() => { for (int i = 0; i < 10; i++) v = i; });

            t.Wait();

            return v;
        }

        private string Autorizacao()
        {
            var t = string.Empty;

            Task.Run(() => t = new Repositorio.DIA().UltimaAutorizacao()).Wait();

            if (t == null || t == string.Empty)
                t = string.Format("{0}0000", DateTime.Now.Year);

            t = new mMascaras().Remove(t);

            MessageBox.Show(t.ToString());

            ulong n = Convert.ToUInt64(t);

            n++;

            string res = n.ToString();
                       
            for (int i = 0; i < 4; i++)
                res = res.Remove(0, 1);

            string r = string.Format("{0}{1}",DateTime.Now.Year, res);

            MessageBox.Show(r.ToString());

            return Convert.ToUInt64(r).ToString(@"000\.000\.0\-0");
        }
        
        private string NovaAutorização()
        {
            int ano = DateTime.Now.Year;
            int d = 0; // 0 to 9
            int c = 0; // 0 to 9
            int m = 0;  // 0 to 9
            int k = 0;  // 0 to 9

            var t = string.Empty;

            Task.Run(() => t = new Repositorio.DIA().UltimaAutorizacao()).Wait();
            
            if (t.Count() > 10)
            {
                string[] _d = t.ToString().Split('-');

                string _rest = _d[0].Remove(0, 5);

                string[] _rest_split = _rest.ToString().Split('.');

                d = Convert.ToInt32(_d[1]); // 0 to 9
                c = Convert.ToInt32(_rest_split[1]); ; // 0 to 9
                m = Convert.ToInt32(_rest_split[0].Remove(0, 1));  // 0 to 9
                k = Convert.ToInt32(_rest_split[0].Remove(1, 1));  // 0 to 9
            }

            d++;

            if (d > 9)
            {
                d = 0;
                c++;

                if(c > 9)
                {
                    c = 0;
                    m++;

                    if (m > 9)
                    {
                        d = 0;
                        k++;
                    }
                }

            }

            string _ano = ano.ToString();
            _ano = _ano.Insert(3,".");

            return string.Format("{0}{1}{2}.{3}-{4}", _ano, k, m, c, d);
        }

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

                                if (D_I_A.Atividade == string.Empty || D_I_A.Atividade == null)
                                    D_I_A.Atividade = task.Result.DescricaoNegocio;

                                D_I_A.FormaAtuacao = task.Result.TipoInstalacoes;
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

        private async void Gravar_DIA(DIA obj)
        {
            var t = Task<int>.Factory.StartNew(()=> new Repositorio.DIA().Gravar(obj));

            await t;                
        }
        #endregion

    }
}
