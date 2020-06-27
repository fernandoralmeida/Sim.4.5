using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Legislacao.ViewModel
{
    using Model;
    using Sim.Mvvm.Commands;


    class vmConsultaC : vmConsulta
    {
        #region Declarations
        private mDataSearch mdatac = new mDataSearch();

        private DateTime _datai;
        private DateTime _dataf;

        private string _resumo = string.Empty;
        private string _autor = string.Empty;

        private int _gorigem;
        private int _gsituacao;
        private int _llc;
        private int _dec;

        private ICommand _commandserach_c;
        private ICommand _commandclear_c;
        #endregion

        #region Properties
        public List<mTiposGenericos> DECs
        {
            get { return new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Class_D_Only_Non_Blocked); }
        }

        public List<mTiposGenericos> LLCs
        {
            get { return new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Class_L_Only_Non_Blocked); }
        }

        public List<mTiposGenericos> Origem
        {
            get { return new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Origem_Only_Non_Blocked); }
        }

        public List<mTiposGenericos> Situacao
        {
            get { return new mListaTiposGenericos().GoList(SqlCommands.SqlCollections.Situacao_Only_Non_Blocked); }
        }

        public DateTime DataI
        {
            get { return _datai; }
            set
            {
                _datai = value;
                RaisePropertyChanged("DataI");
            }
        }

        public DateTime DataF
        {
            get { return _dataf; }
            set
            {
                _dataf = value;
                RaisePropertyChanged("DataF");
            }
        }

        public int LLC
        {
            get { return _llc; }
            set
            {
                _llc = value;
                RaisePropertyChanged("LLC");
            }
        }

        public int DEC
        {
            get { return _dec; }
            set
            {
                _dec = value;
                RaisePropertyChanged("DEC");
            }
        }

        public string Resumo
        {
            get { return _resumo; }
            set
            {
                _resumo = value;
                RaisePropertyChanged("Resumo");
            }
        }

        public string Autor
        {
            get { return _autor; }
            set
            {
                _autor = value;
                RaisePropertyChanged("Autor");
            }
        }

        public int GetOrigem
        {
            get { return _gorigem; }
            set
            {
                _gorigem = value;
                RaisePropertyChanged("GetOrigem");
            }
        }

        public int GetSituacao
        {
            get { return _gsituacao; }
            set
            {
                _gsituacao = value;
                RaisePropertyChanged("GetSituacao");
            }
        }

        #endregion

        #region Commands
        public ICommand CommandSearchC
        {
            get
            {
                if (_commandserach_c == null)
                    _commandserach_c = new DelegateCommand(ExecuteCommandSearchC, null);
                return _commandserach_c;
            }
        }

        private void ExecuteCommandSearchC(object obj)
        {
            AsyncListarLegislacaoC(ParametrosC());
        }

        public ICommand CommandClearC
        {
            get
            {
                if (_commandclear_c == null)
                    _commandclear_c = new DelegateCommand(ExecuteCommandClearC, null);

                return _commandclear_c;
            }
        }

        private void ExecuteCommandClearC(object obj)
        {
            DataI = new DateTime(2017, 1, 1);
            DataF = DateTime.Now;
            Leis = true;
            Decretos = true;
            Leis_Comp = true;
            Resumo = string.Empty;
            Autor = string.Empty;
            LLC = 0;
            DEC = 0;
            GetOrigem = 0;
            GetSituacao = 0;
            ViewFiltros = string.Empty;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            FlowDoc = null;
            ListaLegislativa.Clear();
        }
        #endregion

        #region Constructor
        public vmConsultaC()
        {
            Mvvm.Observers.GlobalNavigation.Pagina = "CONSULTA";
            DataI = new DateTime(2017, 1, 1);
            DataF = DateTime.Now;
            StartProgress = false;
            BlackBox = Visibility.Collapsed;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            Leis = true;
            Decretos = true;
            Leis_Comp = true;            
        }
        #endregion

        #region Methods

        #endregion

        #region Functions

        private List<object> ParametrosC()
        {

            List<object> _param = new List<object>() { };

            _param.Add(Leis);//0
            _param.Add(Leis_Comp);//1
            _param.Add(Decretos);//2
            _param.Add(DataI.ToShortDateString());//3
            _param.Add(DataF.ToShortDateString());//4
            if (Resumo == string.Empty) _param.Add("%"); else _param.Add(Resumo); //5
            if (GetSituacao == 0) _param.Add("%"); else _param.Add(GetSituacao.ToString()); //6
            if (GetOrigem == 0) _param.Add("%"); else _param.Add(GetOrigem.ToString()); //7
            if (Autor == string.Empty) _param.Add("%"); else _param.Add(Autor); //8
            if (LLC == 0) _param.Add("%"); else _param.Add(LLC.ToString()); //9
            if (DEC == 0) _param.Add("%"); else _param.Add(DEC.ToString()); //10

            return _param;
        }

        private void AsyncListarLegislacaoC(List<object> sqlcommand)
        {
            BlackBox = Visibility.Visible;
            MainBox = Visibility.Visible;
            PrintBox = Visibility.Collapsed;
            StartProgress = true;

            string _leis = Leis ? "LEI " : "";
            string _leis_c = Leis_Comp ? "LEI COMPLEMENTAR " : "";
            string _dec = Decretos ? "DECRETO " : "";

            Filtros.Clear();

            Filtros.Add("[TIPO: " + _leis + _leis_c + _dec + "]");
            Filtros.Add("[PERIODOS: " + DataI.ToShortDateString() + " - " + DataI.ToShortDateString() + "]");
            Filtros.Add("[CLASSIFICAÇÃO: LLC " + LLCs[LLC].Nome.ToUpper() + " E DEC " + DECs[DEC].Nome.ToUpper() + "]");
            Filtros.Add("[RESUMO: " + Resumo + "]");
            Filtros.Add("[SITUAÇÃO: " + Situacao[GetSituacao].Nome.ToUpper() + "]");
            Filtros.Add("[ORIGEM: " + Origem[GetOrigem].Nome.ToUpper() + "]");
            Filtros.Add("[AUTOR: " + Autor + "]");

            StringBuilder sb = new StringBuilder();
            foreach (string filtro in Filtros)
                sb.Append(filtro);

            ViewFiltros = sb.ToString();

            Task.Factory.StartNew(() => mdatac.Legislacao_C(sqlcommand))
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        ListaLegislativa = task.Result;
                        BlackBox = Visibility.Collapsed;
                        StartProgress = false;
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

        #endregion
    }
}
