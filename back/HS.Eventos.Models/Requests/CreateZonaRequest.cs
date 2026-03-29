using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HS.Eventos.Models.Requests
{
    public sealed record CreateZonaRequest
    {
        [property: Required(ErrorMessage = "La nombre es requerido.")]
        public string? Nombre { get; set; }

        [property: Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        [property: Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser al menos 1.")]
        public int Capacidad { get; set; }
    }
}
