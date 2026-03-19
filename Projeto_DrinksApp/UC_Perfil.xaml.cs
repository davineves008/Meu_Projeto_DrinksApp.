using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Projeto_DrinksApp
{
    /// <summary>
    /// Interação lógica para UC_Perfil.xam
    /// </summary>
    public partial class UC_Perfil : UserControl
    {
        public UC_Perfil()
        {
            InitializeComponent();
            CarregarDadosDoUsuario();
          
        }
        //btn Salvar as alterações editadas.
        private void btnSalvarPerfil_Click(object sender, RoutedEventArgs e)
        {
            // 1. Coletando os dados da tela
            string nomeEditado = txtNome.Text;
            string emailEditado = txtEmail.Text;

            if (App.ClienteLogado != null)
            {
                ClienteRepositorio repo = new ClienteRepositorio();

                // 2. Tenta salvar no Banco de Dados
                bool sucesso = repo.AtualizarCliente(App.ClienteLogado.IdCliente, nomeEditado, emailEditado);

                if (sucesso)
                {
                    // 3. MUITO IMPORTANTE: Atualiza a memória do App
                    // Se não fizer isso, o PIN ou outras telas ainda usarão o nome/email antigo
                    App.ClienteLogado.Nome = nomeEditado;
                    App.ClienteLogado.Email = emailEditado;

                    MessageBox.Show("Perfil atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void CarregarDadosDoUsuario()
        {
            // Verifica se existe alguém logado para não dar erro de referência nula
            if (App.ClienteLogado != null)
            {
                txtNome.Text = App.ClienteLogado.Nome;
                txtEmail.Text = App.ClienteLogado.Email;


                // Como você mencionou que tem a classe Endereço ligada:
                if (App.ClienteLogado.EnderecoResidencial != null)
                {
                    txtEndereco.Text = App.ClienteLogado.EnderecoResidencial.Logradouro;
                    txtCidade.Text = App.ClienteLogado.EnderecoResidencial.Cidade;
                }
                // Preenche a área de "Último Pedido Realizado"
                if (!string.IsNullOrEmpty(App.ClienteLogado.UltimoPedidoDescricao))
                {
                    lblUltimoPedido.Text = App.ClienteLogado.UltimoPedidoDescricao;

                    // Supondo que você tenha esses nomes no seu XAML:
                    txtDataPedido.Text = $"Entregue em: {App.ClienteLogado.UltimoPedidoData?.ToString("dd/MM/yyyy")}";
                    txtValorPedido.Text = App.ClienteLogado.UltimoPedidoValor.ToString("C2"); // Formato R$
                }
                else
                {
                    lblUltimoPedido.Text = "Nenhum pedido realizado ainda.";
                }
            }
        }

    }
}
