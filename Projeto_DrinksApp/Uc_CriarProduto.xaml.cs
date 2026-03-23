using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projeto_DrinksApp
{
    public partial class Uc_CriarProduto : UserControl
    {
        private string connString = @"Server=TQR216785\SQLEXPRESS;Database=DrinkApps;User Id=tds;Password=tds123;";

        // Controle do ID para saber se é Edição (ID > 0) ou Novo Cadastro (ID = 0)
        private int idProdutoSelecionado = 0;

        public Uc_CriarProduto()
        {
            InitializeComponent();

            // Eventos de validação de entrada
            txtPreco.PreviewTextInput += SomenteNumerosEPonto;
            txtEstoque.PreviewTextInput += SomenteNumeros;

            // Evento para busca automática ao perder o foco ou dar Enter
            txtNomeProduto.LostFocus += TxtNomeProduto_LostFocus;
            txtNomeProduto.KeyDown += (s, e) => { if (e.Key == Key.Enter) BuscarProdutoPorNome(); };
        }

        #region Buscas e Validações

        private void TxtNomeProduto_LostFocus(object sender, RoutedEventArgs e) => BuscarProdutoPorNome();

        //metodo que busca o produto no banco.
        private void BuscarProdutoPorNome()
        {
            if (string.IsNullOrWhiteSpace(txtNomeProduto.Text)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT id, preco, categoria, imagem, estoque FROM produtos WHERE nome = @nome";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtNomeProduto.Text.Trim());

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Produto encontrado! Preenchendo a tela:
                                idProdutoSelecionado = reader.GetInt32(0);
                                txtPreco.Text = reader.GetDecimal(1).ToString("F2");
                                txtImagem.Text = reader["imagem"].ToString();
                                txtEstoque.Text = reader["estoque"].ToString();

                                // Selecionar Categoria no ComboBox
                                string catBanco = reader["categoria"].ToString();
                                foreach (ComboBoxItem item in cbCategoria.Items)
                                {
                                    if (item.Content.ToString() == catBanco)
                                    {
                                        cbCategoria.SelectedItem = item;
                                        break;
                                    }
                                }

                                // Opcional: Mudar o texto do botão para indicar edição
                                btnSalvarProduto.Content = "ATUALIZAR";
                            }
                            else
                            {
                                // Se não encontrar, assume que é um novo produto (mantém ID 0)
                                btnSalvarProduto.Content = "SALVAR";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }

        //validação de numeros
        private void SomenteNumeros(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        //Validação de numero e ponto.
        private void SomenteNumerosEPonto(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9,. ]+").IsMatch(e.Text);
        }

        #endregion

        //Btn pra salvar o produto no banco.
        private void btnSalvarProduto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNomeProduto.Text) || string.IsNullOrWhiteSpace(txtPreco.Text))
                {
                    MessageBox.Show("Campos obrigatórios vazios!");
                    return;
                }

                decimal preco = decimal.Parse(txtPreco.Text.Replace(".", ","));
                int estoque = int.Parse(txtEstoque.Text);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query;

                    // Se idProdutoSelecionado > 0, faz UPDATE. Se for 0, faz INSERT.
                    if (idProdutoSelecionado > 0)
                        query = "UPDATE produtos SET nome=@nome, preco=@preco, categoria=@cat, imagem=@img, estoque=@estoque WHERE id=@id";
                    else
                        query = "INSERT INTO produtos (nome, preco, categoria, imagem, estoque) VALUES (@nome, @preco, @cat, @img, @estoque)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (idProdutoSelecionado > 0) cmd.Parameters.AddWithValue("@id", idProdutoSelecionado);

                        cmd.Parameters.AddWithValue("@nome", txtNomeProduto.Text.Trim());
                        cmd.Parameters.AddWithValue("@preco", preco);
                        cmd.Parameters.AddWithValue("@cat", (cbCategoria.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Geral");
                        cmd.Parameters.AddWithValue("@img", txtImagem.Text ?? "");
                        cmd.Parameters.AddWithValue("@estoque", estoque);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show(idProdutoSelecionado > 0 ? "Atualizado!" : "Cadastrado!");
                        LimparCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }

        //Btn pra excluir produto pelo id.
        private void btnExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            if (idProdutoSelecionado == 0)
            {
                MessageBox.Show("Busque um produto primeiro!");
                return;
            }

            if (MessageBox.Show("Excluir?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "DELETE FROM produtos WHERE id = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idProdutoSelecionado);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Removido!");
                        LimparCampos();
                    }
                }
            }
        }

        //metodo pra limpar os campos.
        private void LimparCampos()
        {
            idProdutoSelecionado = 0;
            txtNomeProduto.Clear();
            txtPreco.Clear();
            txtEstoque.Clear();
            txtImagem.Clear();
            cbCategoria.SelectedIndex = -1;
            btnSalvarProduto.Content = "SALVAR";
            txtNomeProduto.Focus();
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as WindowHome;
            if (window != null) window.ConteudoPrincipal.Content = null;
        }
    }
}