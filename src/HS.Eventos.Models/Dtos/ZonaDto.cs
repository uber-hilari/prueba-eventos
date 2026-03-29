using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HS.Eventos.Models.Dtos
{
    public sealed record ZonaDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Capacidad { get; set; }
        public int Disponible { get; set; }
    }
}
