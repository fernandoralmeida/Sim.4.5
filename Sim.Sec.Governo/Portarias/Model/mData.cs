using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;

namespace Sim.Sec.Governo.Portarias.Model
{

    using Sim.Sec.Governo.Portarias.Sql;

    class mData
    {
        public ObservableCollection<mPortaria> ListaDocumentos { get; set; }

        public bool Insert(mPortaria obj)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);
            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Numero", obj.Numero);
                dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                dataAccess.AddParameters("@Tipo", obj.Tipo);
                dataAccess.AddParameters("@Resumo", obj.Resumo);

                StringBuilder nome = new StringBuilder();

                foreach (mServidor pessoa in obj.Servidor)
                {
                    InsertNewServidor(pessoa.Nome, DateTime.Now);
                    nome.Append(pessoa.Nome + ";");
                }

                if (nome.Length > 1)
                    nome = nome.Remove(nome.Length - 1, 1);

                dataAccess.AddParameters("@Servidor", nome.ToString());
                dataAccess.AddParameters("@Link", obj.Pdf);
                dataAccess.AddParameters("@Publicado", obj.Publicada);
                dataAccess.AddParameters("@Cadastro", obj.Cadastrado.ToShortDateString());
                dataAccess.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                dataAccess.AddParameters("@Excluido", obj.Excluido);

