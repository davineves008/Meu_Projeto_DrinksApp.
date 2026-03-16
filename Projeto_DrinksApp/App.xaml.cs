using Projeto_DrinksApp.Models;
using System.Collections.ObjectModel;
using System.Linq; // Necessário para o FirstOrDefault
using System.Windows;

namespace Projeto_DrinksApp
{
    public partial class App : Application
    {
        //Classe global onde salva os dados do usuario logado;
        public static Clientes ClienteLogado { get; set; }

        // Lista global que armazena os itens selecionados
        public static ObservableCollection<Produto> CarrinhoGlobal { get; set; } = new ObservableCollection<Produto>();

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