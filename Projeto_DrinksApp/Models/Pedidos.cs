using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DrinksApp.Models
{
    internal class Pedidos
    {
        public class Pedido
        {
            public int IdPedido { get; set; }
            public int IdCliente { get; set; }
            public DateTime DataPedido { get; set; }
            public int IdStatus { get; set; }
            public decimal ValorTotal { get; set; }
            public string Observacoes { get; set; }

            // Propriedades auxiliares (opcional)
            // Para exibir o nome do status em vez do número (ex: 1 = Pendente, 2 = Pago)
            public string StatusTexto { get; set; }
        }
    }
}
