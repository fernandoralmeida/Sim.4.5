
namespace Sim.Sec.Governo.Portarias.Sql
{
    public static class SqlCollections
    {
        public static string InsertQuery = @"INSERT INTO [Portarias] ([Numero], [Data], [Tipo], [Resumo], [Servidor], [Link], [Publicado], [Cadastro], [Atualizado], [Excluido]) VALUES (@Numero, @Data, @Tipo, @Resumo, @Servidor, @Link, @Publicado, @Cadastro, @Atualizado, @Excluido)";
        public static string SelectNumberData = @"SELECT Indice, Numero, Data, Tipo, Resumo, Servidor, Link, Publicado, Cadastro, Atualizado, Excluido FROM Portarias WHERE (Excluido = 0) AND (Numero = @Numero) AND (Data = @Data)";
        public static string UpdateQuery = @"UPDATE Portarias SET Numero = @Numero, Data = @Data, Tipo = @Tipo, Resumo = @Resumo, Servidor = @Servidor, Link = @Link, Publicado = @Publicado, Cadastro = @Cadastro, Atualizado = @Atualizado, Excluido = @Excluido WHERE (Indice = @Original_Indice)";
        public static string InsertNewServidor = @"INSERT INTO [Por_Srv_Nomes] ([Nome], [Inserido] , [Bloqueado]) VALUES (@Servidor, @Inserido, @Bloqueado)";
        public static string DeleteQuery = @"DELETE FROM [Portarias] WHERE (([Indice] = @Original_Indice))";
        public static string SelectParameters = @"SELECT * FROM Portarias WHERE (Excluido = 0) AND ((Data BETWEEN @Data1 AND @Data2) AND (Tipo LIKE @Tipo) AND (Resumo LIKE @Resumo) AND (Servidor LIKE @Nome)) ORDER BY Data, Numero";
        public static string SelectNumber = @"SELECT Indice, Numero, Data, Tipo, Resumo, Servidor, Link, Publicado, Cadastro, Atualizado, Excluido FROM Portarias WHERE (Excluido = 0) AND (Numero = @Numero) ORDER BY Data, Numero";
        public static string SelectIndice = @"SELECT * FROM Portarias WHERE (Excluido = 0) AND (Indice = @Indice)";
        public static string SelectServidores = @"SELECT Nome FROM Por_Srv_Nomes WHERE (Bloqueado = 0) ORDER BY Nome";
        public static string SelectServidorName = @"SELECT Nome FROM Por_Srv_Nomes WHERE (Bloqueado = 0) AND (Nome LIKE @Nome) ORDER BY Nome";

        public static string RelatoriesByParameters = @"SELECT * FROM Portarias WHERE (Excluido = 0) AND ((Data BETWEEN @Data1 AND @Data2) AND (Tipo LIKE @Tipo) AND (Servidor LIKE @Nome)) ORDER BY Data, Numero";

        public static string Classi_With_Blocked = @"SELECT * FROM Por_Classi ORDER BY Codigo";
        public static string Classi_Only_Non_Blocked = @"SELECT * FROM Por_Classi WHERE (Bloqueado = 0) ORDER BY Codigo";
        public static string Insert_Classi = @"INSERT INTO Por_Classi (Codigo, Classificacao, Cadastro, Alterado, Bloqueado) VALUES (@Codigo, @Classi, @Cadastro, @Alterado, @Bloqueado)";
        public static string Classi_Block_Non_Block = @"UPDATE Por_Classi SET Bloqueado = @Bloqueado WHERE (Codigo = @Codigo)";
        public static string Update_Classi = @"UPDATE Por_Classi SET Classificacao = @Classificacao WHERE (Codigo = @Codigo)";
        public static string Classi_Last_Row = @"SELECT TOP 1 Codigo FROM Por_Classi ORDER BY Codigo DESC";
    }
}
