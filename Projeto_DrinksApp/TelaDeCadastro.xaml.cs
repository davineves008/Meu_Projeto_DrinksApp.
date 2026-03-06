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

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            // Capturando os dados das TextBoxes que espalhamos na tela
            string nome = txtNome.Text;
            string email = txtEmail.Text;
            string cidade = txtCidade.Text;
            string cpf = txtCPF.Text;
            string senha = txtSenha.Password; // PasswordBox usa .Password em vez de .Text
            string usuario = txtUsuario.Text; // Novo campo

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
    }
}
