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
    /// Interação lógica para UC_Vinhos.xam
    /// </summary>
    public partial class UC_Vinhos : UserControl
    {
        public UC_Vinhos()
        {
            InitializeComponent();
        }
        //Btn Comprar tela de vinhos;
        private void BtnComprar_Click(object sender, RoutedEventArgs e)
        {
            // Supondo que você carregou os dados no Tag ou que o DataContext é o Produto
            var botao = sender as Button;

            // Se estiver usando o Binding no DataContext do botão:
            var produtoSelecionado = botao.DataContext as Produto;

            if (produtoSelecionado != null)
            {
                // CHAMADA GLOBAL:
                App.AdicionarAoCarrinho(produtoSelecionado);

                MessageBox.Show($"{produtoSelecionado.Nome} adicionado ao carrinho!");
            }
        }
    }
}
