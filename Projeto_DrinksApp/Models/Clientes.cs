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
        public string Cidade { get; set; } // Adicionado pois existe na sua tabela Clientes
        public string Usuario { get; set; } // Corrigido para 'Usuario' conforme o SQL
        public string Senha { get; set; }
        public int Nivel { get; set; } // 0 = User, 1 = Admin

        // Propriedade para guardar o resumo do último pedido
        public string UltimoPedidoDescricao { get; set; }
        public decimal UltimoPedidoValor { get; set; }
        public DateTime? UltimoPedidoData { get; set; }

        // Liga o objeto Endereco diretamente ao Cliente
        public Endereço EnderecoResidencial { get; set; }

    }
}
