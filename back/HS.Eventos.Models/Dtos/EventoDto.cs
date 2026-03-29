using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Models.Dtos
{
    public sealed record EventoDto
    {
        public string Id { get; init; } = string.Empty;
        public string Nombre { get; init; } = string.Empty;
        public DateTime Fecha { get; init; }
        public string Lugar { get; init; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public IEnumerable<ZonaDto> Zonas { get; set; } = [];
    }
}
