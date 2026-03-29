using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Dominio.Services
{
    public interface IMessageSender
    {
        Task<string> Send<T>(Message<T> msg);
    }
}
