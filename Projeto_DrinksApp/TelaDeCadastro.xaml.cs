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
using System.Windows.Shapes;
using Projeto_DrinksApp.Models;

namespace Projeto_DrinksApp
{
    /// <summary>
    /// Lógica interna para TelaDeCadastro.xaml
    /// </summary>
    public partial class TelaDeCadastro : Window
    {
        public TelaDeCadastro()
        {
            InitializeComponent();
        }

        //Btn pra cadastro
        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            // Capturando os dados das TextBoxes 
            string nome = txtNome.Text;
            string email = txtEmail.Text;
            string cidade = txtCidade.Text;
            string cpf = txtCPF.Text;
            string senha = txtSenha.Password; // PasswordBox usa .Password em vez de .Text
            string usuario = txtUsuario.Text; // Novo campo

            // VALIDAÇÕES
            //val. nome
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Digite o nome.");
                txtNome.Focus();
                return;
            }

           
            //valida usuario;
            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("Digite o usuário.");
                txtUsuario.Focus();
                return;
            }

            //val. senha;
            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Digite a senha.");
                txtSenha.Focus();
                return;
            }
            //val. senha;
            if (!cpf.All(char.IsDigit))
            {
                MessageBox.Show("CPF deve conter apenas números.");
                txtCPF.Focus();
                return;
            }

            //val. email;
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Email inválido.");
                txtEmail.Focus();
                return;
            }

            //val. senha;
            if (senha.Length < 3)
            {
                MessageBox.Show("A senha deve ter pelo menos 3 caracteres.");
                txtSenha.Focus();
                return;
            }

            ClienteRepositorio repo = new ClienteRepositorio();

            if (repo.Cadastrar(nome, email, cidade, cpf, senha, usuario)) 
            {
                MessageBox.Show("Usuário cadastrado com sucesso!");
                this.Close(); // Fecha a tela de cadastro após o sucesso
            }
            else
            {
                MessageBox.Show("Erro ao cadastrar. Verifique os dados.");
            }
        }

        //btn_Voltar;
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
