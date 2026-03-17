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
using Projeto_DrinksApp.Models;


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

            // Chamamos o método que agora retorna o objeto completo
            var clienteLogado = repo.Login(txtUsuario.Text, txtSenha.Password);

            // MUDANÇA AQUI: Em vez de 'if (login)', usamos 'if (clienteLogado != null)'
            if (clienteLogado != null)
            {
                //Salva na variavel criada na app.xaml.
                App.ClienteLogado = clienteLogado;

                MessageBox.Show($"Seja bem-vindo, {clienteLogado.Nome}!\nPrepare-se para os melhores drinks.",
                 "DrinksApp 🍹",
                 MessageBoxButton.OK,
                 MessageBoxImage.Information);

                string enderecoParaExibir = clienteLogado.EnderecoResidencial?.EnderecoCompleto ?? "Sem endereço";

                WindowHome home = new WindowHome(enderecoParaExibir);
                home.Show();
                this.Close();
            }
            else
            {
                // Se cair aqui, é porque o SELECT não trouxe nenhum registro (usuário ou senha errados)
                MessageBox.Show("Usuário ou senha inválidos!");
            }
        }

        private void BtnCadastro_Click(object sender, RoutedEventArgs e)
        {
            TelaDeCadastro cadastro = new TelaDeCadastro();
            cadastro.Show();
        }

        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Seu código aqui (ex: validar se o campo está vazio)
        }

      

    }
}