using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DrinksApp.Models
{
    public  class Endereço
    {
        // Chave primária da tabela
        public int IdEndereco { get; set; }

        // Chave estrangeira que liga ao Cliente
        public int IdCliente { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; } // Usamos string porque existem números com letras (ex: 10A)
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

        // Propriedade opcional para facilitar a exibição
        public string EnderecoCompleto => $"{Logradouro}, {Numero} - {Bairro}, {Cidade}/{Estado}";
    }
}
