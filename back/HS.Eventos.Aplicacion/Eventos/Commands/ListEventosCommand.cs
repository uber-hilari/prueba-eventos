using HS.Eventos.Models.Dtos;
using HS.Eventos.Models.Responses;
using System.Collections.Generic;

namespace HS.Eventos.Aplicacion.Eventos.Commands
{
    public sealed record ListEventosCommand : HS.ICommand<ListResponse<EventoDto>>
    {
        public string? ExclusiveStartKey { get; init; }
        public int? Limit { get; init; }
    }
}
