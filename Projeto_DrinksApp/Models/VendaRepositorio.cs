using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_DrinksApp.Models
{
    internal class VendaRepositorio
    {

        //Salva no banco de dados;
        public bool SalvarVenda(int idCliente, int qtdItens, decimal total)
        {
            string strCon = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    // REMOVIDO: idvenda (o SQL cuida dele) e a vírgula extra
                    // AJUSTADO: nome das colunas para bater com a sua nova tabela (dataehora, itens, valortotal)
                    string sql = @"INSERT INTO Vendas (idcliente, dataehora, itens, valortotal) 
                           VALUES (@idCli, @data, @itens, @total)";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    // Passando os parâmetros corretamente
                    cmd.Parameters.AddWithValue("@idCli", idCliente);
                    cmd.Parameters.AddWithValue("@data", DateTime.Now);
                    cmd.Parameters.AddWithValue("@itens", qtdItens); // Agora passando a quantidade (int)
                    cmd.Parameters.AddWithValue("@total", total);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar venda: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
