using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Dominio.Entidades
{
    public class Evento : BaseEntity
    {
        public required string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public required string Lugar { get; set; }
        public EstadoEvento Estado { get; set; }
        public virtual ICollection<Zona> Zonas { get; set; } = [];
    }

    public enum EstadoEvento : byte
    {
        Registrado = 1,
        Publicado = 2,
        Realizado = 3,
        Cancelado = 4
    }
}
