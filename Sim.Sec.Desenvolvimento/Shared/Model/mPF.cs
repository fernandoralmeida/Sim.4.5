using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mPF : mPessoaBase
    {
        #region Declarations
        private int _indice;
        private string _nome = string.Empty;
        private string _rg = string.Empty;
        private string _cpf = string.Empty;
        private DateTime _datanascimento;
        private int _sexo;
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }

        public string Nome { get { return _nome; } set { _nome = value.Trim(); RaisePropertyChanged("Nome"); } }

        public string RG { get { return _rg; } set { _rg = value.Trim(); RaisePropertyChanged("RG"); } }

        public string CPF { get { return _cpf; } set { _cpf = value.Trim(); RaisePropertyChanged("CPF"); } }

        public DateTime DataNascimento { get { return _datanascimento; } set { _datanascimento = value; RaisePropertyChanged("DataNascimento"); } }

        public int Sexo { get { return _sexo; } set { _sexo = value; RaisePropertyChanged("Sexo"); } }
        #endregion

        #region Function
        public void Clear()
        {
            Indice = 0;
            Nome = string.Empty;
            RG = string.Empty;
            CPF = string.Empty;
            DataNascimento = DateTime.Now;
            Sexo = 0;
            CEP = string.Empty;
            Logradouro = string.Empty;
            Numero = string.Empty;
            Complemento = string.Empty;
            Bairro = string.Empty;
            CEP = string.Empty;
            Municipio = string.Empty;
            UF = string.Empty;
            Email = string.Empty;
            Telefones = string.Empty;
            Cadastro = DateTime.Now;
            Atualizado = DateTime.Now;
            Ativo = true;
        }
        #endregion
    }
}
