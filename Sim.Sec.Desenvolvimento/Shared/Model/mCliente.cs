using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Desenvolvimento.Shared.Model
{
    using Mvvm.Observers;

    public class mCliente : NotifyProperty
    {
        #region Declarations

        private string _inscricao = string.Empty;
        private string _nomerazao = string.Empty;
        private string _telefones = string.Empty;
        private string _email = string.Empty;
        private string _situacao = string.Empty;

        #endregion

        #region Properties
        public string Inscricao
        {
            get
            {
                return _inscricao;
            }
            set
            {
                _inscricao = value.Trim();
                RaisePropertyChanged("Inscricao");
            }
        }

        public string NomeRazao
        {
            get
            {
                return _nomerazao;
            }
            set
            {
                _nomerazao = value;
                RaisePropertyChanged("NomeRazao");
            }
        }

        public string Telefones
        {
            get
            {
                return _telefones;
            }
            set
            {
                _telefones = value;
                RaisePropertyChanged("Telefones");
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Situacao
        {
            get { return _situacao; }
            set { _situacao = value; RaisePropertyChanged("Situacao"); }
        }

        #endregion

        #region Functions

        public void Clear()
        {
            Inscricao = string.Empty;
            NomeRazao = string.Empty;
            Telefones = string.Empty;
            Email = string.Empty;
            Situacao = string.Empty;
        }

        #endregion
    }
}
