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
using System.Windows.Shapes;
using Projeto_DrinksApp.Models;

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
            // Aqui no futuro você colocará a lógica de filtro
            // Ex: filtrar sua lista de produtos conforme o usuário digita
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

            // Opcional: Você pode esconder outros elementos da Row 1 (os botões de categoria)
            // se quiser que a tela de configuração use todo o espaço vertical da direita.
        }

    }

}
