using HS.Eventos.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion.Eventos.Commands
{
    public sealed record ReadEventoCommand : ICommand<EventoDto>
    {
        public Guid Id { get; init; }
    }
}
