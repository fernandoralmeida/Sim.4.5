using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;

    public class mAtendimento : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _protocolo = string.Empty;
        private DateTime _data;
        private DateTime _hora;
        private mCliente _cliente = new mCliente();
        private int _tipo;
        private int _origem;
        private string _historico = string.Empty;
        private string _operador = string.Empty;
        private bool _ativo;
        private string _canal;

        string _tipotostring = string.Empty;
        string _origemtostring = string.Empty;
        string _atendimentosebrae = string.Empty;
        #endregion

        #region Properties
        public int Indice
        {
            get
            {
                return _indice;
            }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        public string Protocolo
        {
            get
            {
                return _protocolo;
            }
            set
            {
                _protocolo = value;
                RaisePropertyChanged("Protocolo");
            }
        }

        public DateTime Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        public DateTime Hora
        {
            get
            {
                return _hora;
            }
            set
            {
                _hora = value;
                RaisePropertyChanged("Hora");
            }
        }

        public mCliente Cliente
        {
            get
            {
                return _cliente;
            }
            set
            {
                _cliente = value;
                RaisePropertyChanged("Cliente");
            }
        }

        /// <summary>
        /// Tipos de Atendimento
        /// { 1 = Informação, 
        /// 2 = Orientação, 
        /// 3 = Inscrição,
        /// 4 = Agendamento,
        /// 5 = DAS,
        /// 6 = DASN,
        /// 7 = Formalização,
        /// 8 = Alteração,
        /// 9 = Encerramento,
        /// 10 = Viabilidade,
        /// 11 = Eventos}
        /// </summary>
        public int Tipo
        {
            get
            {
                return _tipo;
            }
            set
            {
                _tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }

        public int Origem
        {
            get
            {
                return _origem;
            }
            set
            {
                _origem = value;
                RaisePropertyChanged("Origem");
            }
        }

        public string Historico
        {
            get
            {
                return _historico;
            }
            set
            {
                _historico = value.Trim();
                RaisePropertyChanged("Historico");
            }
        }

        public string Operador
        {
            get
            {
                return _operador;
            }
            set
            {
                _operador = value;
                RaisePropertyChanged("Operador");
            }
        }

        public bool Ativo
        {
            get
            {
                return _ativo;
            }
            set
            {
                _ativo = value;
                RaisePropertyChanged("Ativo");
            }
        }
        /// <summary>
        /// Utilizado somente para preview consulta,
        /// mostrar Tipo e Origem em estring (nome);
        /// </summary>
        public string TipoString
        {
            get
            {
                return _tipotostring;
            }
            set
            {
                _tipotostring = value;
                RaisePropertyChanged("TipoString");
            }
        }

        public string OrigemString
        {
            get
            {
                return _origemtostring;
            }
            set
            {
                _origemtostring = value;
                RaisePropertyChanged("OrigemString");
            }
        }

        public string AtendimentoSebrae
        {
            get { return _atendimentosebrae; }
            set { _atendimentosebrae = value.Trim();
                RaisePropertyChanged("AtendimentoSebrae");
            }
        }

        public string Canal
        {
            get { return _canal; }
            set { _canal = value; RaisePropertyChanged("Canal"); }
        }

        public int Contador { get; set; }
        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            Protocolo = string.Empty;
            Data = DateTime.Now;
            Cliente.Clear();
            Tipo = 0;
            Origem = 0;
            Historico = string.Empty;
            Operador = string.Empty;
            Ativo = true;

            TipoString = string.Empty;
            OrigemString = string.Empty;
            AtendimentoSebrae = string.Empty;
        }

        #endregion
    }
}
