using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{
    using Mvvm.Observers;
    public class Ambulante : NotifyProperty
    {
        #region Declarations
        private int _indice = 0;
        private string _cadastro = string.Empty;
        private Autorizados _titular = new Autorizados();
        private Autorizados _auxiliar = new Autorizados();
        private string _formaatuacao = string.Empty;
        private Veiculo _usaveiculo = new Veiculo();
        private string _local = string.Empty;
        private string _atividade = string.Empty;
        private DateTime _datacadastro = DateTime.Now;
        private DateTime _dataalteracao = DateTime.Now;
        private bool _ativo = true;
        #endregion

        #region Properties
        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string Cadastro
        {
            get { return _cadastro; }
            set { _cadastro = value; RaisePropertyChanged("Cadastro"); }
        }
        public Autorizados Titular
        {
            get { return _titular; }
            set { _titular = value; RaisePropertyChanged("Titular"); }
        }
        public Autorizados Auxiliar
        {
            get { return _auxiliar; }
            set { _auxiliar = value; RaisePropertyChanged("Auxiliar"); }
        }
        public string FormaAtuacao
        {
            get { return _formaatuacao; }
            set { _formaatuacao = value; RaisePropertyChanged("FormaAtuacao"); }
        }
        public Veiculo UsaVeiculo
        {
            get { return _usaveiculo; }
            set { _usaveiculo = value; RaisePropertyChanged("UsaVeiculo"); }
        }
        public string Local
        {
            get { return _local; }
            set { _local = value; RaisePropertyChanged("Local"); }
        }
        public string Atividade
        {
            get { return _atividade; }
            set { _atividade = value; RaisePropertyChanged("Atividade"); }
        }

        public DateTime DataCadastro
        {
            get { return _datacadastro; }
            set { _datacadastro = value; RaisePropertyChanged("DataCadastro"); }
        }
        public DateTime UltimaAlteracao
        {
            get { return _dataalteracao; }
            set { _dataalteracao = value; RaisePropertyChanged("UltimaAlteracao"); }
        }
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
    }
}
