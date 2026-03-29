using HS.Eventos.Aplicacion.Eventos.Commands;
using HS.Eventos.Dominio.Entidades;
using HS.Eventos.Dominio.Repositories;
using HS.Eventos.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion.Eventos.Handlers
{
    internal class ReadEventoCommandHandler(
        IEventoRepository eventoRepository
    ) : ICommandHandler<ReadEventoCommand, EventoDto>
    {
        public async Task<EventoDto> Handle(ReadEventoCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var evento = await eventoRepository.GetAsync(request.Id, cancellationToken) ??
                throw new NotFoundEntityException(nameof(Evento));
            return new EventoDto
            {
                Id = evento.Id.String(),
                Nombre = evento.Nombre,
                Fecha = evento.Fecha,
                Lugar = evento.Lugar,
                Estado = evento.Estado.ToString(),
                Zonas = [.. evento.Zonas.Select(z => new ZonaDto
                {
                    Id = z.Id.String(),
                    Nombre = z.Nombre,
                    Precio = z.Precio,
                    Capacidad = z.Capacidad,
                    Disponible = z.Capacidad
                })]
            };
        }
    }
}
