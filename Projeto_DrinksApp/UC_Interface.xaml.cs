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
            if (isDark)
            {
                // Fundo Escuro Suave
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D30"));
                // Ajustar cores dos textos (Você precisará dar nomes aos TextBlocks no XAML se quiser mudar todos)
            }
            else
            {
                // Fundo Claro (Volta para transparente ou branco)
                this.Background = Brushes.Transparent;
            }
        }

        // Evento dos RadioButtons
        private void rbTema_Checked(object sender, RoutedEventArgs e)
        {
            // Verifica se os componentes já foram inicializados
            if (rbEscuro == null || rbClaro == null) return;

            AplicarTema(rbEscuro.IsChecked == true);
        }

        // 2. Método para os botões de cores de destaque
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            Button btnClicado = sender as Button;
            if (btnClicado != null)
            {
                // Pega a cor do botão que você clicou
                Brush corSelecionada = btnClicado.Background;

                // Aplica ao botão "Salvar Alterações" para dar feedback
                btnSalvar.Background = corSelecionada;
            }
        }

        // 3. Método para o botão Salvar
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            // Aqui você salvaria as preferências (ex: Properties.Settings ou JSON)
            string tema = rbEscuro.IsChecked == true ? "Escuro" : "Claro";
            string idioma = (cmbIdioma.SelectedItem as ComboBoxItem)?.Content.ToString();

            MessageBox.Show($"Configurações Salvas!\nTema: {tema}\nIdioma: {idioma}",
                            "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

