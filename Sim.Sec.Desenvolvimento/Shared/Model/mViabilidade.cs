using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;
    public class mViabilidade : NotifyProperty
    {
        #region Declarations

        private int _indice;
        private string _viabilidade;
        private mCliente _requerente = new mCliente();
        private string _logradouro = string.Empty;
        private string _numero = string.Empty;
        private string _coplemento = string.Empty;
        private string _cep = string.Empty;
        private string _bairro = string.Empty;
        private string _municipio = string.Empty;
        private string _uf = string.Empty;
        private string _ctm = string.Empty;
        private string _atividades = string.Empty;
        private DateTime _data;
        private DateTime _dataresultado;
        private int _situacao;
        private string _situacaostring;
        private string _textoemail = string.Empty;
        private bool _sendmail;
        private string _motivo = string.Empty;
        private string _operador = string.Empty;
        private bool _retornocliente;
        private DateTime _dataretorno;
        private bool _semretorno;
        private bool _ativo;

        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string Protocolo { get { return _viabilidade; } set { _viabilidade = value; RaisePropertyChanged("Protocolo"); } }
        public mCliente Requerente { get { return _requerente; } set { _requerente = value; RaisePropertyChanged("Requerente"); } }
        public string Logradouro { get { return _logradouro; } set { _logradouro = value;RaisePropertyChanged("Logradouro"); } }
        public string Numero { get { return _numero; } set { _numero = value; RaisePropertyChanged("Numero"); } }
        public string Complemento { get { return _coplemento; } set { _coplemento = value; RaisePropertyChanged("Complemento"); } }
        public string CEP { get { return _cep; } set { _cep = value; RaisePropertyChanged("CEP"); } }
        public string Bairro { get { return _bairro; } set { _bairro = value; RaisePropertyChanged("Bairro"); } }
        public string Municipio { get { return _municipio; } set { _municipio = value; RaisePropertyChanged("Municipio"); } }
        public string UF { get { return _uf; } set { _uf = value; RaisePropertyChanged("UF"); } }
        public string CTM { get { return _ctm; } set { _ctm = value; RaisePropertyChanged("CTM"); } }
        public string Atividades { get { return _atividades; } set { _atividades = value; RaisePropertyChanged("Atividades"); } }
        public string TextoEmail { get { return _textoemail; } set { _textoemail = value; RaisePropertyChanged("TextoEmail"); } }
        public bool SendMail { get { return _sendmail; } set { _sendmail = value; RaisePropertyChanged("SendMail"); } }
        public DateTime Data { get { return _data; } set { _data = value; RaisePropertyChanged("Data"); } }
        public DateTime DataParecer { get { return _dataresultado; } set { _dataresultado = value; RaisePropertyChanged("DataParecer"); } }
        public int Perecer { get { return _situacao; } set { _situacao = value; RaisePropertyChanged("Perecer"); } }
        public string PerecerString { get { return _situacaostring; } set { _situacaostring = value;RaisePropertyChanged("PerecerString"); } }
        public string Motivo { get { return _motivo; } set { _motivo = value; RaisePropertyChanged("Motivo"); } }
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
        public bool RetornoCliente
        {
            get { return _retornocliente; }
            set
            {
                _retornocliente = value;
                RaisePropertyChanged("RetornoCliente");
            }
        }
        public DateTime DataRetorno
        {
            get { return _dataretorno; }
            set
            {
                _dataretorno = value;
                RaisePropertyChanged("DataRetorno");
            }
        }
        public bool SemRetorno
        {
            get { return _semretorno; }
            set
            {
                _semretorno = value;
                RaisePropertyChanged("SemRetorno");
            }
        }
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }
        public int Contador { get; set; }
        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            Protocolo = string.Empty;
            Requerente.Clear();
            Logradouro = string.Empty;
            Numero = string.Empty;
            Complemento = string.Empty;
            CEP = string.Empty;
            Bairro = string.Empty;
            Municipio = string.Empty;
            UF = string.Empty;
            CTM = string.Empty;
            Atividades = string.Empty;
            Data = DateTime.Now;
            DataParecer = DateTime.Now;
            DataRetorno = DateTime.Now;
            RetornoCliente = false;
            Perecer = 0;
            TextoEmail = string.Empty;
            SendMail = false;
            Motivo = string.Empty;
            Operador = string.Empty;
            SemRetorno = false;
            Ativo = true;
        }

        #endregion
    }
}
