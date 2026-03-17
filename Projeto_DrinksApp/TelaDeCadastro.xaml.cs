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
using System.Windows.Media.TextFormatting;
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
        
        //btn_Voltar;
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       //btn pra cadastra cliente e endereço
        public  void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            // Criamos o objeto cliente com os dados da tela
            Clientes novoCliente = new Clientes()
            {
                Nome = txtNome.Text,
                Email = txtEmail.Text,
                Usuario = txtUsuario.Text,
                Senha = txtSenha.Password,
                CPF = txtCPF.Text,
                EnderecoResidencial = new Endereço()
                {
                    Logradouro = txtLogradouro.Text,
                    Numero = txtNumero.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Estado = txtEstado.Text,
                    Cep = txtCEP.Text
                }
            };

            ClienteRepositorio repo = new ClienteRepositorio();
            if (repo.CadastrarCompleto(novoCliente))
            {
                MessageBox.Show("Cadastro realizado com sucesso!");
                this.Close();
            }
        }
    }
}
