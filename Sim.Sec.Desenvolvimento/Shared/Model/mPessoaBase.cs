using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public abstract class mPessoaBase : NotifyProperty
    {
        #region Declaratios
        private string _logradouro = string.Empty;
        private string _numero = string.Empty;
        private string _coplemento = string.Empty;
        private string _cep = string.Empty;
        private string _bairro = string.Empty;
        private string _municipio = string.Empty;
        private string _uf = string.Empty;
        private string _email = string.Empty;
        private string _telefones = string.Empty;
        private DateTime _cadastro;
        private DateTime _atualizado;
        private bool _ativo;
        #endregion

        #region Properties
        public string Logradouro
        {
            get { return _logradouro; }
            set
            {
                _logradouro = value.Trim();
                RaisePropertyChanged("Logradouro");
            }
        }
        public string Numero
        {
            get { return _numero; }
            set
            {
                _numero = value.Trim();
                RaisePropertyChanged("Numero");
            }
        }
        public string Complemento
        {
            get { return _coplemento; }
            set
            {
                _coplemento = value.Trim();
                RaisePropertyChanged("Complemento");
            }
        }
        public string CEP
        {
            get { return _cep; }
            set
            {
                _cep = value.Trim();
                RaisePropertyChanged("CEP");
            }
        }
        public string Bairro
        {
            get { return _bairro; }
            set
            {
                _bairro = value.Trim();
                RaisePropertyChanged("Bairro");
            }
        }
        public string Municipio
        {
            get { return _municipio; }
            set
            {
                _municipio = value.Trim();
                RaisePropertyChanged("Municipio");
            }
        }
        public string UF
        {
            get { return _uf; }
            set
            {
                _uf = value.Trim();
                RaisePropertyChanged("UF");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value.Trim();
                RaisePropertyChanged("Email");
            }
        }
        public string Telefones
        {
            get { return _telefones; }
            set
            {
                _telefones = value.Trim();
                RaisePropertyChanged("Telefones");
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
    }
}
