using System;
using System.Linq;
using System.Collections.Generic;
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
    using Controls.ViewModels;

    public class vmD_I_A : VMBase
    {

        #region Declarations
        NavigationService ns;
        private DIA _dia = new DIA();

        private Repositorio.DIA DataDIA = new Repositorio.DIA();

        private bool _expander_veiculo;
        private bool _expander_auxiliar;
        private bool _expander_validade;

        private string _veiculosimnao;
        private string _auxiliarsimnao;
        private string _validadesimnao;

        private int _unidade;
        private string _unidade_tempo;
        #endregion

        #region Properties

        public DIA D_I_A
        {
            get { return _dia; }
            set { _dia = value; RaisePropertyChanged("D_I_A"); }
        }

        public int Unidade
        {
            get { return _unidade; }
            set
            {
                _unidade = value;
                SomaDatas();
                RaisePropertyChanged("Unidade");
            }
        }

        public ObservableCollection<int> Unidades
        {
            get
            {
                var lst = new ObservableCollection<int>();
                for (int i = 1; i <= 30; i++)
                    lst.Add(i);

                return lst;
            }
        }

        public string Unidade_Tempo
        {
            get { return _unidade_tempo; }
            set
            {
                _unidade_tempo = value;
                SomaDatas();
                RaisePropertyChanged("Unidade_Tempo");
            }
        }

        public ObservableCollection<string> Unidades_Tempo
        {
            get
            {
                return new ObservableCollection<string>() { "DIA(S)", "MÊS(ES)", "ANO(S)" };
            }
        }

        public string AuxiliarSimNao
        {
            get { return _auxiliarsimnao; }
            set { _auxiliarsimnao = value; RaisePropertyChanged("AuxiliarSimNao"); }
        }

        public bool Expand_Auxiliar
        {
            get { return _expander_auxiliar; }
            set
            {
                _expander_auxiliar = value;

                if (value == true)
                {
                    AuxiliarSimNao = "TEM AUXILIAR";
                    D_I_A.Auxiliar.Nome = string.Empty;
                    D_I_A.Auxiliar.RG = string.Empty;
                }
                else
                {
                    AuxiliarSimNao = "NÃO TEM AUXILIAR";
                    D_I_A.Auxiliar.Nome = "-";
                    D_I_A.Auxiliar.RG = "-";
                }

                RaisePropertyChanged("Expand_Auxiliar");
            }
        }

        public string ValidadeSimNao
        {
            get { return _validadesimnao; }
            set { _validadesimnao = value; RaisePropertyChanged("ValidadeSimNao"); }
        }

        public bool Expand_Validade
        {
            get { return _expander_validade; }
            set
            {
                _expander_validade = value;

                if (value == true)
                {
                    ValidadeSimNao = "TEM VALIDADE";;
                }
                else
                {
                    ValidadeSimNao = "NÃO TEM VALIDADE";
                    D_I_A.Validade = new DateTime(2001, 1, 1);
                }

                RaisePropertyChanged("Expand_Validade");
            }
        }

        public string VeiculoSimNao
        {
            get { return _veiculosimnao; }
            set { _veiculosimnao = value; RaisePropertyChanged("VeiculoSimNao"); }
        }

        public bool Expand_Veiculo
        {
            get { return _expander_veiculo; }
            set
            {
                _expander_veiculo = value;

                if (value == true)
                {
                    VeiculoSimNao = "TEM VEICULO";
                    D_I_A.Veiculo.Modelo = string.Empty;
                    D_I_A.Veiculo.Placa = string.Empty;
                    D_I_A.Veiculo.Cor = string.Empty;
                }
                else
                {
                    VeiculoSimNao = "NÃO TEM VEICULO";
                    D_I_A.Veiculo.Modelo = "-";
                    D_I_A.Veiculo.Placa = "-";
                    D_I_A.Veiculo.Cor = "-";
                }

                RaisePropertyChanged("Expand_Veiculo");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandSave => new RelayCommand( p=> {
            D_I_A.Situacao = "ATIVO";
            AreaTransferencia.Objeto = D_I_A; 
            Gravar_DIA(D_I_A);
        });

        public ICommand CommandCancelar => new RelayCommand(p =>
        {
            ns.GoBack();

        });
        #endregion

        #region Construtor
        public vmD_I_A()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "D.I.A";
            BlackBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;
            StartProgress = false;
            Expand_Auxiliar = true;
            Expand_Veiculo = true;
            Expand_Validade = true;
            D_I_A.Emissao = DateTime.Now;
            D_I_A.Autorizacao = Autorizacao();
            AsyncMostrarDados(AreaTransferencia.CPF);
        }
        #endregion

        #region Functions

        private void SomaDatas()
        {
            if (Unidade_Tempo == "DIA(S)")
                D_I_A.Validade = Convert.ToDateTime( D_I_A.Emissao.AddDays(Unidade).ToShortDateString());

            if (Unidade_Tempo == "MÊS(ES)")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddMonths(Unidade).ToShortDateString());

            if (Unidade_Tempo == "ANO(S)")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddYears(Unidade).ToShortDateString());
        }

        private string Autorizacao()
        {
            var t = string.Empty;

            Task.Run(() => t = new Repositorio.DIA().UltimaAutorizacao()).Wait();

            if (t == null || t == string.Empty)
                t = string.Format("{0}0000", DateTime.Now.Year);

            t = new mMascaras().Remove(t);

            ulong n = Convert.ToUInt64(t);

            n++;

            string res = n.ToString();
                       
            for (int i = 0; i < 4; i++)
                res = res.Remove(0, 1);

            string r = string.Format("{0}{1}",DateTime.Now.Year, res);

            return Convert.ToUInt64(r).ToString(@"000\.000\.0\-0");
        }

        private void AsyncMostrarDados(string _cca)
        {
            Task<mAmbulante>.Factory.StartNew(() => new mDataCM().GetCAmbulante(_cca))
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

            var t = Task<int>.Factory.StartNew(() => new Repositorio.DIA().Gravar(obj));

            await t;

            try
            {
                if (t.Result > 0)
                {
                    AreaTransferencia.Numero_DIA = D_I_A.Autorizacao;
                    AreaTransferencia.DIA_OK = true;
                    AreaTransferencia.Preview_DIA = true;
                    AsyncMessageBox("D.I.A Gerado!", DialogBoxColor.Green, true);
                }
                else
                    AsyncMessageBox("Dados inválidos!", DialogBoxColor.Red, false);

            }
            catch(Exception ex)
            {
                AsyncMessageBox("Erro inesperado!\n" + ex.Message, DialogBoxColor.Red, false);
            }                
        }
        #endregion

    }
}
