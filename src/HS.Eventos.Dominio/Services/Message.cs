using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Dominio.Services
{
    public sealed class Message<T>
    {
        public Guid MessageId { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime OcurredAt { get; set; }
        public Guid CorrelationId { get; set; }
        public int Version { get; set; } = 1;
        public required T Data { get; set; }
    }
}
