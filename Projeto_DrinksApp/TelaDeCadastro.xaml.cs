using Projeto_DrinksApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Instancia o repositório onde criamos o método com Transação
            ClienteRepositorio repo = new ClienteRepositorio();

            try
            {
                // 2. Criar o objeto do Cliente com os dados da tela
                // Note: Usei os nomes que aparecem na sua classe e no banco (Usuario, Senha, etc)
                Clientes novoCliente = new Clientes
                {
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    CPF = txtCPF.Text,
                    Cidade = txtCidade.Text, // A coluna cidade que existe na sua tabela Clientes
                    Usuario = txtUsuario.Text,
                    Senha = txtSenha.Password, // Se for PasswordBox, use .Password
                    Nivel = cbNivel.SelectedIndex == 0 ? 0 : 1 // 0 para Usuário, 1 para Admin
                };

                // 3. Criar o objeto de Endereço com os dados da parte direita da sua tela
                Endereço novoEndereco = new Endereço
                {
                    Logradouro = txtLogradouro.Text,
                    Numero = txtNumero.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Estado = txtEstado.Text,
                    Cep = txtCPF.Text
                };

                // 4. Validação básica (Opcional, mas recomendado)
                if (string.IsNullOrEmpty(novoCliente.Usuario) || string.IsNullOrEmpty(novoEndereco.Logradouro))
                {
                    MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                    return;
                }

                // 5. Chama o método que salva nas DUAS tabelas ao mesmo tempo
                repo.FinalizarCadastro(novoCliente, novoEndereco);

                // 6. Se o método FinalizarCadastro não disparar exceção, deu tudo certo!
                this.Close();
            }
            catch (Exception ex)
            {
                // Se der erro no Passo 1 (Cliente) ou no Passo 2 (Endereço), cai aqui
                MessageBox.Show("Não foi possível concluir o cadastro.\nDetalhes: " + ex.Message, "Erro");
            }
        }
    }
}
