using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;
    public class mAgendamento : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _agendamento;
        private string _atendimento;
        private int _tipo;
        private int _setor;
        private int _produto;
        private string _nome;
        private string _cpf;
        private string _cnpj;
        private string _phone;
        private string _mail;
        private DateTime _data;
        private DateTime _agendado;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string Agendamento { get { return _agendamento; } set { _agendamento = value;RaisePropertyChanged("Agendamento"); } }
        public string Atendimento { get { return _atendimento; } set { _atendimento = value; RaisePropertyChanged("Atendimento"); } }
        public int Tipo { get { return _tipo; } set { _tipo = value; RaisePropertyChanged("Tipo"); } }
        public int Local { get { return _setor; } set { _setor = value; RaisePropertyChanged("Local"); } }
        public int Produto { get { return _produto; } set { _produto = value; RaisePropertyChanged("Produto"); } }
        public string Nome { get { return _nome; } set { _nome = value; RaisePropertyChanged("Nome"); } }
        public string CPF { get { return _cpf; } set { _cpf = value; RaisePropertyChanged("CPF"); } }
        public string CNPJ { get { return _cnpj; } set { _cnpj = value; RaisePropertyChanged("CNPJ"); } }
        public string Telefone { get { return _phone; } set { _phone = value; RaisePropertyChanged("Telefone"); } }
        public string Email { get { return _mail; } set { _mail = value; RaisePropertyChanged("Email"); } }
        public DateTime Data { get { return _data; } set { _data = value; RaisePropertyChanged("Data"); } }
        public DateTime Agendado { get { return _agendado; } set { _agendado = value; RaisePropertyChanged("Agendado"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Agendamento = string.Empty;
            Atendimento = string.Empty;
            Tipo = 0;
            Local = 0;
            Produto = 0;
            Nome = string.Empty;
            CPF = string.Empty;
            CNPJ = string.Empty;
            Telefone = string.Empty;
            Email = string.Empty;
            Data = DateTime.Now;
            Agendado = DateTime.Now;
            Ativo = false;
        }
        #endregion
    }
}
