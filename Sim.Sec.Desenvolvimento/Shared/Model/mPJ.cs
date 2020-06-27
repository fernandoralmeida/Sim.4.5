using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    public class mPJ : mPessoaBase
    {
        #region Declarations
        private int _indice;
        private string _cnpj;
        private string _matrizfilial;
        private DateTime _abertura = DateTime.Now;
        private string _razaosocial;
        private string _nomefantasia;
        private string _naturezajuridica;
        private string _atividadeprincipal;
        private string _atividadesecundaria;
        private string _situacaocadastral;
        private DateTime _datasituacaocadastral = DateTime.Now;
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CNPJ { get { return _cnpj; } set { _cnpj = value.Trim(); RaisePropertyChanged("CNPJ"); } }
        public string MatrizFilial { get { return _matrizfilial; } set { _matrizfilial = value.Trim(); RaisePropertyChanged("MatrizFilial"); } }
        public DateTime Abertura { get { return _abertura; } set { _abertura = value; RaisePropertyChanged("Abertura"); } }
        public string RazaoSocial { get { return _razaosocial; } set { _razaosocial = value.Trim(); RaisePropertyChanged("RazaoSocial"); } }
        public string NomeFantasia { get { return _nomefantasia; } set { _nomefantasia = value.Trim(); RaisePropertyChanged("NomeFantasia"); } }
        public string NaturezaJuridica { get { return _naturezajuridica; } set { _naturezajuridica = value.Trim(); RaisePropertyChanged("NaturezaJuridica"); } }
        public string AtividadePrincipal { get { return _atividadeprincipal; } set { _atividadeprincipal = value.Trim(); RaisePropertyChanged("AtividadePrincipal"); } }
        public string AtividadeSecundaria { get { return _atividadesecundaria; } set { _atividadesecundaria = value.Trim(); RaisePropertyChanged("AtividadeSecundaria"); } }
        public string SituacaoCadastral { get { return _situacaocadastral; } set { _situacaocadastral = value.Trim(); RaisePropertyChanged("SituacaoCadastral"); } }
        public DateTime DataSituacaoCadastral { get { return _datasituacaocadastral; } set { _datasituacaocadastral = value; RaisePropertyChanged("DataSituacaoCadastral"); } }
        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            CNPJ = string.Empty;
            MatrizFilial = string.Empty;
            Abertura = DateTime.Now;
            RazaoSocial = string.Empty;
            NomeFantasia = string.Empty;
            NaturezaJuridica = string.Empty;
            AtividadePrincipal = string.Empty;
            AtividadeSecundaria = string.Empty;
            SituacaoCadastral = string.Empty;
            DataSituacaoCadastral = DateTime.Now;
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
