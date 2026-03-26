using Projeto_DrinksApp.Models;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;

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
                // 1. ADICIONADO: c.nivel no SELECT
                string query = @"
            SELECT c.IdCliente, c.nome, c.Senha, c.Email, c.nivel, 
                   e.Logradouro, e.Numero, e.Bairro, e.Cidade, e.Estado 
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

                            cliente.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                            cliente.Nome = reader["nome"]?.ToString();
                            cliente.Senha = reader["Senha"]?.ToString();
                            cliente.Email = reader["Email"]?.ToString();

                            // 2. IMPORTANTE: Captura o nível (0 ou 1) para decidir a tela depois
                            cliente.Nivel = reader["nivel"] != DBNull.Value ? Convert.ToInt32(reader["nivel"]) : 0;

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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erro no Login: " + ex.Message);
                return null;
            }
            return null;
        }
    }
    // Método de cadastro permanece igual, apenas verifique se a conexão está abrindo corretamente
    public void FinalizarCadastro(Clientes cliente, Endereço endereco)
    {
        string connString = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";

        using (SqlConnection con = new SqlConnection(connString))
        {
            con.Open();
            SqlTransaction transacao = con.BeginTransaction();

            try
            {
                // PASSO 1: Inserir o Cliente e pegar o ID gerado
                string sqlCliente = @"INSERT INTO Clientes (nome, email, cidade, cpf, Senha, Usuario, nivel) 
                                 OUTPUT INSERTED.idcliente 
                                 VALUES (@nome, @email, @cidade, @cpf, @senha, @usuario, @nivel)";

                SqlCommand cmdCliente = new SqlCommand(sqlCliente, con, transacao);
                cmdCliente.Parameters.AddWithValue("@nome", cliente.Nome);
                cmdCliente.Parameters.AddWithValue("@email", cliente.Email);
                cmdCliente.Parameters.AddWithValue("@cidade", cliente.Cidade); // CORRIGIDO: Adicionado .Cidade
                cmdCliente.Parameters.AddWithValue("@cpf", cliente.CPF);
                cmdCliente.Parameters.AddWithValue("@senha", cliente.Senha);
                cmdCliente.Parameters.AddWithValue("@usuario", cliente.Usuario); // CORRIGIDO: de Login para Usuario
                cmdCliente.Parameters.AddWithValue("@nivel", cliente.Nivel);

                int idGerado = (int)cmdCliente.ExecuteScalar();

                // PASSO 2: Inserir o Endereço vinculado ao ID do cliente
                string sqlEndereco = @"INSERT INTO Endereco (idcliente, logradouro, numero, bairro, cidade, estado, cep) 
                                 VALUES (@idid, @rua, @num, @bairro, @cidade, @estado, @cep)";

                SqlCommand cmdEnd = new SqlCommand(sqlEndereco, con, transacao);
                cmdEnd.Parameters.AddWithValue("@idid", idGerado);
                cmdEnd.Parameters.AddWithValue("@rua", endereco.Logradouro);
                cmdEnd.Parameters.AddWithValue("@num", endereco.Numero);
                cmdEnd.Parameters.AddWithValue("@bairro", endereco.Bairro);
                cmdEnd.Parameters.AddWithValue("@cidade", endereco.Cidade);
                cmdEnd.Parameters.AddWithValue("@estado", endereco.Estado);
                cmdEnd.Parameters.AddWithValue("@cep", endereco.Cep);

                cmdEnd.ExecuteNonQuery();

                transacao.Commit();
                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso");
            }
            catch (Exception ex)
            {
                // Se algo der errado no Passo 1 ou 2, ele cancela os dois para não criar dados órfãos
                if (transacao.Connection != null) transacao.Rollback();
                MessageBox.Show("Erro ao salvar no banco: " + ex.Message, "Erro técnico");
            }
        }
    }

    //quando edito os dados pela tela de perfil ele atualiza no banco de dados.
    public bool AtualizarCliente(int id, string nome, string email)
    {
        using (SqlConnection conn = Conexao.GetConnection())
        {
            try
            {
                conn.Open();
                string query = "UPDATE Clientes SET nome = @nome, Email = @email WHERE IdCliente = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar banco: " + ex.Message);
                return false;
            }
        }
    }

    //Metodo pra excluir o cliente pelo id.

    public bool ExcluirConta(int idCliente)
    {
        using (SqlConnection conn = Conexao.GetConnection())
        {
            try
            {
                conn.Open();

                // A ORDEM IMPORTA: 
                // 1º Deleta os Pedido (tabela dependente)
                // 2º Deleta o Endereço (tabela dependente)
                // 3º Deleta o Cliente (tabela principal)
                string sql = @"
                DELETE FROM Pedido WHERE IdCliente = @id;
                DELETE FROM Endereco WHERE IdCliente = @id;
                DELETE FROM Clientes WHERE IdCliente = @id;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idCliente);

                    // O ExecuteNonQuery retorna o número de linhas afetadas.
                    // Como deletamos de várias tabelas, ele deve ser maior que 0.
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erro ao limpar dados e excluir conta: " + ex.Message);
                return false;
            }
        }
    }
    // Metodo pra atualizar a senha pela UC_Segurança.
    public bool AtualizarSenha(int id, string novaSenha)
    {
        using (SqlConnection conn = Conexao.GetConnection())
        {
            try
            {
                conn.Open();
                string query = "UPDATE Clientes SET Senha = @senha WHERE IdCliente = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@senha", novaSenha);
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar senha: " + ex.Message);
                return false;
            }
        }
    }

    //Metodo que verifica no banco se o clinte ja existe;
    public bool ExisteCliente(string nome)
    {


        string stringConexao = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";

        try
        {
            using (SqlConnection conn = new SqlConnection(stringConexao)) // Use a sua variável de conexão aqui
            {
                conn.Open();

                // A query busca apenas o COUNT para ser mais leve e rápida
                string query = "SELECT COUNT(*) FROM clientes WHERE nome = @nome";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // O .Trim() remove espaços vazios acidentais no início ou fim do nome
                    cmd.Parameters.AddWithValue("@nome", nome.Trim());

                    // ExecuteScalar retorna a primeira coluna da primeira linha (o resultado do COUNT)
                    int resultado = Convert.ToInt32(cmd.ExecuteScalar());

                    // Se o resultado for maior que 0, o cliente já existe
                    return resultado > 0;
                }
            }
        }
        catch (Exception ex)
        {
            // Caso ocorra um erro de conexão, você pode logar o erro aqui
            throw new Exception("Erro ao verificar duplicidade de cliente: " + ex.Message);
        }
    }

    //Meto que verifica se o cpf tem 11 digitos.
    public bool ValidarFormatoCPF(string cpf)
    {
        // Remove qualquer máscara (pontos ou traços) que o usuário possa ter digitado
        string apenasNumeros = Regex.Replace(cpf, @"[^\d]", "");

        // Verifica se tem exatamente 11 dígitos
        return apenasNumeros.Length == 11;
    }
}