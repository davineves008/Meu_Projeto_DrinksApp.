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
            CarregarPerfil();
          
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
        private void CarregarPerfil()
        {
            if (App.ClienteLogado != null)
            {
                // Preenche campos de texto (Garante que não apareça o nome da variável como na imagem a8045a)
                txtNome.Text = App.ClienteLogado.Nome;
                txtEmail.Text = App.ClienteLogado.Email;

                if (App.ClienteLogado.EnderecoResidencial != null)
                {
                    txtEndereco.Text = App.ClienteLogado.EnderecoResidencial.Logradouro;
                    txtCidade.Text = App.ClienteLogado.EnderecoResidencial.Cidade;
                }

                // Lógica de Nível e Visibilidade do Botão ADM
                if (App.ClienteLogado.Nivel == 1)
                {
                    txtNivelUsuario.Text = "ADMINISTRADOR";
                    borderNivel.Background = new SolidColorBrush(Color.FromArgb(40, 255, 215, 0)); // Dourado suave
                    borderNivel.BorderBrush = Brushes.Gold;

                    // Torna o botão de Painel visível apenas para ADM
                    btnAdminArea.Visibility = Visibility.Visible;
                }
                else
                {
                    txtNivelUsuario.Text = "CLIENTE";
                    borderNivel.Background = new SolidColorBrush(Color.FromArgb(40, 0, 249, 255)); // Ciano suave
                    borderNivel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 249, 255));

                    btnAdminArea.Visibility = Visibility.Collapsed;
                }

                // Dados do Último Pedido
                lblUltimoPedido.Text = App.ClienteLogado.UltimoPedidoDescricao ?? "Nenhum pedido recente";
                txtValorPedido.Text = App.ClienteLogado.UltimoPedidoValor.ToString("C2");
                txtDataPedido.Text = "Última atualização: " + DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        private void btnAdminArea_Click(object sender, RoutedEventArgs e)
        {
            // Abre a tela de Administrador passando o nome do ADM logado
            WindowHome admWin = new WindowHome(App.ClienteLogado.Nome);
            admWin.Show();
        }
    }
}
