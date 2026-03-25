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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projeto_DrinksApp
{
    /// <summary>
    /// Interação lógica para UC_Seguranca.xam
    /// </summary>
    public partial class UC_Seguranca : UserControl
    {
        public UC_Seguranca()
        {
            InitializeComponent();
        }

        //Btn pra atualiza a senha no banco de dados.
        private void btnAtualizarSenha_Click(object sender, RoutedEventArgs e)
        {
            string senhaAtualDigitada = txtSenhaAtual.Password;
            string novaSenha = txtSenhaNova.Password;

            // 1. Validação de campos vazios
            if (string.IsNullOrWhiteSpace(senhaAtualDigitada) || string.IsNullOrWhiteSpace(novaSenha))
            {
                MessageBox.Show("Preencha os campos de senha!");
                return;
            }

            // 2. Verifica se a senha atual está correta (comparando com o que está no objeto logado)
            if (senhaAtualDigitada != App.ClienteLogado.Senha)
            {
                MessageBox.Show("A senha atual está incorreta!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                txtSenhaAtual.Clear();
                txtSenhaAtual.Focus();
                return;
            }

            // 3. Tenta atualizar no banco
            ClienteRepositorio repo = new ClienteRepositorio();
            if (repo.AtualizarSenha(App.ClienteLogado.IdCliente, novaSenha))
            {
                // Atualiza a senha no objeto local para não deslogar ou dar erro na próxima troca
                App.ClienteLogado.Senha = novaSenha;

                MessageBox.Show("Senha atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpa os campos
                txtSenhaAtual.Clear();
                txtSenhaNova.Clear();
            }
        }



        //Btn pra deletar usuario do banco de dados;
        private void btnExcluirConta_Click(object sender, RoutedEventArgs e)
        {
            // 1. Pergunta com ícone de aviso
            var resultado = MessageBox.Show("VOCÊ TEM CERTEZA? Esta ação não pode ser desfeita e sua conta será apagada para sempre.",
                                            "AVISO CRÍTICO",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    // 2. Deleta do Banco de Dados
                    ClienteRepositorio repo = new ClienteRepositorio();
                    bool deletou = repo.ExcluirConta(App.ClienteLogado.IdCliente);

                    if (deletou)
                    {
                        MessageBox.Show("Sua conta foi removida. Esperamos ver você de novo em breve!", "Conta Excluída");

                        // 3. Limpa a variável global
                        App.ClienteLogado = null;

                        // 4. Volta para a tela de Login
                        MainWindow login = new MainWindow();
                        login.Show();

                        // 5. Fecha a janela atual (WindowHome)
                        Window.GetWindow(this).Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir conta: " + ex.Message);
                }
            }
        }

        //Metodo pra revelar ou mostrar  senha.
        private void btnRevelarSenha_Click(object sender, RoutedEventArgs e)
        {
            if (btnRevelarSenha.IsChecked == true)
            {
                // MOSTRAR SENHA
                txtSenhaRevelada.Text = txtSenhaNova.Password; // Copia a senha oculta para o texto
                txtSenhaNova.Visibility = Visibility.Collapsed;
                txtSenhaRevelada.Visibility = Visibility.Visible;
                iconOlho.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00F9FF")); // Muda cor para azul neon
            }
            else
            {
                // ESCONDER SENHA
                txtSenhaNova.Password = txtSenhaRevelada.Text; // Devolve o texto para o campo oculto
                txtSenhaRevelada.Visibility = Visibility.Collapsed;
                txtSenhaNova.Visibility = Visibility.Visible;
                iconOlho.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#88FFFFFF")); // Volta para cinza
            }
        }

        //Btn pra voltar a tela de config.
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Procura na árvore visual até encontrar o UC_Config
            DependencyObject parent = VisualTreeHelper.GetParent(this);

            while (parent != null && !(parent is UC_Config))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            // 2. Se encontrou, chama a função de voltar que criamos lá
            if (parent is UC_Config telaConfig)
            {
                telaConfig.VoltarAoMenuPrincipal();
            }
        }
    }
}











