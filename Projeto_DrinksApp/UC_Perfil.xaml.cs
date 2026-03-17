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
        //btn Salvar perfil
        private void btnSalvarPerfil_Click(object sender, RoutedEventArgs e)
        {
            // Coletando os dados editados
            string nome = txtNome.Text;
            string email = txtEmail.Text;
            string endereco = txtEndereco.Text;
            string cidade = txtCidade.Text;

            // Aqui você enviaria para o Banco de Dados ou salvaria no arquivo
            // Por enquanto, vamos dar um feedback para o usuário
            MessageBox.Show($"Dados de {nome} salvos com sucesso!\nEndereço: {endereco}, {cidade}",
                            "Perfil Atualizado",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
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
