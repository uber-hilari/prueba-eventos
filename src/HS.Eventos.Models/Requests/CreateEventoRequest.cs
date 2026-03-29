using HS.Eventos.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HS.Eventos.Models.Requests
{
    public class CreateEventoRequest
    {
        [property: Required(ErrorMessage = "El nombre es requerido")]
        public string? Nombre { get; set; }

        [property: Required(ErrorMessage = "La fecha es requerida")]
        public DateTime? Fecha { get; set; }

        [property: Required(ErrorMessage = "El lugar es requerido")]
        public string? Lugar { get; set; }

        public IEnumerable<CreateZonaRequest> Zonas { get; set; } = [];
    }
}
