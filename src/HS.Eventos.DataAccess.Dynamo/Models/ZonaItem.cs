using Amazon.DynamoDBv2.DataModel;

namespace HS.Eventos.DataAccess.Dynamo.Models
{
    public class ZonaItem
    {
        [DynamoDBProperty("Id")]
        public string Id { get; set; } = string.Empty;

        [DynamoDBProperty("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [DynamoDBProperty("Precio")]
        public decimal Precio { get; set; }

        [DynamoDBProperty("Capacidad")]
        public int Capacidad { get; set; }

        [DynamoDBProperty("EventoId")]
        public string? EventoId { get; set; }
    }
}
