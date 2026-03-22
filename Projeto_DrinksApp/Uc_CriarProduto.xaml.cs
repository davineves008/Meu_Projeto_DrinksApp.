using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Projeto_DrinksApp
{
    public partial class Uc_CriarProduto : UserControl
    {
        // Sua string de conexão para SQL Server
        private string connString = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";

        public Uc_CriarProduto()
        {
            InitializeComponent();
        }

        //Btn pra salvar o produto criado
        private void btnSalvarProduto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Validação de campos (corrigido o erro do .Text vazio)
                if (string.IsNullOrWhiteSpace(txtNomeProduto.Text) || string.IsNullOrWhiteSpace(txtPreco.Text))
                {
                    MessageBox.Show("Preencha os campos obrigatórios!");
                    return;
                }

                // 2. Tratamento do Preço
                decimal preco;
                if (!decimal.TryParse(txtPreco.Text.Replace(".", ","), out preco))
                {
                    MessageBox.Show("Preço inválido! Use apenas números.");
                    return;
                }

                // 3. Salvar no Banco (Usando SqlConnection para SQL Server)
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "INSERT INTO produtos (nome, preco, categoria, imagem) VALUES (@nome, @preco, @cat, @img)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtNomeProduto.Text);
                        cmd.Parameters.AddWithValue("@preco", preco);

                        // Pega o texto do ComboBox selecionado de forma segura
                        string categoria = (cbCategoria.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Sem Categoria";
                        cmd.Parameters.AddWithValue("@cat", categoria);

                        cmd.Parameters.AddWithValue("@img", txtImagem.Text ?? "");

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                        LimparCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar no banco: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //metodo pra limpar os campos
        private void LimparCampos()
        {
            txtNomeProduto.Clear();
            txtPreco.Clear();
            txtImagem.Clear();
            cbCategoria.SelectedIndex = -1;
        }

        //Metodo pra excluir produto dobancode dados;
        private void btnExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            // 1. Confirmação (Importante para não apagar sem querer!)
            var resultado = MessageBox.Show($"Deseja realmente excluir o produto '{txtNomeProduto.Text}'?",
                                           "Confirmar Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        // O comando DELETE remove a linha inteira baseada no nome
                        string query = "DELETE FROM produtos WHERE nome = @nome";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@nome", txtNomeProduto.Text);

                            int linhasAfetadas = cmd.ExecuteNonQuery();

                            if (linhasAfetadas > 0)
                            {
                                MessageBox.Show("Produto removido com sucesso!");
                                LimparCampos();
                            }
                            else
                            {
                                MessageBox.Show("Produto não encontrado no banco de dados.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir: " + ex.Message);
                }
            }
        }

        //Btn pra voltar a uc_config
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            // Volta para a tela de Perfil
            if (WindowHome.Instancia != null)
            {
                WindowHome.Instancia.ConteudoPrincipal.Content = new UC_Config();
            }
        }
    }
}