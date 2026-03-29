using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace HS.Eventos.DataAccess.Dynamo.Models
{
    [DynamoDBTable("Eventos")]
    public class EventoItem
    {
        [DynamoDBHashKey("Id")]
        public string Id { get; set; } = string.Empty;

        [DynamoDBProperty("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [DynamoDBProperty("Fecha")]
        public string Fecha { get; set; } = string.Empty; // ISO 8601

        [DynamoDBProperty("Lugar")]
        public string Lugar { get; set; } = string.Empty;

        [DynamoDBProperty("Estado")]
        public byte Estado { get; set; }

        [DynamoDBProperty("Zonas")]
        public List<ZonaItem> Zonas { get; set; } = [];
    }
}
