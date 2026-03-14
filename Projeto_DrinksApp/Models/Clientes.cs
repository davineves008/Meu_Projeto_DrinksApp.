using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DrinksApp.Models
{
    public class Clientes
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

   
        public string Senha { get; set; }
        public string  Usuario { get; set; }

        // Liga o objeto Endereco diretamente ao Cliente
       public Endereço EnderecoResidencial { get; set; }

    }
}
