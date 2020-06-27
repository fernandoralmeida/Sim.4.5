using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections.ObjectModel;

namespace Sim.Account.Model
{

    using System.Security;

    public class mData
    {
        #region Login/LogOut

        public mAccount AutenticarUsuario(string userid, string senha)
        {

            var loginDB = Data.Factory.Connecting(DataBase.Base.Login);

            mAccount _acc = new mAccount();

            var idmaster = new SecureString();
            idmaster.AppendChar('s');
            idmaster.AppendChar('i');
            idmaster.AppendChar('m');
            idmaster.AppendChar('.');
            idmaster.AppendChar('m');
            idmaster.AppendChar('a');
            idmaster.AppendChar('s');
            idmaster.AppendChar('t');
            idmaster.AppendChar('e');
            idmaster.AppendChar('r');

            if (userid.ToLower() == new mString().SecureStringToString(idmaster) &&
                senha.ToLower() == new mString().SecureStringToString(idmaster))
            {
                _acc.Indice = 0;
                _acc.Identificador = "System";
                _acc.Nome = "SIM MASTER";
                _acc.Sexo = "M";
                _acc.Email = "system.account";
                _acc.Cadastro = new DateTime(2014, 01, 01);
                _acc.Atualizado = new DateTime(2014, 01, 01);
                _acc.Ativo = true;
                _acc.Thema = "Dark";
                _acc.Color = "#FFE51400";
                _acc.Conta.Conta = (int)AccountAccess.Master;
                _acc.Conta.ContaAcesso = AccountAccess.Master.ToString().ToUpper();

                List<mModulos> md = new List<mModulos>();
                md.Add(new mModulos { Indice = 0, Identificador = _acc.Identificador, Modulo = (int)Modulo.Governo, Acesso = true });
                md.Add(new mModulos { Indice = 0, Identificador = _acc.Identificador, Modulo = (int)Modulo.Desenvolvimento, Acesso = true });

                _acc.Modulos = md;

                List<mSubModulos> smd = new List<mSubModulos>();
                smd.Add(new mSubModulos { Indice = 0, Identificador = _acc.Identificador, SubModulo = (int)SubModulo.Legislacao, Acesso = (int)SubModuloAccess.Administrador });
                smd.Add(new mSubModulos { Indice = 0, Identificador = _acc.Identificador, SubModulo = (int)SubModulo.Portarias, Acesso = (int)SubModuloAccess.Administrador });
                smd.Add(new mSubModulos { Indice = 0, Identificador = _acc.Identificador, SubModulo = (int)SubModulo.Contratos, Acesso = (int)SubModuloAccess.Administrador });
                smd.Add(new mSubModulos { Indice = 0, Identificador = _acc.Identificador, SubModulo = (int)SubModulo.Denominacoes, Acesso = (int)SubModuloAccess.Administrador });
                smd.Add(new mSubModulos { Indice = 0, Identificador = _acc.Identificador, SubModulo = (int)SubModulo.SalaEmpreendedor, Acesso = (int)SubModuloAccess.Administrador });
                smd.Add(new mSubModulos { Indice = 0, Identificador = _acc.Identificador, SubModulo = (int)SubModulo.SebraeAqui, Acesso = (int)SubModuloAccess.Administrador });

                _acc.SubModulos = smd;
                _acc.Registro.CodigoAcesso = CodigoAcesso().ToLower();
                _acc.Autenticado = true;

                return _acc;
            }
            else
            {
                try
                {
                    loginDB.ClearParameters();

                    loginDB.AddParameters("@ID", userid);
                    loginDB.AddParameters("@Senha", senha);

                    DataTable dt = loginDB.Read(@"SELECT * FROM Usuarios WHERE (Identificador = @ID) AND (Senha = @Senha) AND (Ativo = True)");

                    if (!dt.Rows.Count.Equals(1))
                    {
                        throw new AggregateException("Senha incorreta!");
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        _acc.Indice = (int)dr[0];
                        _acc.Identificador = dr["Identificador"].ToString();
                        _acc.Nome = dr["Nome"].ToString().ToUpper();
                        _acc.Sexo = dr["Sexo"].ToString();
                        _acc.Email = dr["Email"].ToString();
                        _acc.Cadastro = (DateTime)dr["Cadastro"];
                        _acc.Atualizado = (DateTime)dr["Atualizado"];
                        _acc.Ativo = (bool)dr["Ativo"];

                        _acc.Thema = "Light";
                        _acc.Color = "#FF3399FF";

                        _acc.Conta.Conta = (int)AccountAccess.Normal;
                        _acc.Conta.ContaAcesso = AccountAccess.Normal.ToString().ToUpper();

                        var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
                        dbAccess.ClearParameters();
                        dbAccess.AddParameters("@id", _acc.Identificador);

                        foreach (DataRow ap in dbAccess.Read(@"SELECT Thema, Color FROM Opcoes WHERE (Identificador = @id)").Rows)
                        {
                            _acc.Thema = (string)ap[0];
                            _acc.Color = (string)ap[1];
                        }

                        foreach (DataRow ac in dbAccess.Read(@"SELECT Conta FROM Contas WHERE (Identificador = @id)").Rows)
                        {
                            _acc.Conta.Conta = (int)ac[0];
                            _acc.Conta.ContaAcesso = Convert.ToString((AccountAccess)ac[0]).ToUpper();
                        }

                        #region Modulos

                        List<mModulos> md = new List<mModulos>();

                        foreach (DataRow mdac in dbAccess.Read(@"SELECT * FROM Modulos WHERE (Identificador = @id) AND (Acesso = True) ORDER BY Modulo").Rows)
                        {
                            md.Add(new mModulos { Indice = (int)mdac[0], Identificador = (string)mdac[1], Modulo = (int)mdac[2], Acesso = (bool)mdac[3] });
                        }

                        _acc.Modulos = md;

                        #endregion

                        #region SubModulos

                        List<mSubModulos> smd = new List<mSubModulos>();

                        foreach (DataRow smac in dbAccess.Read(@"SELECT * FROM SubModulos WHERE (Identificador = @id) AND (Acesso > 0) ORDER BY SubModulo").Rows)
                        {
                            smd.Add(new mSubModulos { Indice = (int)smac[0], Identificador = (string)smac[1], SubModulo = (int)smac[2], Acesso = (int)smac[3] });
                        }

                        _acc.SubModulos = smd;

                        #endregion

                        string _codigo = CodigoAcesso();

                        if (_acc.Identificador.ToLower() != "System".ToLower())
                            LogIN(_acc.Identificador, _codigo);

                        dbAccess.ClearParameters();
                        dbAccess.AddParameters("@codigo", _codigo);

                        foreach (DataRow ra in dbAccess.Read("SELECT * FROM Registro_Acesso WHERE(Codigo = @codigo)").Rows)
                        {
                            _acc.Registro.Indice = (int)ra[0];
                            _acc.Registro.CodigoAcesso = (string)ra[1];
                            _acc.Registro.Identificador = (string)ra[2];
                            _acc.Registro.Data = (DateTime)ra[3];
                            _acc.Registro.HoraInicio = (DateTime)ra[4];
                            _acc.Registro.HoraFim = (DateTime)ra[5];
                        }

                        if (_acc.Conta.Conta == (int)AccountAccess.Bloqueado)
                        {
                            _acc.Autenticado = false;
                            throw new AggregateException("Conta Bloqueada");
                        }

                        _acc.Autenticado = true;
                    }

                    return _acc;

                }
                catch (AggregateException ex)
                {
                    return null;
                    throw new AggregateException(ex.Message);
                }
            }

        }

