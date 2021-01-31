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

        private Repositorio.RDIA DataDIA = new Repositorio.RDIA();

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
                return new ObservableCollection<string>() { "DIA", "MÊS", "ANO" };
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
                    D_I_A.Auxiliar.Tel = string.Empty;
                    D_I_A.Auxiliar.CPF = string.Empty;
                }
                else
                {
                    AuxiliarSimNao = "NÃO TEM AUXILIAR";
                    D_I_A.Auxiliar.Nome = "-";
                    D_I_A.Auxiliar.RG = "-";
                    D_I_A.Auxiliar.CPF = "-";
                    D_I_A.Auxiliar.Tel = "-";
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
            D_I_A.Autorizacao = Autorizacao2020();
            AsyncMostrarDados(AreaTransferencia.CPF);
            Exits_Dia(AreaTransferencia.CPF);
        }
        #endregion

        #region Functions

        private void SomaDatas()
        {
            if (Unidade_Tempo == "DIA")
                D_I_A.Validade = Convert.ToDateTime( D_I_A.Emissao.AddDays(Unidade).ToShortDateString());

            if (Unidade_Tempo == "MÊS")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddMonths(Unidade).ToShortDateString());

            if (Unidade_Tempo == "ANO")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddYears(Unidade).ToShortDateString());
        }

        private string Autorizacao()
        {
            var t = string.Empty;

            Task.Run(() => t = new Repositorio.RDIA().UltimaAutorizacao()).Wait();

            if (t == null || t == string.Empty)
                t = string.Format("{0}00000", DateTime.Now.Year);
            else
                t = new mMascaras().Remove(t);
            
            ulong n = Convert.ToUInt64(t);

            n++;

            string res = n.ToString();

            for (int i = 0; i < 4; i++)
                res = res.Remove(0, 1);

            string r = string.Format("{0}{1}", DateTime.Now.Year, res);

            return Convert.ToUInt64(r).ToString(@"000\.000\.0\-00");
        }

        private string Autorizacao2020()
        {
            var t = string.Empty;

            Task.Run(() => t = new Repositorio.RDIA().UltimaAutorizacao()).Wait();

            if (t == null || t == string.Empty)
                t = string.Format("{0}00000", 2020);
            else
                t = new mMascaras().Remove(t);
            

            ulong n = Convert.ToUInt64(t);

            n++;
           
            string res = n.ToString();
                       
            for (int i = 0; i < 4; i++)
                res = res.Remove(0, 1);

            string r = string.Format("{0}{1}", 2020, res);

            return Convert.ToUInt64(r).ToString(@"000\.000\.0\-00");
        }
        
        /// <summary>
        /// Verifica se o numero do CPF ja tem um D.I.A ativo.
        /// </summary>
        /// <param name="_cpf"></param>
        private async void Exits_Dia(string _cpf)
        {
            // retorna um objeto DIA caso haja um cadastrado, [condição: precisa estar ativo ou vencido, se estiver baixado, retorno é nulo]
            var t = Task<Model.DIA>.Factory.StartNew(() => new Repositorio.RDIA().DIA_Existe(_cpf));
            await t;

            if(t.IsCompleted)
            {
                if (_cpf == t.Result.Titular.CPF)
                {
                    AreaTransferencia.DIA_OK = false;
                    AreaTransferencia.DIA_Cancel_Service = true;
                    AsyncMessageBox("Ambulante já tem D.I.A ativo no momento", DialogBoxColor.Red, true); 
                    //System.Windows.MessageBox.Show("Ambulante já tem D.I.A ativo no momento!", "Sim.Alerta!");              

                    //ns.GoBack();
                }

                               
            }
        }

        private async void AsyncMostrarDados(string _cca)
        {
            var t = Task<Ambulante>.Run(() => new Repositorio.RAmbulante().GetAmbulante(_cca));

            await t;
            if (t.IsCompleted)
            {
                try
                {

                    if (t.Result != null)
                    {

                        if (t.Result.Titular.Nome == string.Empty)
                        {
                            AreaTransferencia.DIA_Cancel_Service = true;
                            AsyncMessageBox("Ambulante não cadastrado!", DialogBoxColor.Red, true);                            
                            AreaTransferencia.DIA_OK = false;                           
                        }

                        //Ambulante = task.Result;
                        D_I_A.Titular = t.Result.Titular;                      


                        if (t.Result.Auxiliar != null)
                            D_I_A.Auxiliar = t.Result.Auxiliar;

                        if (D_I_A.Atividade == string.Empty || D_I_A.Atividade == null)
                            D_I_A.Atividade = t.Result.Atividade;

                        D_I_A.FormaAtuacao = t.Result.FormaAtuacao;
                    }
                }
                catch (Exception ex)
                {  }
            }
        }

        private async void Gravar_DIA(DIA obj)
        {

            var t = Task<int>.Factory.StartNew(() => new Repositorio.RDIA().Gravar(obj));

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
