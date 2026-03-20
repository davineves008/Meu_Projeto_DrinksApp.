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
                // 1. COLOCA O ENDEREÇO NO TOPO (No lugar do texto verde da imagem a8045a)
                if (App.ClienteLogado.EnderecoResidencial != null)
                {
                    // Exibe: Rua Tal, 123 - Cidade
                    txtEndereco.Text = $"{App.ClienteLogado.EnderecoResidencial.Logradouro}, {App.ClienteLogado.EnderecoResidencial.Numero}";
                }
                else
                {
                    txtEndereco.Text = "Endereço não cadastrado";
                }

                // 2. Preenche os campos do formulário normalmente
                txtNome.Text = App.ClienteLogado.Nome;
                txtEmail.Text = App.ClienteLogado.Email;
                txtCidade.Text = App.ClienteLogado.EnderecoResidencial?.Cidade;
                txtEndereco.Text = App.ClienteLogado.EnderecoResidencial?.Logradouro;

                // 3. Lógica do Nível (Badge e Botão ADM)
                if (App.ClienteLogado.Nivel == 1)
                {
                    txtNivelUsuario.Text = "ADMINISTRADOR";
                    btnAdminArea.Visibility = Visibility.Visible;
                    borderNivel.BorderBrush = Brushes.Gold;
                }
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
