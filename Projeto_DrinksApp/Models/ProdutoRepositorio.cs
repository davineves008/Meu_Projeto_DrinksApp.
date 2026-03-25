using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_DrinksApp.Models
{
    class ProdutoRepositorio
    {
        // Mesma string de conexão que usamos para limpar os pedidos
        private string strCon = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;TrustServerCertificate=True";

        public List<Produto> PesquisarProdutos(string termo)
        {
            List<Produto> lista = new List<Produto>();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                // Pesquisa pelo nome do produto (nomedoproduto)
                string sql = "SELECT * FROM produtos WHERE nomedoproduto LIKE @termo";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Produto
                    {
                        // Nomes das colunas conforme sua imagem
                        IdProduto = (int)reader["idproduto"],
                        Nome = reader["nomedoproduto"].ToString(),
                        Preco = Convert.ToDecimal(reader["precounitario"]),
                        Estoque = (int)reader["estoque"]
                    });
                }
            }
            return lista;
        }

        public void BaixarEstoque(int produtoId, int quantidadeVendida)
        {
            string strCon = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;TrustServerCertificate=True";

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                // AJUSTADO: 'estoque' e 'idproduto' conforme sua imagem aabeb6
                string sql = "UPDATE Produtos SET estoque = estoque - @qtd WHERE idproduto = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@qtd", quantidadeVendida);
                cmd.Parameters.AddWithValue("@id", produtoId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //Cadastro de produto;
        public void CadastrarProduto(Produto produto)
        {

         
            // Lembre-se de usar sua string de conexão correta aqui
            using (SqlConnection conn = new SqlConnection(Conexao.stringConexao))
            {
                conn.Open();
                // NOMES REAIS DAS COLUNAS: nomedoproduto, idtipo, precounitario, estoque
                string sql = "INSERT INTO Produtos (nomedoproduto, idtipo, precounitario, estoque) " +
                             "VALUES (@nome, @tipo, @preco, @estoque)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", produto.Nome);
                    cmd.Parameters.AddWithValue("@tipo", produto.IdTipo);
                    cmd.Parameters.AddWithValue("@preco", produto.Preco);
                    cmd.Parameters.AddWithValue("@estoque", produto.Estoque);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        
    }
}
