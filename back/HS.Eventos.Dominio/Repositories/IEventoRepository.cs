using HS.Eventos.Dominio.Entidades;
using HS.Eventos.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Eventos.Dominio.Repositories
{
    public interface IEventoRepository
    {
        Task SaveAsync(Evento evento, CancellationToken cancellationToken = default);
        Task<Evento?> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ListResponse<Evento>> ListAsync(string? exclusiveStartKey = null, int? limit = null, CancellationToken cancellationToken = default);
    }
}
