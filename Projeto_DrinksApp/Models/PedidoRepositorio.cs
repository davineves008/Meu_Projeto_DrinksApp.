using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Projeto_DrinksApp.Models.Pedido;

namespace Projeto_DrinksApp.Models
{
    public class PedidoRepositorio
    {
        public List<Pedido> ListarPedidosPorCliente(int idCliente)
        {
            string stringConexao = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";


            List<Pedido> lista = new List<Pedido>();

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Certifique-se de que a query está completa (ORDER BY DESC)
                string sql = "SELECT idpedido, datapedido, valortotal, observacoes FROM Pedido WHERE idcliente = @id ORDER BY datapedido DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idCliente);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Pedido p = new Pedido();
                    p.IdPedido = Convert.ToInt32(dr["idpedido"]);
                    p.DataPedido = Convert.ToDateTime(dr["datapedido"]);
                    p.ValorTotal = Convert.ToDecimal(dr["valortotal"]);
                    p.Observacoes = dr["observacoes"].ToString();
                    lista.Add(p);
                }
            } // Fecha o using
            return lista;
        }


        //insere os pedidos no banco de dados;
        public void InserirPedido(Pedido pedido, int idCliente)
        {
            // Sua string de conexão que vimos na imagem anterior
            string strCon = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(strCon))
            {
                // O comando SQL para gravar na tabela que você criou
                string sql = "INSERT INTO Pedido (idcliente, datapedido, idstatus, valortotal, observacoes) " +
                             "VALUES (@idcliente, @data, @status, @total, @obs)";

                SqlCommand cmd = new SqlCommand(sql, con);

                // Passando os valores das variáveis para o SQL
                cmd.Parameters.AddWithValue("@idcliente", idCliente);
                cmd.Parameters.AddWithValue("@data", DateTime.Now); // Grava a data e hora atual
                cmd.Parameters.AddWithValue("@status", 1); // 1 pode ser o status "Recebido"
                cmd.Parameters.AddWithValue("@total", pedido.ValorTotal);
                cmd.Parameters.AddWithValue("@obs", pedido.Observacoes ?? "");

                con.Open();
                cmd.ExecuteNonQuery(); // Executa a gravação
            }
        }

        //metodo pra limpa os pedidos assim que a janela fechar;
        public void LimparPedidosAntigos()
        {
            string strCon = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(strCon))
            {
                try
                {
                    con.Open();
                    // A ordem aqui é fundamental:
                    // 1º Apaga os itens (tabela filha)
                    // 2º Apaga os pedidos (tabela pai)
                    string sql = "DELETE FROM ItensPedido; DELETE FROM Pedido;";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Isso vai te mostrar se ainda houver erro de permissão ou conexão
                    MessageBox.Show("Erro ao limpar: " + ex.Message);
                }
            }
        }
    }
}
