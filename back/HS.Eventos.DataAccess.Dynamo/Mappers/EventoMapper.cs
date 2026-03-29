using HS.Eventos.DataAccess.Dynamo.Models;
using HS.Eventos.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.DataAccess.Dynamo.Mappers
{
    internal static class EventoMapper
    {
        public static Evento MapToEvento(EventoItem item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            return new Evento
            {
                Id = Guid.Parse(item.Id),
                Nombre = item.Nombre,
                Fecha = DateTime.Parse(item.Fecha),
                Lugar = item.Lugar,
                Estado = (EstadoEvento)item.Estado,
                Zonas = item.Zonas?.Select(z => new Zona
                {
                    Id = Guid.Parse(z.Id),
                    Nombre = z.Nombre,
                    Precio = z.Precio,
                    Capacidad = z.Capacidad
                }).ToList() ?? []
            };
        }

        public static EventoItem MapToEventoItem(Evento evento)
        {
            ArgumentNullException.ThrowIfNull(evento, nameof(evento));
            return new EventoItem
            {
                Id = evento.Id.ToString(),
                Nombre = evento.Nombre,
                Fecha = evento.Fecha.ToString("o"),
                Lugar = evento.Lugar,
                Estado = (byte)evento.Estado,
                Zonas = evento.Zonas?.Select(z => new ZonaItem
                {
                    Id = z.Id.ToString(),
                    Nombre = z.Nombre,
                    Precio = z.Precio,
                    Capacidad = z.Capacidad,
                    EventoId = evento.Id.ToString()
                }).ToList() ?? []
            };
        }
    }
}
