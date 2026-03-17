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
using Projeto_DrinksApp.Models;

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

        //Btn Finalizar a venda;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validação inicial (Carrinho vazio)
            if (App.CarrinhoGlobal == null || App.CarrinhoGlobal.Count == 0)
            {
                MessageBox.Show("Seu carrinho está vazio!", "Carrinho Vazio", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- NOVO: VALIDAÇÃO DO PIN DE SEGURANÇA ---
            // Você pode usar uma Window personalizada ou o Interaction.InputBox (precisa da referência Microsoft.VisualBasic)
            string pinDigitado = Microsoft.VisualBasic.Interaction.InputBox("Digite seu PIN de 4 dígitos para confirmar a venda:", "Segurança do PDV", "");

            if (pinDigitado != App.ClienteLogado.Senha)
            {
                App.ContadorPin++; // Variável que criamos no App.xaml.cs

                if (App.ContadorPin >= 4)
                {
                    MessageBox.Show("Limite de tentativas excedido! O sistema será bloqueado.", "SEGURANÇA", MessageBoxButton.OK, MessageBoxImage.Error);

                    // LOGOUT FORÇADO
                    App.ClienteLogado = null;
                    App.ContadorPin = 0;
                    new MainWindow().Show();
                    Window.GetWindow(this).Close();
                    return;
                }
                else
                {
                    int restantes = 4 - App.ContadorPin;
                    MessageBox.Show($"PIN Incorreto! Tentativas restantes: {restantes}", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Interrompe o método aqui para não finalizar a venda
                }
            }

            // Se chegou aqui, o PIN está correto! Resetamos as tentativas.
            App.ContadorPin = 0;

            // 2. Pergunta do CPF (Só acontece após o PIN estar correto)
            MessageBoxResult resultadoCpf = MessageBox.Show("Deseja informar o CPF?", "CPF", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultadoCpf == MessageBoxResult.Yes)
            {
                MessageBox.Show("CPF registrado!");
            }

            // --- PARTE DO BANCO DE DADOS (Agora protegida pelo PIN) ---
            ProdutoRepositorio repoProd = new ProdutoRepositorio();
            decimal totalVenda = (decimal)App.CarrinhoGlobal.Sum(p => p.PrecoTotal);
            string resumoProdutos = string.Join(", ", App.CarrinhoGlobal.Select(p => p.Nome));

            foreach (var item in App.CarrinhoGlobal)
            {
                repoProd.ExcluirProdutoDoBanco(item.IdProduto);
            }

            if (App.ClienteLogado != null)
            {
                PedidoRepositorio repoPed = new PedidoRepositorio();
                Pedido p = new Pedido();
                p.ValorTotal = totalVenda;
                p.Observacoes = resumoProdutos;

                repoPed.InserirPedido(p, App.ClienteLogado.IdCliente);

                App.ClienteLogado.UltimoPedidoDescricao = resumoProdutos;
                App.ClienteLogado.UltimoPedidoValor = totalVenda;
                App.ClienteLogado.UltimoPedidoData = DateTime.Now;
            }

            // 3. Feedback e Limpeza
            MessageBox.Show($"Venda confirmada com sucesso!\nTotal: {totalVenda:C2}", "Sucesso");
            App.CarrinhoGlobal.Clear();
            FinalizarEResetarCarrinho();
        }
        //metodo que reseta e limpa o carrinho;
        private void FinalizarEResetarCarrinho()
        {
            App.CarrinhoGlobal.Clear();

            // Atualiza a tela do carrinho para mostrar que está vazio
            ListaCarrinho.ItemsSource = null;
            ListaCarrinho.ItemsSource = App.CarrinhoGlobal;

            // Zera o texto do Total Geral
            Txt_TotalGeral.Text = "R$ 0,00";

            // Opcional: Voltar para a tela inicial após a venda
            var principal = Window.GetWindow(this) as WindowHome;
            if (principal != null)
            {
                // principal.ConteudoPrincipal.Content = new UC_Home(); 
            }
        }
    }
}

