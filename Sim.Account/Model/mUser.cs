using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Account.Model
{
    using Mvvm.Observers;

    public class Log
    {
        /// <summary>
        /// Armazena o ID do IDBOX
        /// </summary>
        public static string ID { get; set; }

        /// <summary>
        /// Pega o Nome do Usuário no LOGIN
        /// </summary>
        public static string Nome { get; set; }
    }

    public class mUser : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private string _identificador;
        private string _nome;
        private string _sexo;
        private string _email;
        private DateTime _cadastro;
        private DateTime _atualizado;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set { _indice = value; RaisePropertyChanged("Indice"); }
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
        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                RaisePropertyChanged("Nome");
            }
        }
        public string Sexo
        {
            get { return _sexo; }
            set
            {
                _sexo = value;
                RaisePropertyChanged("Sexo");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        public DateTime Cadastro
        {
            get { return _cadastro; }
            set { _cadastro = value; RaisePropertyChanged("Cadastro"); }
        }
        public DateTime Atualizado
        {
            get { return _atualizado; }
            set
            {
                _atualizado = value;
                RaisePropertyChanged("Atualizado");
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

        #region Functions
        public void Clear()
        {
            Indice = 0;
            Identificador = string.Empty;
            Nome = string.Empty;
            Sexo = string.Empty;
            Email = string.Empty;
            Cadastro = new DateTime(1, 1, DateTime.Now.Year);
            Atualizado = new DateTime(1, 1, DateTime.Now.Year);
            Ativo = true;
        }
        #endregion
    }
}
