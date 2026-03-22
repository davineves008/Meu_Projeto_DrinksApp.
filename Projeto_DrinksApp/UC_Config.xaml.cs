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
    /// Interação lógica para UC_Config.xam
    /// </summary>
    public partial class UC_Config : UserControl
    {
        public UC_Config()
        {
            InitializeComponent();
        }

        //Btn pra tela de interfaces;
        private void BtnGoInterface_Click(object sender, RoutedEventArgs e)
        {
            // Acessa a WindowHome e troca o conteúdo do ContentControl principal
            var windowHome = Window.GetWindow(this) as WindowHome;
            if (windowHome != null)
            {
                windowHome.ConteudoPrincipal.Content = new UC_Interface();
            }
        }

        //Btn pra tela de perfil;
        private void BtnGoPerfil_Click(object sender, RoutedEventArgs e)
        {
            var windowHome = Window.GetWindow(this) as WindowHome;
            if (windowHome != null)
            {
                windowHome.ConteudoPrincipal.Content = new UC_Perfil();
            }
        }

        //btn Pra tela de notificações;
        private void BtnGoNotificacoes_Click(object sender, RoutedEventArgs e)
        {
            // Instancia o UserControl de Notificações
            UC_Notificacoes telaNotificacoes = new UC_Notificacoes();

            // Coloca ele dentro do ContentControl chamado AreaConfiguracao
            AreaConfiguracao.Content = telaNotificacoes;

            
        }

        //Btn pra tela de segurança de dados;
        private void btnSeguranca_Click(object sender, RoutedEventArgs e)
        {
            // Limpa o que estiver na tela e coloca a UC de Segurança
            AreaConfiguracao.Content = new UC_Seguranca();
        }
    }
}
