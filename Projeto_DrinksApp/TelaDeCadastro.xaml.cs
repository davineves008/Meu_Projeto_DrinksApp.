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
        public void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validação simples
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor, preencha os campos obrigatórios.");
                return;
            }

            // 2. Lógica para converter a seleção do ComboBox em número (int)
            // Se o texto selecionado for "ADM", nivel = 1, caso contrário nivel = 0
            int nivelSelecionado = cbNivel.Text == "ADM" ? 1 : 0;

            // 3. Criamos o objeto cliente com os dados da tela
            Clientes novoCliente = new Clientes()
            {
                Nome = txtNome.Text,
                Email = txtEmail.Text,
                Usuario = txtUsuario.Text,
                Senha = txtSenha.Password,
                CPF = txtCPF.Text,
                Nivel = nivelSelecionado, // Atribuindo o nível aqui
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

            // 4. Chamada do repositório
            ClienteRepositorio repo = new ClienteRepositorio();

            if (repo.CadastrarCompleto(novoCliente))
            {
                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
