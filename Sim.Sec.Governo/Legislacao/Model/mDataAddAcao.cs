using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Sec.Governo.Legislacao.Model
{

    using Sim.Data;

    class mDataAddAcao
    {
        /// <summary>
        /// Grava uma nova acao na tabela ligislacaoacoes
        /// Altera a situação do documento alvo da ação
        /// </summary>
        /// <param name="obj">objeto para serem gravados</param>
        /// <returns> Retorna Lista com o ultimo registro da tabela legislacao</returns>
        public mAcoes Add(mAcoes obj)
        {
            IData AcoesDados = Factory.Connecting(DataBase.Base.Governo);

            try
            {
                string sqlcommandAcoes = @"INSERT INTO [Leg_Acoes] ([TipoOrigem], [NumeroOrigem], [ComplementoOrigem], [DataOrigem], [AcaoExecutada], [TipoAlvo], [NumeroAlvo], [ComplementoAlvo], [DataAlvo], [Inserido]) VALUES (@TipoOrigem, @NumeroOrigem, @CompOrigem, @DataOrigem, @Acao, @TipoAlvo, @NumeroAlvo, @CompAlvo, @DataAlvo, @Inserido)";

                AcoesDados.ClearParameters();

                AcoesDados.AddParameters("@TipoOrigem", obj.TipoOrigem);
                AcoesDados.AddParameters("@NumeroOrigem", obj.NumeroOrigem);
                AcoesDados.AddParameters("@CompOrigem", obj.ComplementoOrigem);
                AcoesDados.AddParameters("@DataOrigem", obj.DataOrigem.ToShortDateString());
                AcoesDados.AddParameters("@Acao", obj.Acao);
                AcoesDados.AddParameters("@TipoAlvo", obj.TipoAlvo);
                AcoesDados.AddParameters("@NumeroAlvo", obj.NumeroAlvo);
                AcoesDados.AddParameters("@CompAlvo", obj.ComplementoAlvo);
                AcoesDados.AddParameters("@DataAlvo", obj.DataAlvo.ToShortDateString());
                AcoesDados.AddParameters("@Inserido", obj.Incluido.ToShortDateString());

                var upSituacao = Factory.Connecting(DataBase.Base.Governo);

                string novaSituacao = obj.Acao;

                string sqlcommandalUpdateAlvo = @"UPDATE [Legislacao] SET [Situacao] = @Situacao WHERE (Tipo = @TipoAlvo) AND (Numero = @NumeroAlvo) AND (Complemento = @CompAlvo) AND (Data = @DataAlvo)";

                switch (obj.Acao)
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
                upSituacao.AddParameters("@TipoAlvo", obj.TipoAlvo);
                upSituacao.AddParameters("@NumeroAlvo", obj.NumeroAlvo);
                upSituacao.AddParameters("@CompAlvo", obj.ComplementoAlvo);
                upSituacao.AddParameters("@DataAlvo", obj.DataAlvo.ToShortDateString());

                upSituacao.Write(sqlcommandalUpdateAlvo);

                AcoesDados.Write(sqlcommandAcoes);

                string sqlreturn = @"SELECT TOP 1 Indice, TipoAlvo, NumeroAlvo, ComplementoAlvo, DataAlvo, AcaoExecutada FROM Leg_Acoes ORDER BY Indice DESC";
                AcoesDados.ClearParameters();

                var acoes = new mAcoes();

                foreach (DataRow ac in AcoesDados.Read(sqlreturn).Rows)
                {

                    acoes.Indice = (int)ac["Indice"];
                    acoes.TipoAlvo = ac["TipoAlvo"].ToString();
                    acoes.NumeroAlvo = (int)ac["NumeroAlvo"];
                    acoes.ComplementoAlvo = ac["ComplementoAlvo"].ToString();
                    acoes.DataAlvo = (DateTime)ac["DataAlvo"];
                    acoes.Acao = new mAcoesExercidas().Exercidas((int)(ac["AcaoExecutada"]));
                }

                return acoes;
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
