using System;

namespace Sim.Sec.Desenvolvimento.ComercioAmbulante.Model
{

    using Mvvm.Observers;
    using Shared.Model;

    public class DIA: NotifyProperty
    {

        private int _indice = 0;
        public int Indice
        {
            get { return _indice; }
            set { _indice = value; RaisePropertyChanged("Indice"); }
        }

        private int _inscricaomunicipal;
        public int InscricaoMunicipal
        {
            get { return _inscricaomunicipal; }
            set { _inscricaomunicipal = value; RaisePropertyChanged("InscricaoMunicipal"); }
        }

        private string _autorizacao;
        public string Autorizacao
        {
            get { return _autorizacao; }
            set { _autorizacao = value; RaisePropertyChanged("Autorizacao"); }
        }

        private Autorizados _titular = new Autorizados();
        public Autorizados Titular
        {
            get { return _titular; }
            set { _titular = value; RaisePropertyChanged("Titular"); }
        }

        private Autorizados _auxiliar = new Autorizados();
        public Autorizados Auxiliar
        {
            get { return _auxiliar; }
            set { _auxiliar = value; RaisePropertyChanged("Auxiliar"); }
        }

        private string _atividade;
        public string Atividade
        {
            get { return _atividade; }
            set { _atividade = value;RaisePropertyChanged("Atividade"); }
        }

        private string _atuacao;
        public string FormaAtuacao
        {
            get { return _atuacao; }
            set { _atuacao = value;RaisePropertyChanged("FormaAtuacao"); }
        }

        private Veiculo _veiculo = new Veiculo();
        public Veiculo Veiculo
        {
            get { return _veiculo; }
            set { _veiculo = value;RaisePropertyChanged("Veiculo"); }
        }

        private DateTime _emissao;
        public DateTime Emissao
        {
            get { return _emissao; }
            set { _emissao = value;RaisePropertyChanged("Emissao"); }
        }

        private DateTime? _validade;
        public DateTime? Validade
        {
            get { return _validade; }
            set { _validade = value;RaisePropertyChanged("Validade"); }
        }

        private string _processo;
        public string Processo
        {
            get { return _processo; }
            set { _processo = value;RaisePropertyChanged("Processo"); }
        }

        private string _situacao;
        public string Situacao
        {
            get { return _situacao; }
            set { _situacao = value;RaisePropertyChanged("Situacao"); }
        }

        private int _contador;
        public int Contador
        {
            get { return _contador; }
            set { _contador = value; RaisePropertyChanged(Contador.ToString()); }
        }

    }

    public class Autorizados : NotifyProperty
    {
        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set { _nome = value; RaisePropertyChanged("Nome"); }
        }

        private string _rg;
        public string RG
        {
            get { return _rg; }
            set { _rg = value; RaisePropertyChanged("RG"); }
        }

    }

    public class Veiculo : NotifyProperty
    {
        private string _modelo;
        public string Modelo
        {
            get { return _modelo; }
            set { _modelo = value;RaisePropertyChanged("Modelo"); }
        }

        private string _placa;
        public string Placa
        {
            get { return _placa; }
            set { _placa = value; RaisePropertyChanged("Placa"); }
        }

        private string _cor;
        public string Cor
        {
            get { return _cor; }
            set { _cor = value; RaisePropertyChanged("Cor"); }
        }
    }
}
