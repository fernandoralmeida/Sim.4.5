using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Account
{
    using Model;
    using Mvvm.Observers;
        
    #region Enums
    public enum AccountAccess
    {
        Bloqueado = 0,
        Normal,
        Administrador,
        Master
    }

    public enum SubModuloAccess
    {
        Bloqueado = 0,
        Consulta,
        Operador,
        Administrador
    }

    public enum Modulo
    {
        Governo = 1,
        Desenvolvimento = 2
    }

    public enum SubModulo
    {
        Legislacao = 1,
        Portarias = 2,
        Contratos = 3,
        Denominacoes = 4,
        SalaEmpreendedor = 5,
        SebraeAqui = 6,
        ComercioAmbulante = 7
    }
    #endregion

    public class Logged : GlobalNotifyProperty
    {
        private static bool _autenticado;
        private static int _acessso;
        private static string _conta;
        private static List<mModulos> _modulos = new List<mModulos>();
        private static List<mSubModulos> _submodulos = new List<mSubModulos>();
        private static mRegistroAcesso _reg = new mRegistroAcesso();
        //
        public static int Indice { get; set; }
        public static string Identificador { get; set; }
        public static string Nome { get; set; }
        public static string Sexo { get; set; }
        public static string Email { get; set; }
        public static DateTime Cadastro { get; set; }
        public static DateTime Atualizado { get; set; }
        public static bool Ativo { get; set; }
        //
        public static string Thema { get; set; }
        public static string Color { get; set; }
        //
        public static string Conta { get { return _conta; } set { _conta = value; } }
        public static int Acesso { get { return _acessso; } set { _acessso = value; } }
        public static List<mModulos> Modulos { get { return _modulos; } set { _modulos = value; } }
        public static List<mSubModulos> Submodulos { get { return _submodulos; } set { _submodulos = value; } }
        //
        public static mRegistroAcesso Registro
        {
            get { return _reg; }
            set
            {
                _reg = value;
            }
        }

        public static bool Autenticado
        {
            get { return _autenticado; }
            set
            {
                _autenticado = value;
                OnGlobalPropertyChanged("Autenticado");
            }
        }

        #region Function
        public void LogOut()
        {
            if (Identificador.ToLower() != "System".ToLower())
                new mData().LogOFF(Registro.Indice);

            Identificador = string.Empty;
            Nome = string.Empty;
            Sexo = string.Empty;
            Email = string.Empty;
            Cadastro = DateTime.Now;
            Atualizado = DateTime.Now;
            Ativo = false;
            Thema = "Light";
            Color = "#FF3399FF";
            Conta = AccountAccess.Bloqueado.ToString().ToUpper();
            Acesso = (int)AccountAccess.Bloqueado;
            Modulos.Clear();
            Submodulos.Clear();
            Autenticado = false;
        }
        #endregion
    }
}
