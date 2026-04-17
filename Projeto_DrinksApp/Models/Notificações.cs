using System;
using System.Collections.ObjectModel;

namespace Projeto_DrinksApp.Models
{


    // Serviço estático acessível em qualquer lugar do app
    public static class NotificacaoService
    {
        public static ObservableCollection<NotificacaoItem> Notificacoes { get; }
            = new ObservableCollection<NotificacaoItem>();

        public static void Adicionar(string titulo, string mensagem)
        {
            Notificacoes.Insert(0, new NotificacaoItem
            {
                Titulo = titulo,
                Mensagem = mensagem,
                Horario = DateTime.Now.ToString("HH:mm")
            });
        }
    }
}