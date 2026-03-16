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
    /// Interação lógica para UC_Notificações.xam
    /// </summary>
    public partial class UC_Notificacoes : UserControl
    {
        public UC_Notificacoes()
        {
            InitializeComponent();
            CarregarNotificacoes();
        }
        private void CarregarNotificacoes()
        {
            // Exemplo de dados (Depois você pode buscar isso do Banco de Dados)
            var notificacoes = new List<NotificacaoItem>
            {
                new NotificacaoItem { Titulo = "Estoque Baixo", Mensagem = "A cerveja Brahma está com menos de 5 unidades.", Horario = "14:30" },
                new NotificacaoItem { Titulo = "Venda Realizada", Mensagem = "Pedido #1024 finalizado com sucesso.", Horario = "12:15" },
                new NotificacaoItem { Titulo = "Sistema", Mensagem = "Backup diário concluído.", Horario = "08:00" }
            };

            ListaNotificacoes.ItemsSource = notificacoes;
        }
        //Btn Pra voltar
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as WindowHome;
            if (mainWindow != null)
            {
                // Substitua UC_Config pelo nome real do seu UserControl de configurações
                mainWindow.ConteudoPrincipal.Content = new UC_Config();
            }
        }
    }
}

