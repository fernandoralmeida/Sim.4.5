using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{
    using Mvvm.Observers;
    using Shared.Model;

    public class mAmbulante : NotifyProperty
    {
        #region Declarations
        private int _indice = 0;
        private string _cadastro = string.Empty;
        private string _atendimento = string.Empty;
        private mCliente _pessoa = new mCliente();
        private mCliente _empresa = new mCliente();
        private string _atividades = string.Empty;
        private string _tipoinstalacoes = string.Empty;
        private string _periodotrabalho = string.Empty;
        private int _pessoasenvolvidas = 1;
        private string _local = string.Empty;
        private bool _entrepresentativa = false;
        private bool _querformalizar = false;
        private string _descricao = string.Empty;
        private int _tempoatividade = 0;
        private DateTime _datacadastro = DateTime.Now;
        private DateTime _dataalteracao = DateTime.Now;
        private int _situacao = 0;
        private string _justificativa = string.Empty;
        private bool _temcadastro = false;
        private bool _temlicenca = false;
        private DateTime _datalicenca = new DateTime(1, 1, 1);
        private bool _ativo = true;        
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public int PessoasEnvolvidas { get { return _pessoasenvolvidas; } set { _pessoasenvolvidas = value; RaisePropertyChanged("PessoasEnvolvidas"); } }
        public int Situacao { get { return _situacao; } set { _situacao = value;RaisePropertyChanged("Situacao"); } }
        public string Cadastro
        {
            get { return _cadastro; }
            set { _cadastro = value; RaisePropertyChanged("Cadastro"); }
        }
        public string Atendimento
        {
            get { return _atendimento; }
            set { _atendimento = value; RaisePropertyChanged("Atendimento"); }
        }
        public mCliente Pessoa
        {
            get { return _pessoa; }
            set { _pessoa = value; RaisePropertyChanged("Pessoa"); }
        }
        public mCliente Empresa
        {
            get { return _empresa; }
            set { _empresa = value; RaisePropertyChanged("Empresa"); }
        }
        public string Atividades
        {
            get { return _atividades; }
            set { _atividades = value; RaisePropertyChanged("Atividades"); }
        }
        public string TipoInstalacoes
        {
            get { return _tipoinstalacoes; }
            set { _tipoinstalacoes = value; RaisePropertyChanged("TipoInstalacoes"); }
        }
        public string PeridoTrabalho
        {
            get { return _periodotrabalho; }
            set { _periodotrabalho = value; RaisePropertyChanged("PeriodoTrabalho"); }
        }
        public string Local
        {
            get { return _local; }
            set { _local = value; RaisePropertyChanged("Local"); }
        }
        public bool EntRepresentativa
        {
            get { return _entrepresentativa; }
            set { _entrepresentativa = value; RaisePropertyChanged("EntRepresentativa"); }
        }
        public bool QuerFormalizar
        {
            get { return _querformalizar; }
            set { _querformalizar = value; RaisePropertyChanged("QuerFormalizar"); }
        }
        public string DescricaoNegocio
        {
            get { return _descricao; }
            set { _descricao = value; RaisePropertyChanged("DescricaoNegocio"); }
        }
        public int TempoAtividade
        {
            get { return _tempoatividade; }
            set { _tempoatividade = value; RaisePropertyChanged("TempoAtividade"); }
        }
        public DateTime DataCadastro
        {
            get { return _datacadastro; }
            set { _datacadastro = value; RaisePropertyChanged("DataCadastro"); }
        }
        public DateTime DataAlteracao
        {
            get { return _dataalteracao; }
            set { _dataalteracao = value; RaisePropertyChanged("DataAlteracao"); }
        }
        public string Justificativa { get { return _justificativa; } set { _justificativa = value; RaisePropertyChanged("Justificativa"); } }
        public bool TemCadastro { get { return _temcadastro; } set { _temcadastro = value; RaisePropertyChanged("TemCadastro"); } }
        public bool TemLicenca { get { return _temlicenca; } set { _temlicenca = value; RaisePropertyChanged("TemLicenca"); } }
        public DateTime DataLicenca { get { return _datalicenca; } set { _datalicenca = value; RaisePropertyChanged("DataLicenca"); } }

        public bool Ativo
        {
            get { return _ativo; }
            set
            {
                _ativo = value; RaisePropertyChanged("Ativo");
            }
        }
        public int Contador { get; set; }
        #endregion

        #region Functions
        public void Clear()
        {
            Indice = 0;
            PessoasEnvolvidas = 1;
            Cadastro = string.Empty;
            Atendimento = string.Empty;
            Pessoa.Clear();
            Empresa.Clear();
            Atividades = string.Empty;
            TipoInstalacoes = string.Empty;
            PeridoTrabalho = string.Empty;
            Local = string.Empty;
            EntRepresentativa = false;
            QuerFormalizar = false;
            DescricaoNegocio = string.Empty;
            TempoAtividade = 0;
            DataCadastro = DateTime.Now;
            DataAlteracao = DateTime.Now;
            Situacao = 0;
            Justificativa = string.Empty;
            TemCadastro = false;
            TemLicenca = false;
            DataLicenca = new DateTime(1, 1, 1);
            Ativo = true;
            Contador = 0;
        }
        #endregion
    }
}
