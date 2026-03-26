using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Necessário para ObservableCollection
using System.Windows;
using System.Windows.Controls;
using Projeto_DrinksApp.Models;

namespace Projeto_DrinksApp
{
    public partial class UC_Notificacoes : UserControl
    {
        // Usamos ObservableCollection para que a tela atualize sozinha ao remover itens
        public ObservableCollection<NotificacaoItem> Notificacoes { get; set; }

        public UC_Notificacoes()
        {
            InitializeComponent();
            CarregarNotificacoes();
        }

        private void CarregarNotificacoes()
        {
            // Simulando busca de dados
            Notificacoes = new ObservableCollection<NotificacaoItem>
            {
                new NotificacaoItem { Titulo = "Estoque Baixo", Mensagem = "A cerveja Brahma está com menos de 5 unidades.", Horario = "14:30" },
                new NotificacaoItem { Titulo = "Venda Realizada", Mensagem = "Pedido #1024 finalizado com sucesso.", Horario = "12:15" },
                new NotificacaoItem { Titulo = "Sistema", Mensagem = "Backup diário concluído.", Horario = "08:00" }
            };

            ListaNoticacoes.ItemsSource = Notificacoes;
        }

        // Método para o botão "Limpar Tudo" (o que adicionamos no topo)
        private void BtnLimparNotificacoes_Click(object sender, RoutedEventArgs e)
        {
            if (Notificacoes.Count == 0) return;

            var resultado = MessageBox.Show("Deseja apagar todas as notificações?", "Confirmar",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                Notificacoes.Clear();
                // Se fosse no banco de dados, você chamaria o comando DELETE aqui
            }
        }

        // Método para excluir UMA notificação específica (clique no botão dentro da lista)
        private void BtnExcluirUma_Click(object sender, RoutedEventArgs e)
        {
            // Pega o item que foi clicado através do DataContext do botão
            var btn = sender as Button;
            var itemParaRemover = btn.DataContext as NotificacaoItem;

            if (itemParaRemover != null)
            {
                Notificacoes.Remove(itemParaRemover);
            }
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as WindowHome;
            if (mainWindow != null)
            {
                mainWindow.ConteudoPrincipal.Content = new UC_Config();
            }
        }
    }
}