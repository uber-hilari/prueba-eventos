using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using HS.Eventos.DataAccess.Dynamo.Mappers;
using HS.Eventos.DataAccess.Dynamo.Models;
using HS.Eventos.Dominio.Entidades;
using HS.Eventos.Dominio.Repositories;
using HS.Eventos.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Eventos.DataAccess.Dynamo.Repositories
{
    internal class EventoRepository(IDynamoDBContext context) : IEventoRepository
    {
        public async Task SaveAsync(Evento evento, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await context.SaveAsync(EventoMapper.MapToEventoItem(evento), cancellationToken);
        }

        public async Task<Evento?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var item = await context.LoadAsync<EventoItem>(id.ToString(), cancellationToken);
            if (item == null) return null;
            return EventoMapper.MapToEvento(item);
        }

        public async Task<ListResponse<Evento>> ListAsync(string? exclusiveStartKey = null, int? limit = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Use native DynamoDB pagination via ScanSearch.GetNextSetAsync.
            var operationConfig = new ScanOperationConfig()
            {
                PaginationToken = exclusiveStartKey,
                Limit = limit ?? 15
            };

            var search = context.GetTargetTable<EventoItem>().Scan(operationConfig);
            var nextSet = await search.GetNextSetAsync(cancellationToken);
            var items = nextSet.Select(s => context.FromDocument<EventoItem>(s));
            return new ListResponse<Evento>
            {
                Items = [.. items.Select(EventoMapper.MapToEvento)],
                PaginationToken = search.PaginationToken
            };
        }
    }
}
