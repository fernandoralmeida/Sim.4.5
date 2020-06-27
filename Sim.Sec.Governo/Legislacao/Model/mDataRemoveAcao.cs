using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Sec.Governo.Legislacao.Model
{
    class mDataRemoveAcao
    {
        /// <summary>
        /// Apaga uma ação da tabela legislacaoacoes
        /// verifica o objeto alvo da ação a ser apagada na tabela legislacaoacoes
        /// se tiver somente uma (1) acao sofrida, altera a situacao na tabela legislacao,
        /// se tivser mais de uma (1), mantem a situacao do documento alvo na tabela legislacao.
        /// retorna uma lista atualizada das acoes restantes ref. ao objeto consultante
        /// </summary>
        /// <param name="obj">objeto acoes</param>
        /// <returns>true ou false</returns>
        public List<mAcoes> DelAcao(mAcoes obj)
        {
            string sqlcommand = @"DELETE FROM Leg_Acoes WHERE (Indice = @Indice)";

            Sim.Data.IData AcoesDados = Data.Factory.Connecting(DataBase.Base.Governo);

            try
            {

                var upSituacao = Data.Factory.Connecting(DataBase.Base.Governo);

                string novaSituacao = obj.Acao;

                string sqlcommandRollbackSituacao = @"SELECT * FROM Leg_Acoes WHERE (TipoAlvo = @TipoAlvo) AND (NumeroAlvo = @NumeroAlvo) AND (ComplementoAlvo = @CompAlvo) AND (DataAlvo = @DataAlvo)";

                upSituacao.ClearParameters();

                upSituacao.AddParameters("@TipoAlvo", obj.TipoAlvo);
                upSituacao.AddParameters("@NumeroAlvo", obj.NumeroAlvo);
                upSituacao.AddParameters("@CompAlvo", obj.ComplementoAlvo);
                upSituacao.AddParameters("@DataAlvo", obj.DataAlvo.ToShortDateString());

                if (upSituacao.Read(sqlcommandRollbackSituacao).Rows.Count == 1)
                {
                    novaSituacao = "1";

                    string sqlcommandUpdateSituacao = @"UPDATE [Legislacao] SET [Situacao] = @Situacao WHERE (Tipo = @TipoAlvo) AND (Numero = @NumeroAlvo) AND (Complemento = @CompAlvo) AND (Data = @DataAlvo)";

                    upSituacao.ClearParameters();

                    upSituacao.AddParameters("@Situacao", novaSituacao);
                    upSituacao.AddParameters("@TipoAlvo", obj.TipoAlvo);
                    upSituacao.AddParameters("@NumeroAlvo", obj.NumeroAlvo);
                    upSituacao.AddParameters("@CompAlvo", obj.ComplementoAlvo);
                    upSituacao.AddParameters("@DataAlvo", obj.DataAlvo.ToShortDateString());

                    upSituacao.Write(sqlcommandUpdateSituacao);
                }

                AcoesDados.ClearParameters();

                AcoesDados.AddParameters("@Indice", obj.Indice);

                var listaAcoes = new List<mAcoes>();
                string sqlCommandAcoes = @"SELECT * FROM Leg_Acoes WHERE(TipoOrigem LIKE @Tipo) AND (NumeroOrigem = @Numero) AND (ComplementoOrigem LIKE @Comp)";

                if (AcoesDados.Write(sqlcommand))
                {

                    AcoesDados.ClearParameters();
                    AcoesDados.AddParameters("@Tipo", obj.TipoOrigem);
                    AcoesDados.AddParameters("@Numero", obj.NumeroOrigem);
                    AcoesDados.AddParameters("@Comp", obj.ComplementoOrigem);

                    foreach (DataRow ac in AcoesDados.Read(sqlCommandAcoes).Rows)
                    {
                        var acoes = new mAcoes();
                        acoes.Indice = (int)ac["Indice"];
                        acoes.TipoAlvo = ac["TipoAlvo"].ToString();
                        acoes.NumeroAlvo = (int)ac["NumeroAlvo"];
                        acoes.ComplementoAlvo = ac["ComplementoAlvo"].ToString();
                        acoes.DataAlvo = (DateTime)ac["DataAlvo"];
                        acoes.Acao = new mAcoesExercidas().Exercidas((int)(ac["AcaoExecutada"]));
                        listaAcoes.Add(acoes);
                    }

                }

                return listaAcoes;

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
            finally
            {
                AcoesDados = null;
            }
        }
    }
}
