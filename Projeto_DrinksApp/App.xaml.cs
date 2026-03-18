using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq; // Necessário para o FirstOrDefault
using System.Windows;
using Projeto_DrinksApp.Models;
using static Projeto_DrinksApp.Models.Pedido;


using System.Data;

namespace Projeto_DrinksApp
{
    public partial class App : Application
    {

        //linha de conexao com o sql pra venda produto;
        public static string LinhaConexao = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";

        //contador pra confirma venda;
        public static int ContadorPin { get; set; } = 0;

        //Classe global onde salva os dados do usuario logado;
        public static Clientes ClienteLogado { get; set; }

        // Lista global que armazena os itens selecionados
        public static ObservableCollection<Produto> CarrinhoGlobal { get; set; } = new ObservableCollection<Produto>();


        
        //metodo que adiciona produtoss no carrinho
        public static void AdicionarAoCarrinho(Produto novoProduto)
        {
            // Verifica se esse item específico já está na lista
            var itemNoCarrinho = CarrinhoGlobal.FirstOrDefault(p => p.Nome == novoProduto.Nome);

            if (itemNoCarrinho != null)
            {
                // Se já existir, apenas incrementa a quantidade na linha existente
                itemNoCarrinho.Quantidade++;
            }
            else
            {
                // Se for um produto novo (ex: você tinha Vinho e agora adicionou Brahma), 
                // o .Add() cria automaticamente uma nova linha visual no carrinho.
                CarrinhoGlobal.Add(novoProduto);
            }
        }
        public static void NotificarMudancaNoCarrinho()
        {
            // Procura a janela principal
            var principal = Application.Current.MainWindow as WindowHome;
            if (principal != null)
            {
                // Atualiza a bolinha (Badge)
                principal.AtualizarBadgeCarrinho();

                // Se o UC_Carrinho estiver aberto, atualiza o texto do Total
                // Você pode usar uma lógica de busca de controles ou eventos
            }
        }

    }
}