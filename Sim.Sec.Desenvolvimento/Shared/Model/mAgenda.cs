using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;

    public class mAgenda : NotifyProperty
    {
        #region Declaration
        private int _indice;
        private string _codigo;
        private int _tipo;
        private string _tipostring;
        private string _evento;
        private int _vagas;
        private int _inscritos;
        private string _descricao;
        private DateTime _data;
        private DateTime _hora;
        private int _setor;
        private string _setorstring;
        private int _estado;
        private string _estadostring;
        private DateTime _criacao;
        private bool _ativo;
        private int _contador;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set { _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        public string Codigo
        {
            get { return _codigo; }
            set
            {
                _codigo = value.Trim();
                RaisePropertyChanged("Codigo");
            }
        }

        public int Tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }

        public string TipoString
        {
            get { return _tipostring; }
            set
            {
                _tipostring = value.ToUpper().Trim();
                RaisePropertyChanged("TipoString");
            }
        }

        public string Evento
        {
            get { return _evento; }
            set
            {
                _evento = value.Trim();
                RaisePropertyChanged("Evento");
            }
        }

        public int Vagas
        {
            get { return _vagas; }
            set
            {
                _vagas = value;                
                RaisePropertyChanged("Vagas");
            }
        }

        public int Inscritos
        {
            get { return _inscritos; }
            set { _inscritos = value;
                RaisePropertyChanged("Inscritos");
            }
        }

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                _descricao = value.Trim();
                RaisePropertyChanged("Descricao");
            }
        }

        public DateTime Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        public DateTime Hora
        {
            get { return _hora; }
            set
            {
                _hora = value;
                RaisePropertyChanged("Hora");
            }
        }

        public int Setor
        {
            get { return _setor; }
            set
            {
                _setor = value;
                RaisePropertyChanged("Setor");
            }
        }

        public string SetorString
        {
            get { return _setorstring; }
            set { _setorstring = value.Trim();
                RaisePropertyChanged("SetorString");
            }
        }

        public int Estado
        {
            get { return _estado; }
            set
            {
                _estado = value;
                RaisePropertyChanged("Estado");
            }
        }

        public string EstadoString
        {
            get { return _estadostring; }
            set
            {
                _estadostring = value.Trim();
                RaisePropertyChanged("EstadoString");
            }
        }

        public DateTime Criacao
        {
            get { return _criacao; }
            set
            {
                _criacao = value;
                RaisePropertyChanged("Criacao");
            }
        }

        public bool Ativo
        {
            get { return _ativo; }
            set
            {
                _ativo = value;
                RaisePropertyChanged("Ativo");
            }
        }

        public int Contador
        {
            get { return _contador; }
            set { _contador = value; RaisePropertyChanged("Contador"); }
        }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Codigo = string.Empty;
            Tipo = 0;
            TipoString = string.Empty;
            Evento = string.Empty;
            Vagas = 0;
            Inscritos = 0;
            Descricao = string.Empty;
            Data = DateTime.Now;
            Hora = DateTime.Now;
            Setor = 0;
            Estado = 0;
            Criacao = DateTime.Now;
            Ativo = true;
            Contador = 0;
        }
        #endregion
    }
}
