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
                _quantidade = value;
                OnPropertyChanged("Quantidade");
                OnPropertyChanged("PrecoTotal"); // Avisa que o preço total mudou também!
            }
        }

    
        public decimal PrecoTotal => Preco * Quantidade;

        //Evento que permite atualizar caso um nome ou um valor mude.
        public event PropertyChangedEventHandler PropertyChanged;

        //avisa quando houver uma mudança no codigo xaml.
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}