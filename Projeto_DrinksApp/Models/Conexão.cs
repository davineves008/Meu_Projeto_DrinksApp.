using System.Data.SqlClient;

public class Conexao
{
    public static string stringConexao = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";

    public static SqlConnection GetConnection()
    {
        try
        {
            // Retorna uma nova instância da conexão
            return new SqlConnection(stringConexao);
        }
        catch (Exception ex)
        {
            // Log do erro se necessário
            throw new Exception("Erro ao configurar a conexão: " + ex.Message);
        }
    }
}
