using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.NomesPublicos.Model
{
    using Mvvm.Observers;

    class mObjeto : NotifyProperty
    {

        #region Declaration
        private int _indice;
        private string _tipo = string.Empty;
        private string _nome = string.Empty;
        private string _bairro = string.Empty;
        private string _cep = string.Empty;
        private string _inicio = string.Empty;
        private string _nome_anterior = string.Empty;
        private string _origem = string.Empty;
        private string _numero_origem = string.Empty;
        private string _ano_origem = string.Empty;        
        private int _especie;
        private string _obs = string.Empty;
        private DateTime _cadastro;
        private DateTime _atualizado;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        public string Tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value;
                RaisePropertyChanged("Tipo");
            }
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                RaisePropertyChanged("Nome");
            }
        }

        public string Bairro
        {
            get { return _bairro; }
            set
            {
                _bairro = value;
                RaisePropertyChanged("Bairro");
            }
        }

        public string CEP
        {
            get { return _cep; }
            set
            {
                _cep = value;
                RaisePropertyChanged("CEP");
            }
        }

        public string PontoInicial
        {
            get { return _inicio; }
            set
            {
                _inicio = value;
                RaisePropertyChanged("PontoInicial");
            }
        }

        public string NomeAnterior
        {
            get { return _nome_anterior; }
            set
            {
                _nome_anterior = value;
                RaisePropertyChanged("NomeAnterior");
            }
        }
        
        public string Origem
        {
            get { return _origem; }
            set
            {
                _origem = value;
                RaisePropertyChanged("Origem");
            }
        }
        
        public string NumeroOrigem
        {
            get { return _numero_origem; }
            set
            {
                _numero_origem = value;
                RaisePropertyChanged("NumeroOrigem");
            }
        }

        public string AnoOrigem
        {
            get { return _ano_origem; }
            set
            {
                _ano_origem = value;
                RaisePropertyChanged("AnoOrigem");
            }
        }

        public int Especie
        {
            get { return _especie; }
            set
            {
                _especie = value;
                RaisePropertyChanged("Especie");
            }
        }

        public string Observacoes
        {
            get { return _obs; }
            set
            {
                _obs = value;
                RaisePropertyChanged("Observacoes");
            }
        }

        public DateTime Cadastro
        {
            get { return _cadastro; }
            set
            {
                _cadastro = value;
                RaisePropertyChanged("Cadastro");
            }
        }

        public DateTime Atualizado
        {
            get { return _atualizado; }
            set
            {
                _atualizado = value;
                RaisePropertyChanged("Atualizado");
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
        #endregion

        #region Functions
        public void Clear()
        {
            Ativo = true;
            Indice = 0;
            Especie = 0;
            Tipo = string.Empty;
            Nome = string.Empty;
            Bairro = string.Empty;
            CEP = string.Empty;
            PontoInicial = string.Empty;
            NomeAnterior = string.Empty;
            Origem = string.Empty;
            NumeroOrigem = string.Empty;
            AnoOrigem = string.Empty;
            Observacoes = string.Empty;
            Cadastro = DateTime.Now;
            Atualizado = DateTime.Now;
        }
        #endregion

    }
}
