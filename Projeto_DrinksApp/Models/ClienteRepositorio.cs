using System.Data.SqlClient;
using Projeto_DrinksApp.Models;

public class ClienteRepositorio
{
    //metodo pra logar os clientes;
    public bool Login(string email, string senha)
    {
        
        using (SqlConnection conn = Conexao.GetConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Clientes WHERE Usuario=@Usuario AND Senha=@Senha";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                 
                    cmd.Parameters.AddWithValue("@Usuario", email);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    int resultado = (int)cmd.ExecuteScalar();
                    return resultado > 0;
                }
            }
            catch (Exception ex)
            {
                // Trate o erro de conexão aqui (log, etc)
                return false;
            }
        }
    }

    //Metodo pra cadastrar os clientes;
    public bool Cadastrar(string nome, string email, string cidade, string cpf, string senha, string usuario)
    {
        using SqlConnection conn = Conexao.GetConnection(); // Corrigindo o "risco verde"

        // IMPORTANTE: A ordem das colunas aqui define onde cada @parametro vai entrar
        string query = "INSERT INTO Clientes (nome, email, cidade, cpf, Senha, Usuario) " +
                       "VALUES (@nome, @email, @cidade, @cpf, @Senha, @Usuario)";

        SqlCommand cmd = new SqlCommand(query, conn);

        // Vincule cada parâmetro na MESMA ORDEM da query acima
        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@cidade", cidade);
        cmd.Parameters.AddWithValue("@cpf", cpf);
        cmd.Parameters.AddWithValue("@Senha", senha);   // 'S' Maiúsculo
        cmd.Parameters.AddWithValue("@Usuario", usuario); // 'U' Maiúsculo

        conn.Open();
        return cmd.ExecuteNonQuery() > 0;
    }
}