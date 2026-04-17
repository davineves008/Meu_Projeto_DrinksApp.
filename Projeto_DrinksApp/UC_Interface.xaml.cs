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
    /// Interação lógica para UC_Interface.xam
    /// </summary>
    public partial class UC_Interface : UserControl
    {
        public UC_Interface()
        {
            InitializeComponent();
        }
        private void AplicarTema(bool isDark)
        {
            // Cor para o fundo (Escuro ou Transparente/Claro)
            Brush corFundo = isDark
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1D")) // Um preto mais profundo
                : Brushes.White;

            // 1. Aplica ao próprio UserControl
            this.Background = corFundo;

            // 2. Aplica à Janela Principal (WindowHome)
            var principal = Window.GetWindow(this) as WindowHome;
            if (principal != null)
            {
                principal.Background = corFundo;

                // Dica: Se você tiver uma Grid principal na WindowHome chamada 'GridPrincipal', 
                // você pode mudar o fundo dela também:
                // principal.GridPrincipal.Background = corFundo;
            }
        }

        // Evento dos RadioButtons
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            // 1. Identifica qual cor foi clicada
            Button btn = (Button)sender;
            SolidColorBrush novaCor = (SolidColorBrush)btn.Background;

            // 2. Altera o recurso no dicionário da aplicação
            // Isso atualiza AUTOMATICAMENTE todas as janelas que usam DynamicResource
            Application.Current.Resources["CorDestaqueDinamica"] = novaCor;

            //notificação
            NotificacaoService.Adicionar("Cor Alterada", $"A cor de destaque foi alterada.");
        }

        private void rbTema_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            RadioButton rb = sender as RadioButton;

            if (rb?.Name == "rbEscuro")
            {
                var gradiente = new LinearGradientBrush();
                gradiente.StartPoint = new Point(0, 0);
                gradiente.EndPoint = new Point(1, 1);
                gradiente.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#1A1A2E"), 0));
                gradiente.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#333333"), 1));
                Application.Current.Resources["FundoGradientDinamico"] = gradiente;
                Application.Current.Resources["CorTextoDinamica"] = new SolidColorBrush(Colors.WhiteSmoke);
            }
            else
            {
                // Tema Claro
                Application.Current.Resources["FundoGradientDinamico"] = new SolidColorBrush(Colors.White);
                Application.Current.Resources["CorTextoDinamica"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#121212"));
            }
        }
        // 3. Método para o botão Salvar
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            // Aqui você salvaria as preferências (ex: Properties.Settings ou JSON)
            string tema = rbEscuro.IsChecked == true ? "Escuro" : "Claro";
            NotificacaoService.Adicionar("Tema Alterado", $"Tema alterado para: Modo {tema}.");


            string idioma = (cmbIdioma.SelectedItem as ComboBoxItem)?.Content.ToString();

            MessageBox.Show($"Configurações Salvas!\nTema: {tema}\nIdioma: {idioma}",
                            "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}

