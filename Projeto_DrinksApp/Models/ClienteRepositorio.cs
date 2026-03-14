using System.Data.SqlClient;
using Projeto_DrinksApp.Models;

public class ClienteRepositorio
{
    // MUDANÇA: Agora retorna a classe Clientes em vez de bool
    public Clientes Login(string usuario, string senha)
    {
        using (SqlConnection conn = Conexao.GetConnection())
        {
            try
            {
                conn.Open();
                // Fazemos o JOIN entre Clientes (c) e Endereços (e) usando o IdCliente
                string query = @"
    SELECT c.nome, e.Logradouro, e.Numero, e.Bairro, e.Cidade, e.Estado 
    FROM Clientes c
    LEFT JOIN Endereco e ON c.IdCliente = e.IdCliente
    WHERE c.Usuario = @Usuario AND c.Senha = @Senha";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Clientes cliente = new Clientes();
                            cliente.Nome = reader["nome"]?.ToString();

                            // Verificamos se existe logradouro no banco antes de criar o objeto de endereço
                            if (reader["Logradouro"] != DBNull.Value)
                            {
                                cliente.EnderecoResidencial = new Endereço
                                {
                                    Logradouro = reader["Logradouro"].ToString(),
                                    Numero = reader["Numero"].ToString(),
                                    Bairro = reader["Bairro"].ToString(),
                                    Cidade = reader["Cidade"].ToString(),
                                    Estado = reader["Estado"].ToString()
                                };
                            }
                            return cliente;
                        }
                    }
                }
            }
            catch (Exception) { return null; }
            return null;
        }
    }

    // Método de cadastro permanece igual, apenas verifique se a conexão está abrindo corretamente
    public bool CadastrarCompleto(Clientes cliente)
    {
        using (SqlConnection conn = Conexao.GetConnection())
        {
            conn.Open();
            SqlTransaction transacao = conn.BeginTransaction();

            try
            {
                // 1. Inserir o Cliente e pegar o ID gerado
                string queryCliente = @"
                INSERT INTO Clientes (nome, email, cidade, cpf, Senha, Usuario) 
                VALUES (@nome, @email, @cidade, @cpf, @Senha, @Usuario);
                SELECT SCOPE_IDENTITY();"; // Pega o ID gerado agora

                int novoIdCliente;
                using (SqlCommand cmd = new SqlCommand(queryCliente, conn, transacao))
                {
                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@email", cliente.Email);
                    cmd.Parameters.AddWithValue("@cidade", cliente.EnderecoResidencial.Cidade);
                    cmd.Parameters.AddWithValue("@cpf", cliente.CPF);
                    cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
                    cmd.Parameters.AddWithValue("@Usuario", cliente.Usuario);

                    novoIdCliente = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Inserir o Endereço usando o ID recuperado
                string queryEndereco = @"
                INSERT INTO Endereco (idcliente, logradouro, numero, complemento, bairro, cidade, estado, cep)
                VALUES (@idid, @log, @num, @comp, @bair, @cid, @est, @cep)";

                using (SqlCommand cmdEnd = new SqlCommand(queryEndereco, conn, transacao))
                {
                    var e = cliente.EnderecoResidencial;
                    cmdEnd.Parameters.AddWithValue("@idid", novoIdCliente);
                    cmdEnd.Parameters.AddWithValue("@log", e.Logradouro);
                    cmdEnd.Parameters.AddWithValue("@num", e.Numero);
                    cmdEnd.Parameters.AddWithValue("@comp", e.Complemento ?? (object)DBNull.Value);
                    cmdEnd.Parameters.AddWithValue("@bair", e.Bairro);
                    cmdEnd.Parameters.AddWithValue("@cid", e.Cidade);
                    cmdEnd.Parameters.AddWithValue("@est", e.Estado);
                    cmdEnd.Parameters.AddWithValue("@cep", e.Cep);

                    cmdEnd.ExecuteNonQuery();
                }

                transacao.Commit(); // Salva tudo no banco
                return true;
            }
            catch (Exception ex)
            {
                transacao.Rollback(); // Cancela tudo se der erro
                System.Windows.MessageBox.Show("Erro ao cadastrar: " + ex.Message);
                return false;
            }
        }
    }
}