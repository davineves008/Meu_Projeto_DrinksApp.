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
    /// Interação lógica para UC_Cervejas.xam
    /// </summary>
    public partial class UC_Cervejas : UserControl
    {
        //Variavel pra contar os itens;
        private int TotalItens = 0;

        public UC_Cervejas()
        {
            InitializeComponent();
        }
        //metodo pra comprar o produto;
        private void BtnComprar_Click(object sender, RoutedEventArgs e)
        {
            // O 'sender' é o botão exato que o usuário clicou
            var botao = sender as Button;

            if (botao != null && botao.Tag != null)
            {
                try
                {
                    // Criamos o produto pegando o NOME do Tag e o PREÇO do Uid do botão clicado
                    Produto produtoSelecionado = new Produto
                    {
                        Nome = botao.Tag.ToString(),
                        // Lê o preço (ex: 8.50 ou 140.00) que está no Uid do XAML
                        Preco = decimal.Parse(botao.Uid, System.Globalization.CultureInfo.InvariantCulture),
                        Quantidade = 1
                    };

                    // Adiciona ao carrinho global (aquela lista que criamos no App.xaml.cs)
                    App.AdicionarAoCarrinho(produtoSelecionado);

                    // Atualiza a bolinha azul/vermelha na tela principal
                    if (Application.Current.MainWindow is WindowHome principal)
                    {
                        principal.AtualizarBadgeCarrinho();
                    }

                    // Agora o MessageBox vai mostrar o nome real: "Brahma adicionada", "Jack Daniels adicionado", etc.
                    MessageBox.Show($"{produtoSelecionado.Nome} adicionado ao carrinho!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro técnico ao ler o preço do botão: " + ex.Message);
                }
            }
        }
    }
    }
