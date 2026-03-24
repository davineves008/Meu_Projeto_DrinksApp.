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
        public void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validação de campos obrigatórios do Cliente
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtCPF.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, preencha todos os dados pessoais do cliente.", "Campos Vazios");
                return;
            }

            // 2. Validação de campos obrigatórios do Endereço
            if (string.IsNullOrWhiteSpace(txtLogradouro.Text) ||
                string.IsNullOrWhiteSpace(txtNumero.Text) ||
                string.IsNullOrWhiteSpace(txtBairro.Text) ||
                string.IsNullOrWhiteSpace(txtCidade.Text) ||
                string.IsNullOrWhiteSpace(txtEstado.Text) ||
                string.IsNullOrWhiteSpace(txtCEP.Text))
            {
                MessageBox.Show("Os dados de endereço são obrigatórios. Por favor, preencha todos os campos.", "Endereço Incompleto");
                return;
            }

            // 3. Tratamento e Validação do CPF (11 dígitos)
            string cpfLimpo = Regex.Replace(txtCPF.Text, @"[^\d]", "");
            if (cpfLimpo.Length != 11)
            {
                MessageBox.Show("O CPF deve conter exatamente 11 dígitos.", "CPF Inválido");
                return;
            }

            // 4. Verificação de duplicidade no Banco
            ClienteRepositorio repo = new ClienteRepositorio();
            if (repo.ExisteCliente(txtNome.Text))
            {
                MessageBox.Show("Este nome de cliente já existe no sistema.", "Duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 5. Nível de acesso
            int nivelSelecionado = cbNivel.Text == "ADM" ? 1 : 0;

            // 6. Montagem do objeto Cliente e Endereço
            Clientes novoCliente = new Clientes()
            {
                Nome = txtNome.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Usuario = txtUsuario.Text.Trim(),
                Senha = txtSenha.Password,
                CPF = cpfLimpo,
                Nivel = nivelSelecionado,
                EnderecoResidencial = new Endereço()
                {
                    Logradouro = txtLogradouro.Text.Trim(),
                    Numero = txtNumero.Text.Trim(),
                    Bairro = txtBairro.Text.Trim(),
                    Cidade = txtCidade.Text.Trim(),
                    Estado = txtEstado.Text.Trim(),
                    Cep = Regex.Replace(txtCEP.Text, @"[^\d]", "")
                }
            };

            // 7. Envio para o Banco de Dados
            try
            {
                if (repo.CadastrarCompleto(novoCliente))
                {
                    MessageBox.Show("Cliente e endereço cadastrados com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro");
            }
        }
    }
}
