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
    /// Interação lógica para UC_Perfil.xam
    /// </summary>
    public partial class UC_Perfil : UserControl
    {
        public UC_Perfil()
        {
            InitializeComponent();
        }
        //btn Salvar perfil
        private void btnSalvarPerfil_Click(object sender, RoutedEventArgs e)
        {
            // Coletando os dados editados
            string nome = txtNome.Text;
            string email = txtEmail.Text;
            string endereco = txtEndereco.Text;
            string cidade = txtCidade.Text;

            // Aqui você enviaria para o Banco de Dados ou salvaria no arquivo
            // Por enquanto, vamos dar um feedback para o usuário
            MessageBox.Show($"Dados de {nome} salvos com sucesso!\nEndereço: {endereco}, {cidade}",
                            "Perfil Atualizado",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}
