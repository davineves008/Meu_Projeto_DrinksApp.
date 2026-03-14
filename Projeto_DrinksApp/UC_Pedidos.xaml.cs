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
    /// Interação lógica para UC_Pedidos.xam
    /// </summary>
    public partial class UC_Pedidos : UserControl
    {
        public UC_Pedidos()
        {
            InitializeComponent();

            // Forçamos a atualização assim que a tela abre
            CarregarLista();
        }

        public void CarregarLista()
        {
            // Vincula à lista global que tem as bebidas e lanches
            dgpedidos.ItemsSource = null; // Limpa para garantir
            dgpedidos.ItemsSource = App.CarrinhoGlobal;
        }
    }
}
