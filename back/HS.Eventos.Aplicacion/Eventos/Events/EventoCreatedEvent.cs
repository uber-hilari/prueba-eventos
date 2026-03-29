using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion.Eventos.Events
{
    public sealed record EventoCreatedEvent : IEvent
    {
        public Guid EventoId { get; init; }
    }
}
