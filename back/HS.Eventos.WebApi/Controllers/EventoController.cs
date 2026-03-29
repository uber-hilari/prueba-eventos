using HS.Eventos.Aplicacion.Eventos.Commands;
using HS.Eventos.Models.Requests;
using HS.Eventos.Models.Dtos;
using HS.Eventos.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


namespace HS.Eventos.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController(IMediator mediator, IDistributedCache cache, IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CrearEvento([FromBody] CreateEventoRequest request)
        {
            var command = new CreateEventoCommand
            {
                Nombre = request.Nombre!,
                Fecha = request.Fecha ?? DateTime.Today,
                Lugar = request.Lugar!,
                Zonas = request.Zonas
            };

            await mediator.Send(command);

            return Created();
        }

        private async Task<T?> GetValueFromCache<T>(string cacheKey)
        {
            if (!configuration.GetValue<bool>("Cache:Enabled", true))
            {
                return default;
            }

            try
            {
                var cached = await cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cached))
                {
                    var cachedResponse = JsonSerializer.Deserialize<T>(cached, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return cachedResponse;
                }

                return default;
            }
            catch
            {
                return default;
            }
        }

        private async Task SetValueToCache<T>(string cacheKey, T data)
        {
            if (!configuration.GetValue<bool>("Cache:Enabled", true))
            {
                return;
            }

            try
            {
                var expiration = configuration.GetValue<int>("Cache:ExpirationMinutes", 5);
                var serialized = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                await cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiration) });
            }
            catch
            {
                // Ignorar errores de cache
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEventos([FromQuery] string? exclusiveStartKey = null, [FromQuery] int? limit = null)
        {
            var cacheKey = $"eventos:{exclusiveStartKey ?? string.Empty}:{limit?.ToString() ?? string.Empty}";

            var valueCached = await GetValueFromCache<ListResponse<EventoDto>>(cacheKey);
            if (valueCached is not null)
            {
                return Ok(valueCached);
            }

            var command = new ListEventosCommand
            {
                ExclusiveStartKey = exclusiveStartKey,
                Limit = limit
            };

            var eventos = await mediator.Send(command);

            await SetValueToCache(cacheKey, eventos);

            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerEvento(string id)
        {
            var guid = id.Guid();
            var cacheKey = $"evento:{guid}";

            var valueCached = await GetValueFromCache<EventoDto>(cacheKey);
            if (valueCached is not null)
            {
                return Ok(valueCached);
            }

            var command = new ReadEventoCommand { Id = guid };
            var evento = await mediator.Send(command);

            if (evento is not null)
            {
                await SetValueToCache(cacheKey, evento);
            }

            return Ok(evento);
        }
    }
}
