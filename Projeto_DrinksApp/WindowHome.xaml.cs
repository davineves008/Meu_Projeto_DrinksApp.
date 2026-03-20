using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Projeto_DrinksApp.Models;
using System.Data.SqlClient;

namespace Projeto_DrinksApp
{
    public partial class WindowHome : Window
    {
        public static WindowHome Instancia;

        // AJUSTE AQUI: O construtor agora aceita o nome ou endereço
        public WindowHome(string enderecoCliente = "")
        {
            InitializeComponent();
            Instancia = this;

            // Se não passamos nada pelo parênteses, pegamos do App.ClienteLogado
            if (string.IsNullOrEmpty(enderecoCliente) && App.ClienteLogado != null)
            {
                if (App.ClienteLogado.EnderecoResidencial != null)
                {
                    enderecoCliente = $"{App.ClienteLogado.EnderecoResidencial.Logradouro}, {App.ClienteLogado.EnderecoResidencial.Numero}";
                }
                else
                {
                    // Se não tiver endereço, mostra o nome do usuário logado
                    enderecoCliente = App.ClienteLogado.Nome;
                }
            }

            // Aplica o texto final ao campo da tela
            Txt_Endereco.Text = enderecoCliente;

            AtualizarBadgeCarrinho();
        }

        // --- BOTÃO PERFIL (Abre a UC_Perfil) ---
        private void BtnPerfil_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Perfil();
        }

        // --- NAVEGAÇÃO ---
        private void BtnCerveja_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = new UC_Cervejas();
        private void BtnWhisky_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = new UC_Whisky();
        private void BtnLanches_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = new UC_Lanches();
        private void BtnVinhos_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = new UC_Vinhos();
        private void BtnHome_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = null;
        private void BtnPedidos_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = new UC_Pedidos();
        private void BtnConfig_Click(object sender, RoutedEventArgs e) => ConteudoPrincipal.Content = new UC_Config();

        // --- CARRINHO ---
        private void BtnAbrirCarrinho_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Carrinho();
            AtualizarBadgeCarrinho();
        }

        public void AtualizarBadgeCarrinho()
        {
            int totalItens = App.CarrinhoGlobal.Sum(p => p.Quantidade);
            Txt_ContadorCarrinho.Text = totalItens.ToString();
        }

        // --- SISTEMA ---
        private void BtnSair_Click(object sender, RoutedEventArgs e)
        {
            var Resultado = MessageBox.Show("Deseja realmente sair?", "Sair", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Resultado == MessageBoxResult.Yes)
            {
                MainWindow Login = new MainWindow();
                Login.Show();
                this.Close();
            }
        }

        // --- PESQUISA ---
        private void Txt_Pesquisa_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Txt_Pesquisa.Text == "Pesquisar bebidas ou lanches...")
            {
                Txt_Pesquisa.Text = "";
                Txt_Pesquisa.Foreground = Brushes.White;
            }
        }

        private void Txt_Pesquisa_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Txt_Pesquisa.Text))
            {
                Txt_Pesquisa.Text = "Pesquisar bebidas ou lanches...";
                Txt_Pesquisa.Foreground = Brushes.Gray;
            }
        }

        private void Txt_Pesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PopupSugestoes == null) return;
            string termo = Txt_Pesquisa.Text;

            if (!string.IsNullOrWhiteSpace(termo) && termo != "Pesquisar bebidas ou lanches..." && termo.Length >= 3)
            {
                ProdutoRepositorio repo = new ProdutoRepositorio();
                var resultados = repo.PesquisarProdutos(termo);
                if (resultados.Count > 0)
                {
                    ListSugestoes.ItemsSource = resultados;
                    PopupSugestoes.IsOpen = true;
                }
                else PopupSugestoes.IsOpen = false;
            }
            else PopupSugestoes.IsOpen = false;
        }

        private void ListSugestoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSugestoes.SelectedItem is Produto produtoSelecionado)
            {
                PopupSugestoes.IsOpen = false;
                string nome = produtoSelecionado.Nome.ToLower();

                if (nome.Contains("xis") || nome.Contains("burguer")) ConteudoPrincipal.Content = new UC_Lanches();
                else if (nome.Contains("brahma") || nome.Contains("chopp")) ConteudoPrincipal.Content = new UC_Cervejas();
                else if (nome.Contains("vinho")) ConteudoPrincipal.Content = new UC_Vinhos();
                else if (nome.Contains("whisky") || nome.Contains("jack")) ConteudoPrincipal.Content = new UC_Whisky();

                Txt_Pesquisa.Text = string.Empty;
                Keyboard.ClearFocus();
                ListSugestoes.SelectedItem = null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { new PedidoRepositorio().LimparPedidosAntigos(); } catch { }
        }
    }
}