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

        //Metodo que carrega o perfil.
        private void CarregarPerfil()
        {
            if (App.ClienteLogado != null)
            {
                // 1. Preenchimento de Dados Básicos
                txtNome.Text = App.ClienteLogado.Nome;
                txtEmail.Text = App.ClienteLogado.Email;

                // 2. Lógica do Endereço (Garante exibição para usuário comum)
                if (App.ClienteLogado.EnderecoResidencial != null)
                {
                    // Verifica se as propriedades não são nulas antes de montar a string
                    string logradouro = App.ClienteLogado.EnderecoResidencial.Logradouro ?? "Rua não informada";
                    string numero = App.ClienteLogado.EnderecoResidencial.Numero ?? "S/N";

                    txtEndereco.Text = $"{logradouro}, {numero}";
                    txtCidade.Text = App.ClienteLogado.EnderecoResidencial.Cidade ?? "Cidade não informada";
                }
                else
                {
                    txtEndereco.Text = "Endereço não cadastrado";
                    txtCidade.Text = "---";
                }

                // 3. Lógica do Nível (Comparação correta de int com int)
                if (App.ClienteLogado.Nivel == 1)
                {
                    txtNivelUsuario.Text = "ADMINISTRADOR";
                    btnAdminArea.Visibility = Visibility.Visible;
                    if (borderNivel != null) borderNivel.BorderBrush = Brushes.Gold;
                }
                else
                {
                    txtNivelUsuario.Text = "CLIENTE";
                    btnAdminArea.Visibility = Visibility.Collapsed;
                    if (borderNivel != null) borderNivel.BorderBrush = Brushes.Gray;
                }

                // 4. Busca do Último Pedido Realizado
                try
                {
                    Models.PedidoRepositorio repo = new Models.PedidoRepositorio();

                    // Busca a lista usando o ID do cliente logado
                    var pedidos = repo.ListarPedidosPorCliente(App.ClienteLogado.IdCliente);

                    if (pedidos != null && pedidos.Count > 0)
                    {
                        // O primeiro da lista é o mais recente devido ao ORDER BY DESC no SQL
                        var ultimo = pedidos[0];

                        lblUltimoPedido.Text = $"Pedido #{ultimo.IdPedido} - {ultimo.Observacoes}";
                        txtDataPedido.Text = $"Entregue em: {ultimo.DataPedido:dd/MM/yyyy}";
                        txtValorPedido.Text = $" {ultimo.ValorTotal:c2}";
                    }
                    else
                    {
                        lblUltimoPedido.Text = "Nenhum pedido recente";
                        txtDataPedido.Text = "Entregue em: --/--/----";
                    }
                }
                catch (Exception)
                {
                    lblUltimoPedido.Text = "Erro ao carregar histórico";
                    txtDataPedido.Text = "Entregue em: --/--/----";
                }
            }
        }
        private void btnAdminArea_Click(object sender, RoutedEventArgs e)
        {
            WindowHome admWin = new WindowHome(App.ClienteLogado.Nome);
            admWin.Show();

            Window.GetWindow(this)?.Close();
        }

        //Btn voltar a tela de config.
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            // Acessa a instância da WindowHome e carrega novamente a UC_Config
            if (WindowHome.Instancia != null)
            {
                WindowHome.Instancia.ConteudoPrincipal.Content = new UC_Config();
            }
        }
    }
}
