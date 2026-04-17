using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Projeto_DrinksApp.Models;

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
            txtNomeProduto.KeyDown += (s, e) => { if (e.Key == Key.Enter) BuscarProduto(); };

        }

        //metodo que seleciona a categoria certa;
        private void SelecionarCategoriaNoCombo(int idTipo)
        {
            // Faz o caminho inverso: recebe o número do banco e seleciona o texto no combo
            foreach (ComboBoxItem item in cbCategoria.Items)
            {
                if (ConverterCategoriaParaId(item.Content.ToString()) == idTipo)
                {
                    cbCategoria.SelectedItem = item;
                    break;
                }
            }
        }



        #region Buscas e Validações

        private void TxtNomeProduto_LostFocus(object sender, RoutedEventArgs e) => BuscarProduto();

        //metodo que busca o produto no banco.
        private void BuscarProduto()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    // Busca pelo nome exato
                    string sql = "SELECT * FROM Produtos WHERE nomedoproduto = @nome";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtNomeProduto.Text);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            // Preenche os campos com o que achou no banco
                            idProdutoSelecionado = (int)dr["idproduto"];
                            txtPreco.Text = dr["precounitario"].ToString();
                            txtEstoque.Text = dr["estoque"].ToString();

                            // Ajusta o ComboBox (idtipo)
                            int idTipo = (int)dr["idtipo"];
                            SelecionarCategoriaNoCombo(idTipo);

                            MessageBox.Show("Produto carregado!");
                        }
                        else
                        {
                            MessageBox.Show("Produto não encontrado.");
                            idProdutoSelecionado = 0;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
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
                Produto p = new Produto();
                p.Nome = txtNomeProduto.Text;
                p.Preco = decimal.Parse(txtPreco.Text);
                p.Estoque = int.Parse(txtEstoque.Text);

                // AQUI É ONDE VOCÊ USA O MÉTODO! 
                // Ao adicionar esta linha, o método ConverterCategoriaParaId vai acender.
                if (cbCategoria.SelectedItem is ComboBoxItem itemSelecionado)
                {
                    string categoriaTexto = itemSelecionado.Content.ToString();
                    p.IdTipo = ConverterCategoriaParaId(categoriaTexto);
                }

                ProdutoRepositorio repo = new ProdutoRepositorio();
                repo.CadastrarProduto(p);

                MessageBox.Show("Produto salvo com sucesso!");
                BtnVoltar_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }

        // O método abaixo deixará de ficar cinza assim que você salvar o arquivo
        private int ConverterCategoriaParaId(string categoriaTexto)
        {
            switch (categoriaTexto)
            {
                case "Cerveja": return 6;
                case "Whisky": return 10;
                case "Lanches": return 2;
                case "Vinho": return 9;
                case "Destilados": return 11;
                default: return 1;
            }
        }

        //Btn pra excluir produto pelo id.
        private void btnExcluirProduto_Click(object sender, RoutedEventArgs e)
        {
            // 1. Verifica se existe um ID carregado (vindo da busca que fizemos)
            if (idProdutoSelecionado == 0)
            {
                MessageBox.Show("Por favor, busque um produto pelo nome primeiro para poder excluí-lo!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 2. Confirmação para evitar exclusões acidentais
            var resultado = MessageBox.Show($"Deseja realmente excluir o produto '{txtNomeProduto.Text}' permanentemente?",
                                            "Confirmação de Exclusão",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    // Use a sua string de conexão (ex: Conexao.StringConexao ou connString)
                    using (SqlConnection conn = new SqlConnection(Conexao.stringConexao))
                    {
                        conn.Open();

                        // IMPORTANTE: Usei 'idproduto' porque é o nome que aparece na foto do seu SQL
                        string query = "DELETE FROM Produtos WHERE idproduto = @id";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // Passamos o ID que foi guardado na variável global da tela
                            cmd.Parameters.AddWithValue("@id", idProdutoSelecionado);

                            int linhasAfetadas = cmd.ExecuteNonQuery();

                            if (linhasAfetadas > 0)
                            {
                                MessageBox.Show("Produto removido com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                                // Limpa a tela para a próxima operação
                                LimparCampos();
                            }
                            else
                            {
                                MessageBox.Show("O produto não foi encontrado no banco de dados ou já foi removido.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro técnico ao excluir: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
            cbCategoria.SelectedIndex = -1;
            btnSalvarProduto.Content = "SALVAR";
            txtNomeProduto.Focus();
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            var principal = Window.GetWindow(this) as WindowHome;

            if (principal != null)
            {
                principal.ConteudoPrincipal.Content = new UC_Config();
            }
        }
    }
}