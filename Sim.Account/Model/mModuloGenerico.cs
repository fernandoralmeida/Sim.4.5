using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Account.Model
{

    using Mvvm.Observers;

    class mModuloGenerico : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _identificador;
        private string _nomemodulo;
        private int _valormodulo;
        private int _valoracesso;
        private string _acessonome;
        private bool _acessomodulo;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }

        public string Identificador
        {
            get { return _identificador; }
            set
            {
                _identificador = value;
                RaisePropertyChanged("Identificador");
            }
        }

        public string NomeModulo
        {
            get { return _nomemodulo; }
            set
            {
                _nomemodulo = value;
                RaisePropertyChanged("NomeModulo");
            }
        }

        public int ValorModulo
        {
            get { return _valormodulo; }
            set
            {
                _valormodulo = value;
                RaisePropertyChanged("ValorModulo");
            }
        }

        public int ValorAcesso
        {
            get { return _valoracesso; }
            set
            {
                _valoracesso = value;
                RaisePropertyChanged("ValorAcesso");
            }
        }

        public bool AcessoModulo
        {
            get { return _acessomodulo; }
            set
            {
                _acessomodulo = value;
                RaisePropertyChanged("AcessoModulo");
            }
        }

        public string AcessoNome
        {
            get { return _acessonome; }
            set
            {
                _acessonome = value;
                RaisePropertyChanged("AcessoNome");
            }
        }

        public bool Ativo
        {
            get { return _ativo; }
            set
            {
                _ativo = value;
                RaisePropertyChanged("Ativo");
            }
        }
        #endregion
    }
}
