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
    /// Interação lógica para UC_Whisky.xam
    /// </summary>
    public partial class UC_Whisky : UserControl
    {
        public UC_Whisky()
        {
            InitializeComponent();
        }

        //Btn comprar da tela de Whisky;
        private void BtnComprar_Click(object sender, RoutedEventArgs e)
        {
            var produto = (sender as Button).DataContext as Produto;
            if (produto != null)
            {
                App.AdicionarAoCarrinho(produto);
            }
        }
    }
}
