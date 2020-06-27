using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;
    public class mInscricao : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _inscricao;
        private string _atendimento;
        private string _evento;
        private string _nome;
        private string _cpf;
        private string _cnpj;
        private string _phone;
        private string _mail;
        private DateTime _data;
        private bool _presente;
        private bool _ativo;
        private int _contador;
        #endregion

        #region Properties
        public int Contador { get { return _contador; } set { _contador = value; RaisePropertyChanged("Contador"); } }
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string Inscricao { get { return _inscricao; } set { _inscricao = value; RaisePropertyChanged("Inscricao"); } }
        public string Evento { get { return _evento; } set { _evento = value; RaisePropertyChanged("Evento"); } }
        public string Atendimento { get { return _atendimento; } set { _atendimento = value; RaisePropertyChanged("Atendimento"); } }
        public string Nome { get { return _nome; } set { _nome = value; RaisePropertyChanged("Nome"); } }
        public string CPF { get { return _cpf; } set { _cpf = value; RaisePropertyChanged("CPF"); } }
        public string CNPJ { get { return _cnpj; } set { _cnpj = value; RaisePropertyChanged("CNPJ"); } }
        public string Telefone { get { return _phone; } set { _phone = value; RaisePropertyChanged("Telefone"); } }
        public string Email { get { return _mail; } set { _mail = value; RaisePropertyChanged("Email"); } }
        public DateTime Data { get { return _data; } set { _data = value; RaisePropertyChanged("Data"); } }
        public bool Presente { get { return _presente; } set { _presente = value; RaisePropertyChanged("Presente"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Inscricao = string.Empty;
            Atendimento = string.Empty;
            Evento = string.Empty;
            Nome = string.Empty;
            CPF = string.Empty;
            CNPJ = string.Empty;
            Telefone = string.Empty;
            Email = string.Empty;
            Data = DateTime.Now;
            Ativo = false;
            Presente = false;
            Contador = 0;
        }
        #endregion
    }
}
