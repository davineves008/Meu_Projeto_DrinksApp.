using System.Collections.ObjectModel;
using System.Linq; // Necessário para o FirstOrDefault
using System.Windows;

namespace Projeto_DrinksApp
{
    public partial class App : Application
    {
        // Lista global que armazena os itens selecionados
        public static ObservableCollection<Produto> CarrinhoGlobal = new ObservableCollection<Produto>();

        public static void AdicionarAoCarrinho(Produto p)
        {
            var itemExistente = CarrinhoGlobal.FirstOrDefault(item => item.IdProduto == p.IdProduto);

            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
            }
            else
            {
                if (p.Quantidade <= 0) p.Quantidade = 1;
                CarrinhoGlobal.Add(p);
            }

            // Chamamos o contador da bolinha vermelha aqui
            AtualizaContadorGlobal();
        }

        public static void AtualizaContadorGlobal()
        {
            // Soma a quantidade total de itens no carrinho
            int totalItens = CarrinhoGlobal.Sum(p => p.Quantidade);

            // Encontra a Window principal e atualiza o texto do contador
            var mainWindow = Application.Current.MainWindow as WindowHome;
            if (mainWindow != null)
            {
                // Certifique-se de que o nome no XAML seja Txt_ContadorCarrinho
                mainWindow.Txt_ContadorCarrinho.Text = totalItens.ToString();
            }
        }
    }
}