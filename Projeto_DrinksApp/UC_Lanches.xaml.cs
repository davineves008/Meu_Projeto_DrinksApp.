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
    /// Interação lógica para UC_Lanches.xam
    /// </summary>
    public partial class UC_Lanches : UserControl
    {
        public UC_Lanches()
        {
            InitializeComponent();
        }
        //Btn de comprar da tela de lanches;
        private void BtnComprar_Click(object sender, RoutedEventArgs e)
        {
            var botao = sender as Button;

            if (botao != null)
            {
                // Criamos um objeto temporário com os dados que você colocou no botão
                Produto produtoSelecionado = new Produto
                {
                    Nome = botao.Tag.ToString(),
                    // Convertemos o Uid (preço) de string para decimal
                    Preco = decimal.Parse(botao.Uid, System.Globalization.CultureInfo.InvariantCulture),
                    Quantidade = 1
                };

                // Adiciona ao carrinho global
                App.AdicionarAoCarrinho(produtoSelecionado);

                // Atualiza a bolinha (Badge) na WindowHome
                var windowPrincipal = Application.Current.MainWindow as WindowHome;
                if (windowPrincipal != null)
                {
                    windowPrincipal.AtualizarBadgeCarrinho();
                }

                MessageBox.Show($"{produtoSelecionado.Nome} adicionado ao carrinho!");
            }
        }
    }
}
