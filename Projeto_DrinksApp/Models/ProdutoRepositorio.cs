using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void ExcluirProdutoDoBanco(int produtoId)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                // Query para deletar o produto pelo ID
                string sql = "DELETE FROM Produtos WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", produtoId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
