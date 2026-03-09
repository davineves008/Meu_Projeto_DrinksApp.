using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Projeto_DrinksApp
{
    public class Produto : INotifyPropertyChanged
    {
        // Propriedades que refletem o Banco de Dados
        public int IdProduto { get; set; }
        public int IdFornecedor { get; set; }
        public int IdTipo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; } // SQL Decimal(10,2) vira decimal no C#
        public int Estoque { get; set; }
        public bool Ativo { get; set; }

        // Propriedade extra para o funcionamento do Carrinho (não precisa estar no SQL)
        private int _quantidade;
        public int Quantidade
        {
            get => _quantidade;
            set
            {
                if (_quantidade != value)
                {
                    _quantidade = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PrecoTotal)); // Notifica que o total mudou
                }
            }
        }

        // Propriedade calculada para exibir no Carrinho
        public decimal PrecoTotal => Quantidade * Preco;

        // Implementação da interface para atualização automática da tela
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}