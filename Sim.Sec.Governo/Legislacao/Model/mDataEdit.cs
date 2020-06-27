using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mDataEdit
    {
        /// <summary>
        /// Altera as Informações do document.
        /// </summary>
        /// <param name="obj">obj que contem as informações para serem alteradas</param>
        /// <returns>true ou false</returns>
        public bool Update(mLegislacao obj)
        {
            string sqlcommand = @"UPDATE [Legislacao] SET [Tipo] = @Tipo, [Numero] = @Numero, [Complemento] = @Complemento, [Data] = @Data, [Publicado] = @Publicado, [Resumo] = @Resumo, [Classificado] = @Classificado, [Link] = @Link, [Situacao] = @Situacao, [Origem] = @Origem, [Autor] = @Autor, [Cadastro] = @Cadastro, [Atualizado] = @Atualizado,  [Excluido] = @Excluido WHERE ([Indice] = @Original_Indice)";

            Sim.Data.IData AcessarDados = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {
                AcessarDados.ClearParameters();
                AcessarDados.AddParameters("@Tipo", obj.Tipo);
                AcessarDados.AddParameters("@Numeo", obj.Numero);
                AcessarDados.AddParameters("@Complemento", obj.Complemento);
                AcessarDados.AddParameters("@Data", obj.Data);
                AcessarDados.AddParameters("@Publicado", obj.Publicado);
                AcessarDados.AddParameters("@Resumo", obj.Resumo);
                AcessarDados.AddParameters("@Classificado", obj.Classificacao);
                AcessarDados.AddParameters("@Link", obj.Link);
                AcessarDados.AddParameters("@Situacao", obj.Situacao);
                AcessarDados.AddParameters("@Origem", obj.Origem);
                AcessarDados.AddParameters("@Autor", obj.Autor);
                AcessarDados.AddParameters("@Cadastro", obj.Cadastro);
                AcessarDados.AddParameters("@Atualizado", obj.Atualizado.ToShortDateString());
                AcessarDados.AddParameters("@Excluido", obj.Excluido);

                AcessarDados.AddParameters("@Original_Indice", obj.Indice);

                return AcessarDados.Write(sqlcommand);

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
            finally
            {
                AcessarDados = null;
            }
        }

        /// <summary>
        /// Gera lista com dados a serem Alterados.
        /// </summary>
        /// <param name="obj">objeto que contem parametros pra gerar a lista</param>
        /// <returns>Lista</returns>
        public mLegislacao EditDoc(int indice)
        {
            var dataLegis = Data.Factory.Connecting(DataBase.Base.Governo);
            var dataAcoes = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {

                string sqlCommand = @"SELECT * FROM Legislacao WHERE (Indice = @Indice) AND (Excluido = 0)";
                string sqlCommandAcoes = @"SELECT * FROM Leg_Acoes WHERE(TipoOrigem LIKE @Tipo) AND (NumeroOrigem = @Numero) AND (ComplementoOrigem LIKE @Comp)";

                dataLegis.ClearParameters();
                dataLegis.AddParameters("@Indice", indice);

                //System.Windows.Forms.MessageBox.Show(obj.Tipo + obj.Numero + obj.Complemento);

                //var lista = new Model.DocsCollections();
                var legislacao = new mLegislacao();

                foreach (DataRow leg in dataLegis.Read(sqlCommand).Rows)
                {                    

                    legislacao.Indice = (int)leg["Indice"];
                    legislacao.Tipo = leg["Tipo"].ToString();
                    legislacao.Numero = (int)leg["Numero"];
                    legislacao.Complemento = leg["Complemento"].ToString();
                    legislacao.Data = (DateTime)leg["Data"];
                    legislacao.Publicado = leg["Publicado"].ToString();
                    legislacao.Resumo = leg["Resumo"].ToString();
                    legislacao.Classificacao = leg["Classificado"].ToString();
                    legislacao.Link = mLink.Create(legislacao.Tipo, legislacao.Data.ToString("yyyy"), legislacao.Numero);
                    legislacao.Situacao = leg["Situacao"].ToString();
                    legislacao.Origem = leg["Origem"].ToString();
                    legislacao.Autor = leg["Autor"].ToString();
                    legislacao.Cadastro = (DateTime)leg["Cadastro"];
                    legislacao.Atualizado = (DateTime)leg["Atualizado"];
                    legislacao.Excluido = (bool)leg["Excluido"];

                    dataAcoes.ClearParameters();
                    dataAcoes.AddParameters("@Tipo", legislacao.Tipo);
                    dataAcoes.AddParameters("@Numero", legislacao.Numero);
                    dataAcoes.AddParameters("@Comp", legislacao.Complemento);

                    var listaAcoes = new List<mAcoes>();

                    foreach (DataRow ac in dataAcoes.Read(sqlCommandAcoes).Rows)
                    {
                        var acoes = new mAcoes();
                        acoes.Indice = (int)ac["Indice"];

                        acoes.TipoOrigem = legislacao.Tipo;
                        acoes.NumeroOrigem = legislacao.Numero;
                        acoes.ComplementoOrigem = legislacao.Complemento;
                        acoes.DataOrigem = legislacao.Data;

                        acoes.Acao = new mAcoesExercidas().Exercidas((int)(ac["AcaoExecutada"]));

                        acoes.TipoAlvo = ac["TipoAlvo"].ToString();
                        acoes.NumeroAlvo = (int)ac["NumeroAlvo"];
                        acoes.ComplementoAlvo = ac["ComplementoAlvo"].ToString();
                        acoes.DataAlvo = (DateTime)ac["DataAlvo"];

                        acoes.Incluido = (DateTime)ac["Inserido"];

                        listaAcoes.Add(acoes);
                    }

                    legislacao.ListaAcoes = listaAcoes;

                    //lista.Add(legislacao);
                }

                return legislacao;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataLegis = null;
                dataAcoes = null;
            }
        }
    }
}
