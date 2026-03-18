using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DrinksApp.Models
{
    internal class VendaRepositorio
    {
        public void RegistrarVenda(int idProduto, int idCliente, decimal valorTotal, DateTime dataHora)
        {
            using (SqlConnection conn = new SqlConnection(App.LinhaConexao))
            {
                // Adicionamos a coluna Datahora no INSERT
                string sql = @"INSERT INTO vendas (idProduto, idcliente, Valortotal, Datahora) 
                       VALUES (@idProd, @idCli, @valor, @data)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@idProd", idProduto);
                cmd.Parameters.AddWithValue("@idCli", idCliente);
                cmd.Parameters.AddWithValue("@valor", valorTotal);
                cmd.Parameters.AddWithValue("@data", dataHora); // Passando a data e hora do C#

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
