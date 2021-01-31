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
    using Controls.ViewModels;

    public class vmDIA_Edit : VMBase
    {
        #region Declarations
        NavigationService ns;
        private DIA _dia = new DIA();

        private Repositorio.RDIA DataDIA = new Repositorio.RDIA();

        private ObservableCollection<string> _situacoes = new ObservableCollection<string>();

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
        public ObservableCollection<string> Situacoes
        {
            get { return _situacoes; }
            set { _situacoes = value; RaisePropertyChanged("Situacoes"); }
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
                    D_I_A.Auxiliar.CPF = string.Empty;
                    D_I_A.Auxiliar.Nome = string.Empty;
                    D_I_A.Auxiliar.Tel = string.Empty;
                    D_I_A.Auxiliar.RG = string.Empty;
                }
                else
                {
                    AuxiliarSimNao = "NÃO TEM AUXILIAR";
                    D_I_A.Auxiliar.CPF = "-";
                    D_I_A.Auxiliar.Nome = "-";
                    D_I_A.Auxiliar.Tel = "-";
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
                    ValidadeSimNao = "TEM VALIDADE"; ;
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
        public ICommand CommandSave => new RelayCommand(p => {
            AreaTransferencia.Objeto = D_I_A;
            Gravar_DIA(D_I_A);
        });

        public ICommand CommandCancelar => new RelayCommand(p =>
        {
            ns.GoBack();

        });
        #endregion

        #region Construtor
        public vmDIA_Edit()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "D.I.A - EDIT";
            BlackBox = Visibility.Collapsed;
            ViewMessageBox = Visibility.Collapsed;
            Situacoes = new ObservableCollection<string>() { "", "ATIVO", "BAIXADO", "CANCELADO" };
            StartProgress = false;
            D_I_A.Emissao = DateTime.Now;
            AsyncMostrarDados(AreaTransferencia.Indice);
        }
        #endregion

        #region Functions

        private void SomaDatas()
        {
            if (Unidade_Tempo == "DIA")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddDays(Unidade).ToShortDateString());

            if (Unidade_Tempo == "MÊS")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddMonths(Unidade).ToShortDateString());

            if (Unidade_Tempo == "ANO")
                D_I_A.Validade = Convert.ToDateTime(D_I_A.Emissao.AddYears(Unidade).ToShortDateString());
        }

        private async void AsyncMostrarDados(int indice)
        {
            var t = Task<Model.DIA>.Run(() => new Repositorio.RDIA().GetDIA(indice));

            await t;

            if(t.IsCompleted)
            {
                try
                {
                    if (t.Result != null)
                    {
                        D_I_A = t.Result;

                        DateTime d = Convert.ToDateTime(D_I_A.Validade);

                        var dif = d.Date - D_I_A.Emissao.Date;

                        //dif.TotalDays;
                        var _dias = dif.TotalDays;
                        var _meses = dif.TotalDays / 30;
                        var _anos = dif.TotalDays / 365;

                        //System.Windows.MessageBox.Show("dia: " + _dias + "\n" + "mes: " + _meses + "\n" + "ano: " +_anos);

                        if (_dias < 31 && _meses < 1 && _anos < 1)
                        {
                            Unidade = Convert.ToInt32(_dias);
                            Unidade_Tempo = "DIA";
                        }
                        else if(_dias > 30 && _meses <= (12 * 2))
                        { 
                            Unidade = Convert.ToInt32(_meses);
                            Unidade_Tempo = "MÊS";
                        }
                        else if (_meses > (12*2) && _anos > ((365 * 2)/365))
                        {
                            Unidade = Convert.ToInt32(_anos);
                            Unidade_Tempo = "ANO";
                        }

                        if (D_I_A.Auxiliar.Nome == "-")
                            Expand_Auxiliar = false;

                        if (D_I_A.Veiculo.Modelo == "-")
                            Expand_Veiculo = false;

                        if (D_I_A.Validade == new DateTime(2001, 1, 1))
                            Expand_Validade = false;
                        else
                            Expand_Validade = true;

                        if (D_I_A.Validade < DateTime.Now)
                        {
                            Situacoes.Add("VENCIDO");
                            D_I_A.Situacao = "VENCIDO";
                        }
                    }
                }
                catch (Exception ex)
                {
                    AsyncMessageBox("Erro '" + ex.Message + "' inesperado, informe o Suporte!", DialogBoxColor.Orange, false);
                }
            }
        }

        private async void Gravar_DIA(DIA obj)
        {

            var t = Task<int>.Factory.StartNew(() => new Repositorio.RDIA().Alterar(obj));

            await t;

            try
            {
                if (t.Result > 0)
                {
                    AreaTransferencia.Numero_DIA = D_I_A.Autorizacao;
                    AreaTransferencia.DIA_OK = true;
                    AreaTransferencia.Preview_DIA = true;
                    AsyncMessageBox("D.I.A Alterado!", DialogBoxColor.Green, true);
                }
                else
                    AsyncMessageBox("Dados inválidos!", DialogBoxColor.Red, false);

            }
            catch (Exception ex)
            {
                AsyncMessageBox("Erro inesperado!\n" + ex.Message, DialogBoxColor.Red, false);
            }
        }
        #endregion
    }
}
