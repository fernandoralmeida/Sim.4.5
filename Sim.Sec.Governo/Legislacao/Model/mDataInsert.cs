using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.Model
{
    using Sim.Data;
    using SqlCommands;

    class mDataInsert
    {
        /// <summary>
        /// Grava dados na tabela "legislacao", "legislacaoacoes"
        /// verifica e altera a situacao do documento alvo da acao.
        /// </summary>
        /// <param name="obj">objeto contendo dados para gravar na tabela legislacao</param>
        /// <param name="lista">lista de objeto contendo as acoes para tabela legislacaoacoes</param>
        /// <returns>se gravar, retorna true</returns>
        public bool Insert(mLegislacao obj)
        {
            string sqlcommand = @"INSERT INTO [Legislacao] ([Tipo], [Numero], [Complemento], [Data], [Publicado], [Resumo], [Classificado], [Link], [Situacao], [Origem], [Autor], [Cadastro], [Atualizado], [Excluido]) VALUES (@Tipo, @Numero, @Complemento, @Data, @Publicado, @Resumo, @Classificado, @Link, @Situacao, @Origem, @Autor, @Cadastro, @Atualizado, @Excluido)";

            IData AcessarDados = Data.Factory.Connecting(DataBase.Base.Governo);

            string comp = obj.Complemento;

            if (obj.Complemento == null)
                obj.Complemento = string.Empty;

            if (comp == null || comp == string.Empty)
                comp = "%";     

            try
            {
                AcessarDados.ClearParameters();
                AcessarDados.AddParameters("@Tipo", obj.Tipo);
                AcessarDados.AddParameters("@Numeo", obj.Numero);
                AcessarDados.AddParameters("@Complemento", comp);

                //checa se ja existe registro com as informações do obj.
                if (AcessarDados.Read(SqlCollections.SelectSimples).Rows.Count > 0)
                    return false;

                AcessarDados.ClearParameters();
                AcessarDados.AddParameters("@Tipo", obj.Tipo);
                AcessarDados.AddParameters("@Numeo", obj.Numero);
                AcessarDados.AddParameters("@Complemento", obj.Complemento);
                AcessarDados.AddParameters("@Data", obj.Data.ToShortDateString());
                AcessarDados.AddParameters("@Publicado", obj.Publicado);
                AcessarDados.AddParameters("@Resumo", obj.Resumo);
                AcessarDados.AddParameters("@Classificado", obj.Classificacao);
                AcessarDados.AddParameters("@Link", obj.Link);
                AcessarDados.AddParameters("@Situacao", obj.Situacao);
                AcessarDados.AddParameters("@Origem", obj.Origem);
                AcessarDados.AddParameters("@Autor", obj.Autor);
                AcessarDados.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
                AcessarDados.AddParameters("@Atualizado", obj.Cadastro.ToShortDateString());
                AcessarDados.AddParameters("@Excluido", obj.Excluido);
                
                IData AcoesDados = Data.Factory.Connecting(DataBase.Base.Governo);
                string sqlcommandAcoes = @"INSERT INTO [Leg_Acoes] ([TipoOrigem], [NumeroOrigem], [ComplementoOrigem], [DataOrigem], [AcaoExecutada], [TipoAlvo], [NumeroAlvo], [ComplementoAlvo], [DataAlvo], [Inserido]) VALUES (@TipoOrigem, @NumeroOrigem, @CompOrigem, @DataOrigem, @Acao, @TipoAlvo, @NumeroAlvo, @CompAlvo, @DataAlvo, @Inserido)";

                //lista as acoes para serem gravados
                foreach (mAcoes acoes in obj.ListaAcoes)
                {

                    string compOrigem = acoes.ComplementoOrigem;
                    string compAlvo = acoes.ComplementoAlvo;

                    if (compOrigem == null)
                        compOrigem = string.Empty;

                    if (compAlvo == null)
                        compAlvo = string.Empty;

                    AcoesDados.ClearParameters();

                    AcoesDados.AddParameters("@TipoOrigem", acoes.TipoOrigem);
                    AcoesDados.AddParameters("@NumeroOrigem", acoes.NumeroOrigem);
                    AcoesDados.AddParameters("@CompOrigem", compOrigem);
                    AcoesDados.AddParameters("@DataOrigem", acoes.DataOrigem.ToShortDateString());
                    AcoesDados.AddParameters("@Acao", acoes.Acao);
                    AcoesDados.AddParameters("@TipoAlvo", acoes.TipoAlvo);
                    AcoesDados.AddParameters("@NumeroAlvo", acoes.NumeroAlvo);
                    AcoesDados.AddParameters("@CompAlvo", compAlvo);
                    AcoesDados.AddParameters("@DataAlvo", acoes.DataAlvo.ToShortDateString());
                    AcoesDados.AddParameters("@Inserido", acoes.Incluido.ToShortDateString());

                    if (AcoesDados.Write(sqlcommandAcoes) == false)
                        return false;

                    //checa a acao e altera a situacao do documento alvo
                    var upSituacao = Data.Factory.Connecting(DataBase.Base.Governo);

                    string novaSituacao = acoes.Acao;

                    string sqlcommandalUpdateAlvo = @"UPDATE [Legislacao] SET [Situacao] = @Situacao WHERE (Tipo = @TipoAlvo) AND (Numero = @NumeroAlvo) AND (Complemento = @CompAlvo) AND (Data = @DataAlvo)";

                    switch (acoes.Acao)
                    {
                        case "1":
                            novaSituacao = "2";
                            break;

                        case "2":
                            novaSituacao = "3";
                            break;
                    }

                    upSituacao.ClearParameters();

                    upSituacao.AddParameters("@Situacao", novaSituacao);
                    upSituacao.AddParameters("@TipoAlvo", acoes.TipoAlvo);
                    upSituacao.AddParameters("@NumeroAlvo", acoes.NumeroAlvo);
                    upSituacao.AddParameters("@CompAlvo", acoes.ComplementoAlvo);
                    upSituacao.AddParameters("@DataAlvo", acoes.DataAlvo.ToShortDateString());

                    upSituacao.Write(sqlcommandalUpdateAlvo);

                }
                
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
    }
}
