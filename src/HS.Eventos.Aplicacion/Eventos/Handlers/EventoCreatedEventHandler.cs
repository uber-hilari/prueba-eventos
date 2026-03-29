using HS.Eventos.Aplicacion.Eventos.Events;
using HS.Eventos.Dominio.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion.Eventos.Handlers
{
    public class EventoCreatedEventHandler(
        IMessageSender messageSender
    ) : IEventHandler<EventoCreatedEvent>
    {
        public async Task Handle(EventoCreatedEvent @event, CancellationToken cancellationToken)
        {
            var message = new Message<EventoCreatedEvent>
            {
                MessageId = Guid.NewGuid(),
                EventId = Guid.NewGuid(),
                Name = nameof(EventoCreatedEvent),
                OcurredAt = DateTime.UtcNow,
                CorrelationId = Guid.NewGuid(),
                Version = 1,
                Data = @event
            };

            await messageSender.Send(message);
        }
    }
}
