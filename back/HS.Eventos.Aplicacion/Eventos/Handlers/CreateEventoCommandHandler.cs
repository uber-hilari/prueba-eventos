using HS.Eventos.Aplicacion.Eventos.Commands;
using HS.Eventos.Aplicacion.Eventos.Events;
using HS.Eventos.Dominio.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion.Eventos.Handlers
{
    public class CreateEventoCommandHandler (
        IEventoRepository eventoRepository,
        IEventSender eventSender
    ) : ICommandHandler<CreateEventoCommand>
    {
        public async Task Handle(CreateEventoCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var evento = new Dominio.Entidades.Evento
            {
                Id = Guid.CreateVersion7(),
                Nombre = request.Nombre,
                Fecha = request.Fecha,
                Lugar = request.Lugar,
                Estado = Dominio.Entidades.EstadoEvento.Registrado,
                Zonas = [..request.Zonas.Select(z => new Dominio.Entidades.Zona
                {
                    Id = Guid.CreateVersion7(),
                    Nombre = z.Nombre ?? string.Empty,
                    Precio = z.Precio,
                    Capacidad = z.Capacidad
                })]
            };

            await eventoRepository.SaveAsync(evento, cancellationToken);

            await eventSender.Publish(new EventoCreatedEvent
            {
                EventoId = evento.Id
            }, cancellationToken);
        }
    }
}
