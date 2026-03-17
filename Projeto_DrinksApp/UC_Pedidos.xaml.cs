using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Projeto_DrinksApp.Models.Pedido;
using System;
using System.Collections.Generic;

using System.Data;
using Projeto_DrinksApp.Models; // Isso faz o C# encontrar a classe Pedido

namespace Projeto_DrinksApp
{
    /// <summary>
    /// Interação lógica para UC_Pedidos.xam
    /// </summary>
    public partial class UC_Pedidos : UserControl
    {
        public UC_Pedidos()
        {
            InitializeComponent();

            // Forçamos a atualização assim que a tela abre
            CarregarLista();
        }

        public void CarregarLista()
        {
            try
            {
                // 1. Criar o repositório (Isso resolve a ondinha vermelha no 'repo')
                PedidoRepositorio repo = new PedidoRepositorio();

                // 2. Buscar os dados do banco usando o ID do cliente logado
                // Verifique se na sua classe Cliente a propriedade é 'Id' ou 'idcliente'
                var listaDePedidos = repo.ListarPedidosPorCliente(App.ClienteLogado.IdCliente);

                // 3. Vincula ao DataGrid
                dgpedidos.ItemsSource = null; // Limpa o que tinha antes
                dgpedidos.ItemsSource = listaDePedidos; // Exibe os pedidos do banco
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message);
            }
        }


        private void dgpedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
