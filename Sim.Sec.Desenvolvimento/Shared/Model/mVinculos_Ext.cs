using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{

    using Mvvm.Observers;

    public class mVinculos_Ext : NotifyProperty
    {
        #region Declarations
        private int _indice = 0;
        private string _cnpj = string.Empty;
        private string _razaosocical = string.Empty;
        private string _telefones = string.Empty;
        private string _vinculoempresa = string.Empty;
        private int _vinculovalor = 0;
        private int _acao = 0; // valor -1 = REMOVE || valor 0 = ADICIONAR || valor 1 = ALTERAR

        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CNPJ { get { return _cnpj; } set { _cnpj = value; RaisePropertyChanged("CNPJ"); } }
        public string RazaoSocical { get { return _razaosocical; } set { _razaosocical = value; RaisePropertyChanged("RazaoSocical"); } }
        public string Telefones { get { return _telefones; } set { _telefones = value; RaisePropertyChanged("Telefones"); } }
        public string VinculoEmpresa { get { return _vinculoempresa; } set { _vinculoempresa = value; RaisePropertyChanged("VinculoEmpresa"); } }
        public int VinculoValor { get { return _vinculovalor; } set { _vinculovalor = value; RaisePropertyChanged("VinculoValor"); } }
        public int Acao { get { return _acao; } set { _acao = value; RaisePropertyChanged("Acao"); } }

        #endregion
    }
}
