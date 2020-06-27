using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;

    public class mPerfil : NotifyProperty
    {
        #region Declarations

        private int _indice;
        private string _cpf;
        private int _perfil;
        private bool _negocio;
        private bool _ativo;
        private string _perfilstring;

        #endregion

        #region Properties

        public int Indice { get { return _indice; } set { _indice = value; RaisePropertyChanged("Indice"); } }
        public string CPF { get { return _cpf; } set { _cpf = value; RaisePropertyChanged("CPF"); } }
        /// <summary>
        /// Perfil cliente onde:
        /// 1 = Potencial Empreendedor
        /// 2 = Empreendedor
        /// 3 = Profissional Liberal
        /// 4 = Outros
        /// </summary>
        public int Perfil { get { return _perfil; } set { _perfil = value; RaisePropertyChanged("Perfil"); } }
        /// <summary>
        /// Tem Negócio
        /// Sim / Não
        /// </summary>
        public bool Negocio { get { return _negocio; } set { _negocio = value; RaisePropertyChanged("Negocio"); } }
        /// <summary>
        /// Registro Ativo
        /// Sim / Não
        /// </summary>
        public bool Ativo { get { return _ativo; } set { _ativo = value; RaisePropertyChanged("Ativo"); } }

        public string PerfilString { get { return _perfilstring; } set { _perfilstring = value; RaisePropertyChanged("PerfilString"); } }
        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            CPF = string.Empty;
            Perfil = 0;
            Negocio = false;
            Ativo = true;
        }

        #endregion
    }
}
