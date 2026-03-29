using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Dominio.Entidades
{
    public class Zona : BaseEntity
    {
        public required string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Capacidad { get; set; }
    }
}
