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
        public void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var Produto = botao.DataContext as Produto;

            if (Produto != null)
            {
                Produto.Quantidade++;
                AtualizarTotalGeral();
            }
        }

        //btn pra remover o produto do carrinho;
        public void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;
            var Produto = botao.DataContext as Produto;

            if(Produto != null)
            {
                if (Produto.Quantidade > 1)
                {
                    Produto.Quantidade--;
                }
                else
                {
                    //caso tenha 1 produto e clico em retirar ele limpa a lista;
                    App.CarrinhoGlobal.Remove(Produto);
                }
                AtualizarTotalGeral();
            }
                
        }

        //metodo pra atualizar geral o carrinho;
        public void AtualizarTotalGeral()
        {
            decimal total = App.CarrinhoGlobal.Sum(P => P.PrecoTotal);
            Txt_TotalGeral.Text = string.Format("R$ {0:f2}", total);
        }


    }
}
