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
            // 1. Verificar se o carrinho não está vazio
            if (App.CarrinhoGlobal == null || App.CarrinhoGlobal.Count == 0)
            {
                MessageBox.Show("Seu carrinho está vazio! Adicione alguns drinks antes de finalizar.",
                                "Carrinho Vazio", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Pergunta sobre o CPF (Sim ou Não)
            MessageBoxResult resultadoCpf = MessageBox.Show("Deseja informar o CPF na nota fiscal?",
                                                            "CPF na Compra",
                                                            MessageBoxButton.YesNo,
                                                            MessageBoxImage.Question);

            if (resultadoCpf == MessageBoxResult.Yes)
            {
                // Aqui você poderia abrir um pequeno InputBox ou janela para o CPF.
                // Por enquanto, vamos simular que ele foi informado.
                MessageBox.Show("CPF registrado com sucesso!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // 3. Confirmar a Venda
         
            // Calcule uma única vez aqui no topo
            decimal total = (decimal)App.CarrinhoGlobal.Sum(p => p.PrecoTotal);

            MessageBox.Show($"Venda confirmada com sucesso!\nValor Total: R$ {total:F2}\n\nObrigado pela preferência!",
                            "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            // Dentro do seu botão de finalizar venda no Carrinho:
            if (App.ClienteLogado != null)
            {
                PedidoRepositorio repo = new PedidoRepositorio();

                Pedido p = new Pedido();
                p.ValorTotal = App.CarrinhoGlobal.Sum(x => x.PrecoTotal);
                p.Observacoes = string.Join(", ", App.CarrinhoGlobal.Select(x => x.Nome));

                // GRAVA NO BANCO DE DADOS
                repo.InserirPedido(p, App.ClienteLogado.IdCliente);

                MessageBox.Show("Pedido salvo no banco de dados!");

                // Agora sim, limpa o carrinho
                App.CarrinhoGlobal.Clear();
            }

            // 4. Limpar o carrinho após a venda
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

