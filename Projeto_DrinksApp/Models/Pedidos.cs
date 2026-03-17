using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_DrinksApp.Models
{
    // Deixe apenas uma classe pública principal
    public class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacoes { get; set; }

        // A conexão com o objeto Cliente (verifique se sua classe de cliente é 'Cliente' ou 'Clientes')
        public Clientes Cliente { get; set; }

        // Propriedade para o texto do status
        public string StatusTexto { get; set; }
    }
}