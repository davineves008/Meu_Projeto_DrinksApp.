using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string Usuario = txtUsuario.Text;
            string senha = txtSenha.Password;

            if (string.IsNullOrWhiteSpace(Usuario))
            {
                MessageBox.Show("Digite o Usuario!");
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Digite a senha!");
                txtSenha.Focus();
                return;
            }

            ClienteRepositorio repo = new ClienteRepositorio();

            if (repo.Login(Usuario, senha))
            {
                MessageBox.Show("Login realizado com sucesso!");

                WindowHome pagina = new WindowHome();
                pagina.Show();
            }
            else
            {
                MessageBox.Show("Usuario ou senha inválidos!");
            }
        }

        private void BtnCadastro_Click(object sender, RoutedEventArgs e)
        {
            TelaDeCadastro cadastro = new TelaDeCadastro();
            cadastro.Show();
        }

        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}