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
    /// <summary>
    /// Lógica interna para WindowHome.xaml
    /// </summary>
    public partial class WindowHome : Window
    {
        //referencia estatica pra propria janela;
        public static WindowHome Instancia;
        public WindowHome(string enderecoCliente)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(enderecoCliente))
            {
                Txt_Endereco.Text = enderecoCliente;
            }
        }


        private void Btn_Perfil_Click(object sender, RoutedEventArgs e)
        {

        }
        //Btn tela de cervejas;
        private void BtnCerveja_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Cervejas();
        }
        //btn tela whisky;
        private void BtnWhisky_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Whisky();
        }
        //Btn tela de lanches;
        private void BtnLanches_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Lanches();
        }
        // Btn Tela principal;
        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = null;
        }
        //Btn Tela de vinhos;
        private void BtnVinhos_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Vinhos();
        }
        //btn pra sair e logar na tela de login;
        private void BtnSair_Click(object sender, RoutedEventArgs e)
        {
            var Resultado = MessageBox.Show("Deseja realmente sair e voltar a tela de login?", "sair", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(Resultado == MessageBoxResult.Yes )
            {
                MainWindow Login = new MainWindow();
                Login.Show();

                this.Close();
            }

        }
        //barra de pesquisa;
        private void Txt_Pesquisa_GotFocus(object sender, RoutedEventArgs e)
        {
            // Padronizei para o texto completo
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

        //Metodo de pesquisa 
        private void Txt_Pesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PopupSugestoes == null) return;

            string termo = Txt_Pesquisa.Text;

            // Verificação rigorosa para não pesquisar o placeholder
            if (!string.IsNullOrWhiteSpace(termo) &&
                termo != "Pesquisar bebidas ou lanches..." &&
                termo.Length >= 3)
            {
                ProdutoRepositorio repo = new ProdutoRepositorio();
                var resultados = repo.PesquisarProdutos(termo);

                if (resultados.Count > 0)
                {
                    ListSugestoes.ItemsSource = resultados;
                    PopupSugestoes.IsOpen = true;
                }
                else
                {
                    PopupSugestoes.IsOpen = false;
                }
            }
            else
            {
                PopupSugestoes.IsOpen = false;
            }
        }
        //Metodo pra ver detalhes do produto;
        private void ListSugestoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verifica se o item clicado não é nulo
            if (ListSugestoes.SelectedItem is Produto produtoSelecionado)
            {
                // 1. Fecha o popup e limpa a seleção para a próxima busca
                PopupSugestoes.IsOpen = false;

                // 2. Transforma o nome em minúsculo para facilitar a comparação
                string nome = produtoSelecionado.Nome.ToLower();

                // 3. Lógica de "GPS" do sistema
                if (nome.Contains("xis") || nome.Contains("burguer") || nome.Contains("lanche"))
                {
                    ConteudoPrincipal.Content = new UC_Lanches();
                }
                else if (nome.Contains("brahma") || nome.Contains("chopp")) 
                {
                    ConteudoPrincipal.Content = new UC_Cervejas();
                }
                else if (nome.Contains("vinho") || nome.Contains("tinto"))
                {
                    ConteudoPrincipal.Content = new UC_Vinhos();
                }
                else if (nome.Contains("jack") || nome.Contains("daniels") || nome.Contains("whisky"))
                {
                    ConteudoPrincipal.Content = new UC_Whisky();
                }

                // Limpa o texto da pesquisa (opcional)
                Txt_Pesquisa.Text = string.Empty;
                //Txt_Pesquisa.Foreground = Brushes.Gray;

                Keyboard.ClearFocus();

                // Tira o foco do ListBox para não ficar marcado
                ListSugestoes.SelectedItem = null;
            }
        }


        //Btn pra abrir a tela do carrinho;
        private void BtnAbrirCarrinho_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Carrinho();
            AtualizarBadgeCarrinho();

        }
        private void BtnPedidos_Click(object sender, RoutedEventArgs e)
        {
            ConteudoPrincipal.Content = new UC_Pedidos();
        }

        //Btn que abre a tela de perfil;
        private void BtnPerfil_Click(object sender, RoutedEventArgs e)
        {
            UC_Perfil telaPerfil = new UC_Perfil();

            ConteudoPrincipal.Content = telaPerfil;
        }

        //metodo pra atualiza bolinha carrinho;
        public void AtualizarBadgeCarrinho()
        {
            // Soma a quantidade de todos os itens na lista global
            int totalItens = App.CarrinhoGlobal.Sum(p => p.Quantidade);
            Txt_ContadorCarrinho.Text = totalItens.ToString();
        }
        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            // 1. Instancia a User Control de Interface
            UC_Interface telaConfig = new UC_Interface();

            // 2. Insere a UC dentro do ContentControl que você definiu no XAML
            ConteudoPrincipal.Content = new UC_Config();

           
        }

        //metodo que limpa a lista de pedidos assim que fechar o programa;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                PedidoRepositorio repo = new PedidoRepositorio();
                repo.LimparPedidosAntigos();
            }
            catch (Exception ex)
            {
                // Opcional: logar o erro caso a limpeza falhe ao fechar
            }
        }

    }

}
