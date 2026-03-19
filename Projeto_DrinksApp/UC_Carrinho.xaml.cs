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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validação inicial (Carrinho vazio)
            if (App.CarrinhoGlobal == null || App.CarrinhoGlobal.Count == 0)
            {
                MessageBox.Show("Seu carrinho está vazio!", "Carrinho Vazio", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- VALIDAÇÃO DO PIN (Senha do Usuário) ---
            string pinDigitado = Microsoft.VisualBasic.Interaction.InputBox("Confirme sua senha de 4 dígitos para finalizar:", "Segurança do PDV", "");

            // Compara o que foi digitado com a senha do objeto logado
            if (pinDigitado.Trim() != App.ClienteLogado.Senha.ToString().Trim())
            {
                App.ContadorPin++;

                if (App.ContadorPin >= 4)
                {
                    MessageBox.Show("Limite de tentativas excedido!", "BLOQUEADO", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Logout e reset
                    App.ClienteLogado = null;
                    App.ContadorPin = 0;
                    new MainWindow().Show();
                    Window.GetWindow(this).Close();
                    return;
                }
                else
                {
                    int restantes = 4 - App.ContadorPin;
                    MessageBox.Show($"Senha Incorreta! Tentativas restantes: {restantes}", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
                    return;
                }
            }

            // SE CHEGOU AQUI, A SENHA ESTÁ CORRETA!
            App.ContadorPin = 0;

           

            // 2. Fluxo de Venda (CPF e Banco de Dados)
            MessageBoxResult resultadoCpf = MessageBox.Show("Deseja informar o CPF?", "CPF", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // 1. Primeiro, preparamos os dados do carrinho
            decimal totalVenda = (decimal)App.CarrinhoGlobal.Sum(x => x.PrecoTotal);
            string resumoFinal = string.Join(", ", App.CarrinhoGlobal.Select(x => x.Nome));
            int quantidadeDeItens = App.CarrinhoGlobal.Count; // O número que o banco espera

            if (App.ClienteLogado != null)
            {
                // 2. Gravando na tabela de Vendas (Repositorio que criamos por último)
                VendaRepositorio repoVenda = new VendaRepositorio();
                // IMPORTANTE: Passamos 'quantidadeDeItens' (int) e não a string!
                bool salvou = repoVenda.SalvarVenda(App.ClienteLogado.IdCliente, quantidadeDeItens, totalVenda);

                // 3. Gravando na tabela de Pedidos (Para o histórico neon aparecer)
                PedidoRepositorio repoPed = new PedidoRepositorio();
                Pedido p = new Pedido();
                p.ValorTotal = totalVenda;
                p.Observacoes = resumoFinal; // Aqui salvamos os nomes dos produtos para o cliente ler

                repoPed.InserirPedido(p, App.ClienteLogado.IdCliente);

                // 4. Atualiza a memória para o perfil
                App.ClienteLogado.UltimoPedidoDescricao = resumoFinal;
                App.ClienteLogado.UltimoPedidoValor = totalVenda;
                App.ClienteLogado.UltimoPedidoData = DateTime.Now;

                MessageBox.Show("Venda finalizada com sucesso!");
            }
            App.CarrinhoGlobal.Clear();

            // Método que você já tem para atualizar a lista da tela
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

