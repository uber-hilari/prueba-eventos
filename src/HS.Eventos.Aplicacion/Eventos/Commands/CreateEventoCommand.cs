using HS.Eventos.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion.Eventos.Commands
{
    public sealed record CreateEventoCommand : ICommand
    {
        public required string Nombre { get; init; }
        public DateTime Fecha { get; init; }
        public required string Lugar { get; init; }
        public IEnumerable<CreateZonaRequest> Zonas { get; set; } = [];
    }
}
