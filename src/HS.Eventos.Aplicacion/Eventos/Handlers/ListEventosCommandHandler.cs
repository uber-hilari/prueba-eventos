using HS.Eventos.Aplicacion.Eventos.Commands;
using HS.Eventos.Dominio.Repositories;
using HS.Eventos.Models.Dtos;
using HS.Eventos.Models.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Eventos.Aplicacion.Eventos.Handlers
{
    internal class ListEventosCommandHandler(
        IEventoRepository eventoRepository
    ) : HS.ICommandHandler<ListEventosCommand, ListResponse<EventoDto>>
    {
        public async Task<ListResponse<EventoDto>> Handle(ListEventosCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var eventos = await eventoRepository.ListAsync(request.ExclusiveStartKey, request.Limit, cancellationToken).ConfigureAwait(false);
            return eventos.MapperTo(e => new EventoDto
            {
                Id = e.Id.String(),
                Nombre = e.Nombre,
                Fecha = e.Fecha,
                Lugar = e.Lugar,
                Estado = e.Estado.ToString()
            });
        }
    }
}