                return dataAccess.Write(SqlCollections.InsertQuery);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool InsertNewServidor(string name, DateTime data)
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);
            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Servidor", name);
                dataAccess.AddParameters("@Inserido", data.ToShortDateString());
                dataAccess.AddParameters("@Bloqueado", false);

                return dataAccess.Write(SqlCollections.InsertNewServidor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Update(mPortaria obj)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Numero", obj.Numero);
                dataAccess.AddParameters("@Data", obj.Data.ToShortDateString());
                dataAccess.AddParameters("@Tipo", obj.Tipo);
                dataAccess.AddParameters("@Resumo", obj.Resumo);

                StringBuilder nome = new StringBuilder();

                foreach (mServidor pessoa in obj.Servidor)
                {
                    InsertNewServidor(pessoa.Nome, DateTime.Now);
                    nome.Append(pessoa.Nome + ";");
                }

                if (nome.Length > 1)
                    nome = nome.Remove(nome.Length - 1, 1);

                dataAccess.AddParameters("@Servidor", nome.ToString());
                dataAccess.AddParameters("@Link", obj.Pdf);
                dataAccess.AddParameters("@Publicado", obj.Publicada);
                dataAccess.AddParameters("@Cadastro", obj.Cadastrado.ToShortDateString());
                dataAccess.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                dataAccess.AddParameters("@Excluido", obj.Excluido);

                dataAccess.AddParameters("@Original_Indice", obj.Indice);

                return dataAccess.Write(SqlCollections.UpdateQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Delete(mPortaria obj)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<mPortaria> ConsultaSimples(int numero)
        {
            var list = new ObservableCollection<mPortaria>();

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Numero", numero);

                List<mClassificacao> tipos = new List<mClassificacao>();

                tipos = ListaGenerica(SqlCollections.Classi_With_Blocked);

                foreach (DataRow dr in dataAccess.Read(SqlCollections.SelectNumber).Rows)
                {
                    var docs = new mPortaria();
                    docs.Indice = (int)dr["Indice"];
                    docs.IndiceLink = dr["Indice"].ToString();
                    docs.Numero = (int)dr["Numero"];
                    docs.Data = (DateTime)dr["Data"];
                    docs.Publicada = dr["Publicado"].ToString();
                    docs.Resumo = (string)dr["Resumo"];
                    docs.Pdf = (string)dr["Link"];
                    docs.Cadastrado = (DateTime)dr["Cadastro"];
                    docs.Atualizado = (DateTime)dr["Atualizado"];
                    docs.Excluido = (bool)dr["Excluido"];

                    int tipo = (int)dr["Tipo"];

                    docs.Tipo = string.Format("{0}", tipos[tipo].Nome);

                    string[] linha = dr["Servidor"].ToString().Split(';');

                    ObservableCollection<mServidor> func = new ObservableCollection<mServidor>();

                    foreach (string l in linha)
                    {
                        mServidor svnome = new mServidor();
                        svnome.Nome = l.ToString();
                        func.Add(svnome);
                    }

                    docs.Servidor = func;
                    list.Add(docs);
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }

        public ObservableCollection<mPortaria> ConsultaDetelhada(string tipo, string nome, string resumo, DateTime datai, DateTime dataf)
        {
            var list = new ObservableCollection<mPortaria>();

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {
                if (tipo == "0")
                    tipo = "%";
                
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Data1", datai.ToShortDateString());
                dataAccess.AddParameters("@Data2", dataf.ToShortDateString());
                dataAccess.AddParameters("@Tipo", tipo);
                dataAccess.AddParameters("@Resumo", "%" + resumo + "%");
                dataAccess.AddParameters("@Nome", "%" + nome + "%");

                List<mClassificacao> tipos = new List<mClassificacao>();

                tipos = ListaGenerica(SqlCollections.Classi_With_Blocked);

                int vreg = dataAccess.Read(SqlCollections.SelectParameters).Rows.Count;

                int cont_reg = 0;

                foreach (DataRow dr in dataAccess.Read(SqlCollections.SelectParameters).Rows)
                {
                    cont_reg += 1;

                    var docs = new mPortaria();
                    docs.Indice = (int)dr["Indice"];
                    docs.IndiceLink = dr["Indice"].ToString();
                    docs.Numero = (int)dr["Numero"];
                    docs.Data = (DateTime)dr["Data"];
                    docs.Publicada = dr["Publicado"].ToString();
                    docs.Resumo = (string)dr["Resumo"];
                    docs.Pdf = (string)dr["Link"];
                    docs.Cadastrado = (DateTime)dr["Cadastro"];
                    docs.Atualizado = (DateTime)dr["Atualizado"];
                    docs.Excluido = (bool)dr["Excluido"];

                    int rtipo = (int)dr["Tipo"];

                    docs.Tipo = string.Format("{0}", tipos[rtipo].Nome);

                    string[] linha = dr["Servidor"].ToString().Split(';');

                    ObservableCollection<mServidor> func = new ObservableCollection<mServidor>();

                    foreach (string l in linha)
                    {
                        mServidor svnome = new mServidor();
                        svnome.Nome = l.ToString();
                        func.Add(svnome);
                    }

                    docs.Servidor = func;
                    list.Add(docs);
                    
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }

        public List<mPortaria> ListaParaEdicao(int indice)
        {

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Indice", indice);

                List<mClassificacao> tipos = new List<mClassificacao>();

                tipos = ListaGenerica(SqlCollections.Classi_With_Blocked);

                List<mPortaria> lista = new List<mPortaria>();

                foreach (DataRow dr in dataAccess.Read(SqlCollections.SelectIndice).Rows)
                {
                    var docs = new mPortaria();
                    docs.Indice = (int)dr["Indice"];
                    docs.IndiceLink = dr["Indice"].ToString();
                    docs.Numero = (int)dr["Numero"];
                    docs.Data = (DateTime)dr["Data"];
                    docs.Publicada = dr["Publicado"].ToString();
                    docs.Resumo = (string)dr["Resumo"];
                    docs.Pdf = (string)dr["Link"];
                    docs.Cadastrado = (DateTime)dr["Cadastro"];
                    docs.Atualizado = (DateTime)dr["Atualizado"];
                    docs.Excluido = (bool)dr["Excluido"];
                    docs.Tipo = dr["Tipo"].ToString();

                    string[] linha = dr["Servidor"].ToString().Split(';');

                    ObservableCollection<mServidor> func = new ObservableCollection<mServidor>();

                    foreach (string l in linha)
                    {
                        mServidor svnome = new mServidor();
                        svnome.Nome = l.ToString();
                        func.Add(svnome);
                    }

                    docs.Servidor = func;

                    lista.Add(docs);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }

        public List<mClassificacao> ListaGenerica(string comando)
        {
            var lst = new List<mClassificacao>();

            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {

                foreach (DataRow dr in dataAccess.Read(comando).Rows)
                {
                    var classe = new mClassificacao();
                    classe.Codigo = (int)dr[1];//Codigo
                    classe.Nome = (string)dr[2];//Nome
                    //classe.Cadastro = (DateTime)dr[3];
                    //classe.Alterado = (DateTime)dr[4];
                    classe.Bloqueado = (bool)dr[5];//Bloqueado
                    lst.Add(classe);
                }

            }
            catch
            {
                lst.Clear();
                var classe = new mClassificacao();
                classe.Codigo = 0;
                classe.Nome = "...";
                lst.Add(classe);
            }

            return lst;
        }

        public List<string> ListName()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);

            List<string> lst = new List<string>();

            try
            {
                dataAccess.ClearParameters();
                //dataAccess.AddParameters("@Nome", name + "%");

                foreach (DataRow dr in dataAccess.Read(SqlCollections.SelectServidores).Rows)
                {
                    lst.Add(dr["Nome"].ToString());
                }
            }
            catch
            {
                lst.Add("...");
            }

            return lst;
        }

        public bool BlockNoBlockClassificacao(string commandsql, mClassificacao obj)
        {

            var mdata = Data.Factory.Connecting(DataBase.Base.Governo);

            mdata.ClearParameters();
            mdata.AddParameters("@Bloqueado", obj.Bloqueado);
            mdata.AddParameters("@Codigo", obj.Codigo);

            return mdata.Write(commandsql);
        }

        public bool InsertClassificacao(string sqlcommand1, string sqlcommand2, mClassificacao obj)
        {

            var mdata = Data.Factory.Connecting(DataBase.Base.Governo);

            int last_codigo = 0;

            foreach (DataRow dr in mdata.Read(sqlcommand2).Rows)
                last_codigo = (int)dr[0] + 1;

            mdata.ClearParameters();

            mdata.AddParameters("@Codigo", last_codigo);
            mdata.AddParameters("@Nome", obj.Nome);
            mdata.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
            mdata.AddParameters("@Alterado", obj.Alterado.ToShortDateString());
            mdata.AddParameters("@Bloqueado", obj.Bloqueado);

            return mdata.Write(sqlcommand1);
        }

        public ObservableCollection<mPortaria> LastRows()
        {
            var dataAccess = Data.Factory.Connecting(DataBase.Base.Governo);
            try
            {
                var list = new ObservableCollection<mPortaria>();

                string sqlport = @"SELECT TOP 20 * FROM Portarias ORDER BY Cadastro DESC, Numero DESC";

                List<mClassificacao> tipos = new List<mClassificacao>();

                tipos = ListaGenerica(SqlCollections.Classi_With_Blocked);

                foreach (DataRow dr in dataAccess.Read(sqlport).Rows)
                {
                    var docs = new mPortaria();
                    docs.Indice = (int)dr["Indice"];
                    docs.IndiceLink = dr["Indice"].ToString();
                    docs.Numero = (int)dr["Numero"];
                    docs.Data = (DateTime)dr["Data"];
                    docs.Publicada = dr["Publicado"].ToString();
                    docs.Resumo = (string)dr["Resumo"];
                    docs.Pdf = (string)dr["Link"];
                    docs.Cadastrado = (DateTime)dr["Cadastro"];
                    docs.Atualizado = (DateTime)dr["Atualizado"];
                    docs.Excluido = (bool)dr["Excluido"];

                    int tipo = (int)dr["Tipo"];

                    docs.Tipo = string.Format("{0}", tipos[tipo].Nome);

                    string[] linha = dr["Servidor"].ToString().Split(';');

                    ObservableCollection<mServidor> func = new ObservableCollection<mServidor>();

                    foreach (string l in linha)
                    {
                        mServidor svnome = new mServidor();
                        svnome.Nome = l.ToString();
                        func.Add(svnome);
                    }

                    docs.Servidor = func;
                    list.Add(docs);
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataAccess = null;
            }
        }
    }
}