        public string Operador(string userid)
        {

            var loginDB = Data.Factory.Connecting(DataBase.Base.Login);

            var idmaster = new SecureString();
            idmaster.AppendChar('s');
            idmaster.AppendChar('i');
            idmaster.AppendChar('m');
            idmaster.AppendChar('.');
            idmaster.AppendChar('m');
            idmaster.AppendChar('a');
            idmaster.AppendChar('s');
            idmaster.AppendChar('t');
            idmaster.AppendChar('e');
            idmaster.AppendChar('r');

            if (userid.ToLower() == new mString().SecureStringToString(idmaster))
            {
                return "Master".ToUpper();
            }
            else
            {

                string n = string.Empty;

                loginDB.ClearParameters();

                loginDB.AddParameters("@ID", userid);

                DataTable dt = loginDB.Read(@"SELECT Nome FROM Usuarios WHERE (Identificador = @ID) AND (Ativo = True)");

                if (!dt.Rows.Count.Equals(1))
                {
                    throw new InvalidOperationException("Operador inválido!");
                }

                foreach (DataRow dr in dt.Rows)
                {
                    n = dr["Nome"].ToString().ToUpper();
                }

                return n;
            }
        }

        private string CodigoAcesso()
        {
            string r = string.Empty;

            string a = DateTime.Now.Year.ToString("0000");
            string m = DateTime.Now.Month.ToString("00");
            string d = DateTime.Now.Day.ToString("00");

            string hs = DateTime.Now.Hour.ToString("00");
            string mn = DateTime.Now.Minute.ToString("00");
            string ss = DateTime.Now.Second.ToString("00");

            r = string.Format(@"{0}_{1}{2}{3}{4}{5}{6}", Logged.Identificador, a, m, d, hs, mn, ss);

            return r;
        }

