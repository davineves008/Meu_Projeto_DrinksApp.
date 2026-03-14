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
            var botao = sender as Button;

            if (botao != null)
            {
                // Se o Uid estiver vazio, usamos 0.00 para não dar erro
                string precoTxt = string.IsNullOrEmpty(botao.Uid) ? "0.00" : botao.Uid;

                try
                {
                    Produto produtoSelecionado = new Produto
                    {
                        Nome = botao.Tag?.ToString() ?? "Produto sem nome",
                        Preco = decimal.Parse(precoTxt, System.Globalization.CultureInfo.InvariantCulture),
                        Quantidade = 1
                    };

                    App.AdicionarAoCarrinho(produtoSelecionado);

                    if (Application.Current.MainWindow is WindowHome principal)
                    {
                        principal.AtualizarBadgeCarrinho();
                    }

                    MessageBox.Show($"{produtoSelecionado.Nome} adicionado!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Verifique o Uid do botão {botao.Tag}: " + ex.Message);
                }
            }
        }
    }
}

