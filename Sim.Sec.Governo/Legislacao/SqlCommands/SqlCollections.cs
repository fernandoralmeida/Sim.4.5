using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Sec.Governo.Legislacao.SqlCommands
{
    public static partial class SqlCollections
    {
        
        public static string Class_L_Only_Non_Blocked = @"SELECT * FROM Leg_Lo_Lc_Classi WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Class_L_All = @"SELECT * FROM Leg_Lo_Lc_Classi ORDER BY Codigo";
        public static string Class_L_Block_NoBlock = @"UPDATE Leg_Lo_Lc_Classi SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Class_L_Insert = @"INSERT INTO Leg_Lo_Lc_Classi (Codigo, Classificacao, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Class_L_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_Lo_Lc_Classi ORDER BY Codigo DESC";

        public static string Class_D_Only_Non_Blocked = @"SELECT * FROM Leg_De_Classi WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Class_D_All = @"SELECT * FROM Leg_De_Classi ORDER BY Codigo";
        public static string Class_D_Block_NoBlock = @"UPDATE Leg_De_Classi SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Class_D_Insert = @"INSERT INTO Leg_De_Classi (Codigo, Classificacao, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Class_D_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_De_Classi ORDER BY Codigo DESC";

        public static string Acao_Tipos_Only_Non_Blocked = @"SELECT * FROM Leg_Acs_Tipos WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Acao_All = @"SELECT * FROM Leg_Acs_Tipos ORDER BY Codigo";
        public static string Acao_Tipos_Block_NoBlock = @"UPDATE Leg_Acs_Tipos SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Acao_Insert = @"INSERT INTO Leg_Acs_Tipos (Codigo, Acao, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Acao_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_Acs_Tipos ORDER BY Codigo DESC";

        public static string Autor_Only_Non_Blocked = @"SELECT * FROM Leg_Autor WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Autor_All = @"SELECT * FROM Leg_Autor ORDER BY Codigo";
        public static string Autor_Block_NoBlock = @"UPDATE Leg_Autor SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Autor_Insert = @"INSERT INTO Leg_Autor (Codigo, Autor, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Autor_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_Autor ORDER BY Codigo DESC";

        public static string Origem_Only_Non_Blocked = @"SELECT * FROM Leg_Origem WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Origem_All = @"SELECT * FROM Leg_Origem ORDER BY Codigo";
        public static string Origem_Block_NoBlock = @"UPDATE Leg_Origem SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Origem_Insert = @"INSERT INTO Leg_Origem (Codigo, Origem, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Origem_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_Origem ORDER BY Codigo DESC";

        public static string Situacao_Only_Non_Blocked = @"SELECT * FROM Leg_Situacao WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Situacao_All = @"SELECT * FROM Leg_Situacao ORDER BY Codigo";
        public static string Situacao_Block_NoBlock = @"UPDATE Leg_Situacao SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Situacao_Insert = @"INSERT INTO Leg_Situacao (Codigo, Situacao, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Situacao_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_Situacao ORDER BY Codigo DESC";

        public static string Tipo_Only_Non_Blocked = @"SELECT * FROM Leg_Tipo WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Tipo_All = @"SELECT * FROM Leg_Tipo ORDER BY Codigo";
        public static string Tipo_Block_NoBlock = @"UPDATE [Leg_Tipo] SET [Bloqueado] = @Bloqueado WHERE ([Codigo] = @Codigo)";
        public static string Tipo_Inset = @"INSERT INTO Leg_Tipo (Codigo, Tipo, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Nome, @Cadastro, @Alterado, @Bloqueado)";
        public static string Tipo_Last_Codigo = @"SELECT TOP 1 Codigo FROM Leg_Tipo ORDER BY Codigo DESC";

        public static string SelectSimples = @"SELECT * FROM Legislacao WHERE (Tipo LIKE @Tipo) AND (Numero = @Numero) AND (Complemento LIKE @Complemento) AND (Excluido = 0) ORDER BY Data DESC, Numero DESC";
        public static string SelectSemFiltro = @"SELECT * FROM Legislacao WHERE (Excluido = 0) ORDER BY Data DESC, Numero DESC";
        public static string SelectDetalhado = @"SELECT  * FROM            Legislacao
WHERE        (Tipo LIKE @Tipo) AND (Data BETWEEN @Data1 AND @Data2) AND (Resumo LIKE '%' +  @Resumo + '%') AND (Classificado LIKE @Classificado) AND (Situacao LIKE @Situacao) 
AND (Origem LIKE @Origem) AND (Autor LIKE '%' + @Autor + '%') AND (Excluido = 0)
ORDER BY Data DESC, Numero DESC";
        public static string AcoesSemFiltro = @"SELECT * FROM  Leg_Acoes";
        public static string AcoesRecebidas = @"SELECT        TipoOrigem, NumeroOrigem, ComplementoOrigem, DataOrigem, Acao
FROM            Leg_Acoes
WHERE (TipoAlvo = @Tipo) AND (NumeroAlvo = @Numero) AND  (ComplementoAlvo = @Comp)";
    }
}
