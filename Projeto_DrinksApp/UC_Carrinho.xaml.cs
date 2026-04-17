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

        //metodo de finalizar a venda;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Validações Iniciais
                if (App.CarrinhoGlobal == null || App.CarrinhoGlobal.Count == 0)
                {
                    MessageBox.Show("O carrinho está vazio!");
                    return;
                }

                // 2. Validação de Senha (PIN)
                string pin = Microsoft.VisualBasic.Interaction.InputBox("Digite sua senha para confirmar:", "Segurança", "");
                if (string.IsNullOrEmpty(pin)) return;

                if (pin.Trim() != (App.ClienteLogado?.Senha?.ToString().Trim() ?? ""))
                {
                    MessageBox.Show("Senha incorreta!");
                    return;
                }

                // 3. Preparação dos Dados
                decimal totalVenda = (decimal)App.CarrinhoGlobal.Sum(x => x.PrecoTotal);
                // Criamos um texto com os nomes dos produtos para aparecer no histórico
                string resumoProdutos = string.Join(", ", App.CarrinhoGlobal.Select(x => x.Nome));

                if (App.ClienteLogado != null)
                {
                    VendaRepositorio repoVenda = new VendaRepositorio();
                    ProdutoRepositorio repoProd = new ProdutoRepositorio();
                    PedidoRepositorio repoPed = new PedidoRepositorio(); // Repositório do Histórico

                    // 4. Salva a Venda Financeira
                    bool salvouVenda = repoVenda.SalvarVenda(App.ClienteLogado.IdCliente, App.CarrinhoGlobal.Count, totalVenda);

                    if (salvouVenda)
                    {
                        // 5. Loop para Baixar Estoque e Identificar IDs
                        foreach (var item in App.CarrinhoGlobal)
                        {
                            int idReal = item.IdProduto;

                            // Correção manual se o ID estiver vindo 0 da tela
                            if (idReal <= 0)
                            {
                                switch (item.Nome)
                                {
                                    case "Brahma Duplo Malte": idReal = 101; break;
                                    case "Jack Daniels": idReal = 102; break;
                                    case "Xis Burguer": idReal = 103; break;
                                    case "Vinho Tinto": idReal = 104; break;
                                }
                            }

                            // Baixa o estoque no banco
                            repoProd.BaixarEstoque(idReal, item.Quantidade);
                        }

                        // 6. GRAVAR NO HISTÓRICO (Para aparecer nos "Últimos Pedidos")
                        Pedido novoPedido = new Pedido
                        {
                            ValorTotal = totalVenda,
                            Observacoes = resumoProdutos // Aqui fica o texto: "Brahma, Vinho Tinto..."
                        };

                        // Salva na tabela de pedidos vinculada ao ID do cliente logado
                        repoPed.InserirPedido(novoPedido, App.ClienteLogado.IdCliente);

                        // 7. Atualiza o objeto do cliente na memória para o Perfil atualizar sem precisar deslogar
                        App.ClienteLogado.UltimoPedidoDescricao = resumoProdutos;
                        App.ClienteLogado.UltimoPedidoValor = totalVenda;
                        App.ClienteLogado.UltimoPedidoData = DateTime.Now;

                        MessageBox.Show($"Pedido finalizado!\nTotal: {totalVenda:C2}\nEstoque e Histórico atualizados.");

                        //notificação
                        NotificacaoService.Adicionar("Compra Realizada", $"Pedido #{novoPedido.IdPedido} finalizado com sucesso.");

                        // 8. Limpa e Reseta
                        App.CarrinhoGlobal.Clear();
                        FinalizarEResetarCarrinho();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao finalizar: " + ex.Message);
            }
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

