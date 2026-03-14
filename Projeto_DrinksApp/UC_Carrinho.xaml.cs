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
    /// Interação lógica para UC_Carrinho.xam
    /// </summary>
    public partial class UC_Carrinho : UserControl
    {
        public UC_Carrinho()
        {
            InitializeComponent();
            ListaCarrinho.ItemsSource = App.CarrinhoGlobal;

            AtualizarTotalGeral();
        }

        //Btn pra adicionar produtos no carrinho; 
        // Dentro do método que aumenta a quantidade
        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            var produto = (sender as Button).DataContext as Produto;
            if (produto != null)
            {
                produto.Quantidade++; // Aumenta a quantidade

                AtualizarTotalGeral(); // Atualiza o total do carrinho

                // AQUI ENTRA O CÓDIGO:
                var principal = Application.Current.MainWindow as WindowHome;
                if (principal != null)
                {
                    principal.AtualizarBadgeCarrinho();
                }
            }
        }

        //btn pra remover o produto do carrinho;
        public void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var produto = botao.DataContext as Produto;

            if (produto != null)
            {
                if (produto.Quantidade > 1)
                {
                    produto.Quantidade--; // O OnPropertyChanged vai atualizar o texto da quantidade sozinho!
                }
                else
                {
                    App.CarrinhoGlobal.Remove(produto);
                }
                AtualizarTotalGeral();

                // Se quiser atualizar a bolinha vermelha aqui também:
                // No UC_Carrinho.xaml.cs
                var principal = Application.Current.MainWindow as WindowHome;
                if (principal != null)
                {
                    principal.AtualizarBadgeCarrinho();
                }
            }
        }
                
        

        //metodo pra atualizar geral o carrinho;
        public void AtualizarTotalGeral()
        {
            decimal total = App.CarrinhoGlobal.Sum(P => P.PrecoTotal);
            Txt_TotalGeral.Text = string.Format("R$ {0:f2}", total);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
