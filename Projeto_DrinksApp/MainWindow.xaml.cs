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
            // 1. Captura as entradas do usuário
            string Usuario = txtUsuario.Text;
            string senha = txtSenha.Password;

            // 2. Validações básicas de campos vazios
            if (string.IsNullOrWhiteSpace(Usuario))
            {
                MessageBox.Show("Digite o Usuário!");
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Digite a senha!");
                txtSenha.Password = ""; // Limpa a senha se estiver apenas com espaços
                txtSenha.Focus();
                return;
            }

            try
            {
                // 3. Tenta realizar o login no Banco de Dados
                ClienteRepositorio repo = new ClienteRepositorio();
                var clienteLogado = repo.Login(Usuario, senha);

                if (clienteLogado != null)
                {
                    // 4. Salva o objeto na variável global para ser usado em todo o App
                    App.ClienteLogado = clienteLogado;

                    // 5. Monta a string de endereço para exibir no topo da WindowHome
                    string infoParaTopo = "";

                    if (clienteLogado.EnderecoResidencial != null)
                    {
                        // Formato: "Rua Exemplo, 123"
                        infoParaTopo = $"{clienteLogado.EnderecoResidencial.Logradouro}, {clienteLogado.EnderecoResidencial.Numero}";
                    }
                    else
                    {
                        // Se o endereço for nulo no banco, usamos o Nome como fallback
                        infoParaTopo = clienteLogado.Nome;
                    }

                    // 6. Abre a janela principal (WindowHome) passando o endereço formatado
                    // Sem aspas em clienteLogado.Nome para evitar o erro das imagens anteriores
                    WindowHome home = new WindowHome(infoParaTopo);
                    home.Show();

                    // 7. Fecha a tela de login
                    this.Close();
                }
                else
                {
                    // Caso o login falhe (Usuário ou senha não encontrados)
                    MessageBox.Show("Usuário ou senha inválidos!", "Erro de Acesso", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtSenha.Clear();
                    txtSenha.Focus();
                }
            }
            catch (Exception ex)
            {
                // Caso haja algum erro de conexão com o Banco de Dados
                MessageBox.Show("Erro ao conectar com o banco de dados: " + ex.Message);
            }
        }
        //
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