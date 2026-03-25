using Projeto_DrinksApp.Models;
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
using Projeto_DrinksApp;

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
            VerificarAcessoAdm();
        }

        //Metodo que verificaquem esta logado;
        private void VerificarAcessoAdm()
        {
            // Verificamos se o nível é IGUAL a 1 (int) e não ao texto "ADM"
            if (WindowHome.Instancia?.Usuariologado != null &&
                WindowHome.Instancia.Usuariologado.Nivel == 1)
            {
                btnCriarProduto.Visibility = Visibility.Visible;
            }
            else
            {
                btnCriarProduto.Visibility = Visibility.Collapsed;
            }
        }

        private void btnCriarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (WindowHome.Instancia != null)
            {
                // Isso remove a UC_Config e coloca a Uc_CriarProduto no lugar
                WindowHome.Instancia.ConteudoPrincipal.Content = new Uc_CriarProduto();
            }
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
            // 1. Instancia a tela de notificações
            UC_Notificacoes telaNotificacoes = new UC_Notificacoes();

            // 2. Busca a janela onde este UserControl está "espetado"
            // Substitua 'MainWindow' ou 'WindowHome' pelo nome real da sua classe da tela principal
            var windowHome = Window.GetWindow(this) as WindowHome;

            if (windowHome != null)
            { 
                
                // 3. Acessa o ContentControl da tela principal e troca o conteúdo
                windowHome.ConteudoPrincipal.Content = telaNotificacoes;
            }
        }

        //Btn pra tela de segurança de dados;
        private void btnSeguranca_Click(object sender, RoutedEventArgs e)
        {
            // Limpa o que estiver na tela e coloca a UC de Segurança
            AreaConfiguracao.Content = new UC_Seguranca();
        }
    }
}
