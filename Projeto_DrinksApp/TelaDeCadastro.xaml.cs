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
            ClienteRepositorio repo = new ClienteRepositorio();

            try
            {
                //  Campos vazios
                if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtCPF.Text) ||
                    string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                    string.IsNullOrWhiteSpace(txtLogradouro.Text) ||
                    string.IsNullOrWhiteSpace(txtNumero.Text) ||
                    string.IsNullOrWhiteSpace(txtBairro.Text) ||
                    string.IsNullOrWhiteSpace(txtCidade.Text) ||
                    string.IsNullOrWhiteSpace(txtEstado.Text) ||
                    string.IsNullOrWhiteSpace(txtCEP.Text) ||
                    txtSenha.Password.Length == 0 ||
                    txtSenha.Password.Length == 0)
                {
                    MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Campos Vazios", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Nome
                if (!Regex.IsMatch(txtNome.Text, @"^[a-zA-ZÀ-ÿ\s]{3,}$"))
                {
                    MessageBox.Show("Nome inválido! Digite seu nome completo usando apenas letras.", "Nome Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Usuário
                if (!Regex.IsMatch(txtUsuario.Text, @"^[a-zA-Z0-9_]{4,}$"))
                {
                    MessageBox.Show("Usuário inválido! Mínimo 4 caracteres, apenas letras, números e _", "Usuário Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Email
                if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("E-mail inválido! Use o formato: exemplo@email.com", "Email Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //CPF
                if (!repo.ValidarFormatoCPF(txtCPF.Text))
                {
                    MessageBox.Show("CPF inválido! Digite um CPF com 11 números.", "CPF Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Senha
                if (txtSenha.Password.Length < 6)
                {
                    MessageBox.Show("A senha deve ter no mínimo 6 caracteres.", "Senha Fraca", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!Regex.IsMatch(txtSenha.Password, @"[A-Za-z]") || !Regex.IsMatch(txtSenha.Password, @"[0-9]"))
                {
                    MessageBox.Show("A senha deve conter pelo menos 1 letra e 1 número.", "Senha Fraca", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Confirmar Senha
                if (txtSenha.Password != txtSenha.Password)
                {
                    MessageBox.Show("As senhas não coincidem. Verifique e tente novamente.", "Senhas Diferentes", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // CEP
                if (!Regex.IsMatch(txtCEP.Text, @"^\d{5}-?\d{3}$"))
                {
                    MessageBox.Show("CEP inválido! Use o formato: 00000-000", "CEP Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Número
                if (!Regex.IsMatch(txtNumero.Text, @"^\d+$"))
                {
                    MessageBox.Show("Número inválido! Digite apenas números.", "Número Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Estado
                if (!Regex.IsMatch(txtEstado.Text, @"^[A-Za-z]{2}$"))
                {
                    MessageBox.Show("Estado inválido! Use a sigla com 2 letras. Ex: RS, SP, RJ", "Estado Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Bairro
                if (!Regex.IsMatch(txtBairro.Text, @"^[a-zA-ZÀ-ÿ\s]+$"))
                {
                    MessageBox.Show("Bairro inválido! Digite apenas letras.", "Bairro Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Cidade
                if (!Regex.IsMatch(txtCidade.Text, @"^[a-zA-ZÀ-ÿ\s]+$"))
                {
                    MessageBox.Show("Cidade inválida! Digite apenas letras.", "Cidade Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Logradouro
                if (txtLogradouro.Text.Trim().Length < 5)
                {
                    MessageBox.Show("Logradouro inválido! Digite o nome completo da rua.", "Logradouro Inválido", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 
                Clientes novoCliente = new Clientes
                {
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    CPF = txtCPF.Text,
                    Cidade = txtCidade.Text,
                    Usuario = txtUsuario.Text,
                    Senha = txtSenha.Password,
                    Nivel = cbNivel.SelectedIndex == 0 ? 0 : 1
                };

                Endereço novoEndereco = new Endereço
                {
                    Logradouro = txtLogradouro.Text,
                    Numero = txtNumero.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Estado = txtEstado.Text,
                    Cep = txtCEP.Text
                };

                repo.FinalizarCadastro(novoCliente, novoEndereco);

                //Notificação de cadastro
                NotificacaoService.Adicionar("Cadastro Realizado", $"Novo usuário {novoCliente.Usuario} cadastrado com sucesso.");

                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível concluir o cadastro.\nDetalhes: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

