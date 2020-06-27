using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public class mVinculos : NotifyProperty
    {

        #region Declarations

        private int _indice;
        private string _cnpj;
        private string _cpf;
        private int _vinculo;
        private string _vinculoestring;
        private DateTime _data;
        private bool _ativo;
        private int _acao;
        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CNPJ { get { return _cnpj; } set { _cnpj = value; RaisePropertyChanged("CNPJ"); } }
        public string CPF { get { return _cpf; } set { _cpf = value; RaisePropertyChanged("CPF"); } }
        public int Vinculo { get { return _vinculo; } set { _vinculo = value; RaisePropertyChanged("Vinculo"); } }
        public string VinculoString { get { return _vinculoestring; } set { _vinculoestring = value; RaisePropertyChanged("VinculoString"); } }
        public DateTime Data { get { return _data; } set { _data = value; RaisePropertyChanged("Data"); } }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }

        /// <summary>
        /// Propriedade nao gravavel, Valor -1 = REMOVER, Valor 0 = MANTER, Valor 1 = ADICIONAR
        /// Usado para informar qual comando deve-se adotar
        /// </summary>
        public int Acao { get { return _acao; } set { _acao = value; RaisePropertyChanged("Acao"); } }

        #endregion

        #region Functions

        /// <summary>
        /// Limpa Controle
        /// </summary>
        public void Clear()
        {
            Indice = 0;
            CNPJ = string.Empty;
            CPF = string.Empty;
            Vinculo = 0;
            VinculoString = string.Empty;
            Data = DateTime.Now;
            Ativo = true;
            Acao = 0;            
        }

        #endregion

    }
}