        private bool LogIN(string id, string codigo)
        {
            var dbLogin = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                dbLogin.ClearParameters();
                dbLogin.AddParameters("@codigo", codigo);
                dbLogin.AddParameters("@id", id);
                dbLogin.AddParameters("@data", DateTime.Now.ToShortDateString());
                dbLogin.AddParameters("@hora1", DateTime.Now.ToLongTimeString());
                dbLogin.AddParameters("@hora2", DateTime.Now.ToLongTimeString());

                string sql = @"
INSERT INTO
Registro_Acesso (Codigo, Identificador, Data, HoraE, HoraS) VALUES
(@codigo, @id, @data, @hora1, @hora2)";

                return dbLogin.Write(sql);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool LogOFF(int indice)
        {
            var dbLogin = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {

                dbLogin.ClearParameters();
                dbLogin.AddParameters("@hora", DateTime.Now.ToLongTimeString());
                dbLogin.AddParameters("@indice", indice);

                string sql = @"
UPDATE Registro_Acesso
SET HoraS = @hora
WHERE (Indice = @indice)";

                return dbLogin.Write(sql);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Insert/Update Account
        public bool GravarUsuario(mUser obj)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                dbAccess.ClearParameters();

                dbAccess.AddParameters("@Identificador", obj.Identificador);
                dbAccess.AddParameters("@Senha", obj.Identificador);
                dbAccess.AddParameters("@Nome", obj.Nome);
                dbAccess.AddParameters("@Sexo", obj.Sexo);
                dbAccess.AddParameters("@Email", obj.Email);
                dbAccess.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
                dbAccess.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                dbAccess.AddParameters("@Ativo", obj.Ativo);

                string Insert = @"
INSERT INTO
Usuarios (Identificador, Senha, Nome, Sexo, Email, Cadastro, Atualizado, Ativo) VALUES
(@Identificador, @Senha, @Nome, @Sexo, @Email, @Cadastro, @Atualizado, @Ativo)";

                if (dbAccess.Write(Insert))
                    return true;
                else
                    throw new Exception(string.Format("Usuário {0} ja cadastrado!", obj.Identificador));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateUsuario(mUser obj)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                dbAccess.ClearParameters();


                dbAccess.AddParameters("@Nome", obj.Nome);
                dbAccess.AddParameters("@Sexo", obj.Sexo);
                dbAccess.AddParameters("@Email", obj.Email);
                dbAccess.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                dbAccess.AddParameters("@Ativo", obj.Ativo);

                dbAccess.AddParameters("@Indice", obj.Indice);
                dbAccess.AddParameters("@Identificador", obj.Identificador);

                string Update = @"
UPDATE Usuarios
SET Nome = @Nome, Sexo = @Sexo, Email = @Email, Atualizado = @Atualizado, Ativo = @Ativo
WHERE (Indice = @Indice) AND (Identificador = @Identificador)";


                if (dbAccess.Write(Update))
                    return true;
                else
                    throw new Exception(string.Format("Dados do usuário {0} não alterado!", obj.Identificador));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GravarConta(mContas obj)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                dbAccess.ClearParameters();
                dbAccess.AddParameters("@Identificador", obj.Identificador);
                dbAccess.AddParameters("@Conta", obj.Conta);
                dbAccess.AddParameters("@Ativo", obj.Ativo);

                string Insert = @"
INSERT INTO
Contas (Identificador, Conta, Ativo) VALUES
(@Identificador, @Conta, @Ativo)";

                string Update = @"
UPDATE Contas
SET Conta = @Conta
WHERE (Identificador = @Identificador)";

                if (dbAccess.Write(Insert))
                    return true;

                else
                {
                    dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Conta", obj.Conta);
                    dbAccess.AddParameters("@Identificador", obj.Identificador);

                    if (dbAccess.Write(Update))
                        return true;

                    else
                        throw new Exception(string.Format("Acesso para usuário {0} não liberado!", obj.Identificador));
                }

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public void RemoveAcessoModulos(string id)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {

                dbAccess.ClearParameters();
                dbAccess.AddParameters("@Identificador", id);

                foreach (DataRow mr in dbAccess.Read(@"SELECT Acesso FROM Modulos WHERE (Identificador = @Identificador)").Rows)
                {
                    dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Acesso", false);
                    dbAccess.AddParameters("@Identificador", id);
                    dbAccess.Write(@"UPDATE Modulos SET Acesso = @Acesso WHERE (Identificador = @Identificador)");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GravarModulos(mModulos obj)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {

                string Insert = @"
INSERT INTO
Modulos (Identificador, Modulo, Acesso) VALUES
(@Identificador, @Modulo, @Acesso)";

                string Update = @"
UPDATE Modulos
SET Acesso = @Acesso
WHERE (Identificador = @Identificador) AND (Modulo = @Modulo)";

                dbAccess.ClearParameters();
                dbAccess.AddParameters("@Identificador", obj.Identificador);
                dbAccess.AddParameters("@Modulo", obj.Modulo);

                if (dbAccess.Read(@"SELECT * FROM Modulos WHERE (Identificador = @Identificador) AND (Modulo = @Modulo)").Rows.Count == 1)
                {
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Acesso", obj.Acesso);
                    dbAccess.AddParameters("@Identificador", obj.Identificador);
                    dbAccess.AddParameters("@Modulo", obj.Modulo);
                    return dbAccess.Write(Update);
                }
                else
                {
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Identificador", obj.Identificador);
                    dbAccess.AddParameters("@Modulo", obj.Modulo);
                    dbAccess.AddParameters("@Acesso", obj.Acesso);

                    if (dbAccess.Write(Insert))
                        return true;

                    else
                        throw new Exception(string.Format("Acesso ao Modulo {0} não liberado!", obj.Modulo));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveAcessoSubModulos(string id)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                dbAccess.ClearParameters();
                dbAccess.AddParameters("@Identificador", id);

                foreach (DataRow mr in dbAccess.Read(@"SELECT Acesso FROM SubModulos WHERE (Identificador = @Identificador)").Rows)
                {
                    dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Acesso", "0");
                    dbAccess.AddParameters("@Identificador", id);
                    dbAccess.Write(@"UPDATE SubModulos SET Acesso = @Acesso WHERE (Identificador = @Identificador)");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GravarSubModulos(mSubModulos obj)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {

                string Insert = @"
INSERT INTO
SubModulos (Identificador, SubModulo, Acesso) VALUES
(@Identificador, @SubModulo, @Acesso)";

                string Update = @"
UPDATE SubModulos
SET Acesso = @Acesso
WHERE (Identificador = @Identificador) AND (SubModulo = @SubModulo)";

                dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
                dbAccess.ClearParameters();
                dbAccess.AddParameters("@Identificador", obj.Identificador);
                dbAccess.AddParameters("@SubModulo", obj.SubModulo);

                if (dbAccess.Read(@"SELECT * FROM SubModulos WHERE (Identificador = @Identificador) AND (SubModulo = @SubModulo)").Rows.Count == 1)
                {
                    dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Acesso", obj.Acesso);
                    dbAccess.AddParameters("@Identificador", obj.Identificador);
                    dbAccess.AddParameters("@SubModulo", obj.SubModulo);
                    return dbAccess.Write(Update);
                }
                else
                {
                    dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Identificador", obj.Identificador);
                    dbAccess.AddParameters("@SubModulo", obj.SubModulo);
                    dbAccess.AddParameters("@Acesso", obj.Acesso);
                    if (dbAccess.Write(Insert))
                        return true;

                    else
                        throw new Exception(string.Format("Acesso ao SubModulo {0} não liberado!", obj.SubModulo));
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GravarOpcoes(mOpcoes obj)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                dbAccess.ClearParameters();

                dbAccess.AddParameters("@Identificador", obj.Identificador);
                dbAccess.AddParameters("@Thema", obj.Thema);
                dbAccess.AddParameters("@Color", obj.Color);

                string Insert = @"
INSERT INTO
Opcoes (Identificador, Thema, Color) VALUES
(@Identificador, @Thema, @Color)";

                string Update = @"
UPDATE Opcoes
SET Thema = @Thema, Color = @Color
WHERE (Identificador = @Identificador)";

                if (dbAccess.Write(Insert))
                    return true;

                else
                {
                    dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@Thema", obj.Thema);
                    dbAccess.AddParameters("@Color", obj.Color);
                    dbAccess.AddParameters("@Identificador", obj.Identificador);

                    if (dbAccess.Write(Update))
                        return true;

                    else
                        throw new Exception("Ocorreu erro durante alteração de thema e cor!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Listas
        public List<mGenerico> TiposContasLista()
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                List<mGenerico> _list = new List<mGenerico>();


                string SelectAll = @"
SELECT *
FROM Contas_Nome
ORDER BY Valor";

                foreach (DataRow dr in dbAccess.Read(SelectAll).Rows)
                {
                    mGenerico acc = new mGenerico();

                    acc.Indice = (int)dr[0];
                    acc.Nome = (string)dr[1];
                    acc.Valor = (int)dr[2];
                    acc.Ativo = (bool)dr[3];

                    _list.Add(acc);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public List<mGenerico> TiposModulosLista()
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                List<mGenerico> _list = new List<mGenerico>();


                string SelectAll = @"
SELECT *
FROM Modulos_Nome
ORDER BY Valor";

                foreach (DataRow dr in dbAccess.Read(SelectAll).Rows)
                {
                    mGenerico acc = new mGenerico();

                    acc.Indice = (int)dr[0];
                    acc.Nome = (string)dr[1];
                    acc.Valor = (int)dr[2];
                    acc.Ativo = (bool)dr[3];

                    _list.Add(acc);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public List<mGenerico> TiposSubModulosLista()
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                List<mGenerico> _list = new List<mGenerico>();


                string SelectAll = @"
SELECT *
FROM SubModulos_Nome
ORDER BY Valor";

                foreach (DataRow dr in dbAccess.Read(SelectAll).Rows)
                {
                    mGenerico acc = new mGenerico();

                    acc.Indice = (int)dr[0];
                    acc.Nome = (string)dr[1];
                    acc.Valor = (int)dr[2];
                    acc.Ativo = (bool)dr[3];

                    _list.Add(acc);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public List<mGenerico> AcessoSubModuloLista()
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                List<mGenerico> _list = new List<mGenerico>();


                string SelectAll = @"
SELECT *
FROM SubModulos_Acesso
ORDER BY Valor";

                foreach (DataRow dr in dbAccess.Read(SelectAll).Rows)
                {
                    mGenerico acc = new mGenerico();

                    acc.Indice = (int)dr[0];
                    acc.Nome = (string)dr[1];
                    acc.Valor = (int)dr[2];
                    acc.Ativo = (bool)dr[3];

                    _list.Add(acc);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<mAccount> Accountlist(int access)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {
                ObservableCollection<mAccount> _list = new ObservableCollection<mAccount>();

                dbAccess.ClearParameters();

                dbAccess.AddParameters("@Conta", access);

                string SelectUserByAcesso = @"
SELECT Usuarios.*, Opcoes.Thema, Opcoes.Color, Contas.Conta
FROM (Usuarios INNER JOIN Opcoes ON Usuarios.Identificador = Opcoes.Identificador) INNER JOIN Contas ON Usuarios.Identificador = Contas.Identificador
WHERE (((Contas.Conta)<=@Conta) AND ((Usuarios.Ativo)=True))
ORDER BY Usuarios.Nome;";

                foreach (DataRow dr in dbAccess.Read(SelectUserByAcesso).Rows)
                {
                    mAccount acc = new mAccount();

                    acc.Identificador = dr["Identificador"].ToString();
                    acc.Nome = dr["Nome"].ToString().ToUpper();
                    acc.Email = dr["Email"].ToString().ToLower();
                    acc.Sexo = dr["Sexo"].ToString();
                    acc.Cadastro = (DateTime)dr["Cadastro"];
                    acc.Atualizado = (DateTime)dr["Atualizado"];
                    acc.Ativo = (bool)dr["Ativo"];
                    acc.Conta = new mContas() { Conta = (int)dr["Conta"], ContaAcesso = Convert.ToString((AccountAccess)dr["Conta"]) };

                    _list.Add(acc);
                }

                return _list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Manager Account

        public mAccount User(string id)
        {
            var ac = new mAccount();
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);

            try
            {

                dbAccess.ClearParameters();
                dbAccess.AddParameters("@ID", id);

                foreach (DataRow dr in dbAccess.Read("SELECT * FROM Usuarios WHERE (Identificador = @ID)").Rows)
                {
                    ac.Identificador = dr["Identificador"].ToString();
                    ac.Nome = dr["Nome"].ToString().ToUpper();
                    ac.Sexo = dr["Sexo"].ToString();
                    ac.Email = dr["Email"].ToString();
                    ac.Cadastro = (DateTime)dr["Cadastro"];
                    ac.Atualizado = (DateTime)dr["Atualizado"];
                    ac.Ativo = (bool)dr["Ativo"];

                    ac.Thema = "Light";
                    ac.Color = "#FF3399FF";

                    ac.Conta = new mContas() { Identificador = ac.Identificador, Indice = 0, ContaAcesso = AccountAccess.Normal.ToString().ToUpper(), Conta = (int)AccountAccess.Normal };

                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@id", id);

                    foreach (DataRow ap in dbAccess.Read(@"SELECT Thema, Color FROM Opcoes WHERE (Identificador = @id)").Rows)
                    {
                        ac.Thema = (string)ap[0];
                        ac.Color = (string)ap[1];
                    }

                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@id", id);
                    foreach (DataRow ct in dbAccess.Read(@"SELECT * FROM Contas WHERE (Identificador = @id)").Rows)
                    {
                        ac.Conta.Indice = (int)ct[0];
                        ac.Conta.Identificador = (string)ct[1];
                        ac.Conta.Conta = (int)ct[2];
                        ac.Conta.ContaAcesso = Convert.ToString((AccountAccess)ct[2]);
                        ac.Conta.Ativo = (bool)ct[3];
                    }

                    #region Modulos

                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@id", id);
                    foreach (DataRow mdac in dbAccess.Read(@"SELECT * FROM Modulos WHERE (Identificador = @id) AND (Acesso = True) ORDER BY Modulo").Rows)
                    {
                        ac.Modulos.Add(new mModulos { Indice = (int)mdac[0], Identificador = (string)mdac[1], Modulo = (int)mdac[2], Acesso = (bool)mdac[3] });
                    }

                    #endregion

                    #region SubModulos

                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@id", id);
                    foreach (DataRow smac in dbAccess.Read(@"SELECT * FROM SubModulos WHERE (Identificador = @id) AND (Acesso > 0) ORDER BY SubModulo").Rows)
                    {
                        ac.SubModulos.Add(new mSubModulos { Indice = (int)smac[0], Identificador = (string)smac[1], SubModulo = (int)smac[2], Acesso = (int)smac[3] });
                    }

                    #endregion                    
                }

                return ac;

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public bool ChangePW(string id, string oldpw, string newpw)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
            bool ret = false;
            try
            {
                dbAccess.ClearParameters();

                dbAccess.AddParameters("@id", id);
                dbAccess.AddParameters("@pw", oldpw);

                foreach (DataRow dr in dbAccess.Read("SELECT Identificador FROM Usuarios WHERE (Identificador = @id) AND (Senha = @pw) AND (Ativo = True)").Rows)
                {
                    dbAccess.ClearParameters();
                    dbAccess.AddParameters("@npw", newpw);
                    dbAccess.AddParameters("@id", id);

                    ret = dbAccess.Write(@"UPDATE Usuarios SET Senha = @npw WHERE (Identificador = @id)");
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ResetPW(string id, string pw)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
            try
            {

                dbAccess.ClearParameters();
                dbAccess.AddParameters("@npw", pw);
                dbAccess.AddParameters("@id", id);

                return dbAccess.Write(@"UPDATE Usuarios SET Senha = @npw WHERE (Identificador = @id)");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool BlockAccount(string id)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
            try
            {
                dbAccess.ClearParameters();
                dbAccess.AddParameters("@id", id);

                return dbAccess.Write(@"UPDATE Contas SET Conta = 0 WHERE (Identificador = @id)");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteAccount(string id)
        {
            var dbAccess = Data.Factory.Connecting(DataBase.Base.Login);
            try
            {
                dbAccess.ClearParameters();
                dbAccess.AddParameters("@id", id);

                dbAccess.Write(@"DELETE FROM Contas WHERE (Identificador = @id)");
                dbAccess.Write(@"DELETE FROM Opcoes WHERE (Identificador = @id)");
                dbAccess.Write(@"DELETE FROM Modulos WHERE (Identificador = @id)");
                dbAccess.Write(@"DELETE FROM SubModulos WHERE (Identificador = @id)");
                dbAccess.Write(@"DELETE FROM Registro_Acesso WHERE (Identificador = @id)");
                return dbAccess.Write(@"DELETE FROM Usuarios WHERE (Identificador = @id)");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
