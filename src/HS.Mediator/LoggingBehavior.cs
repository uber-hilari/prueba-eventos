using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    public sealed class LoggingBehavior<TRequest, TResponse>(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var requestName = typeof(TRequest).Name;

            // Calcular la serialización solo si es necesario para evitar CA1873
            string? requestJson = null;
            if (logger.IsEnabled(LogLevel.Information))
            {
                requestJson = LoggingHelper.SerializeRequest(request);
                logger.LogInformation("Handling {RequestName} - Request: {RequestJson}", requestName, requestJson);
            }

            var sw = Stopwatch.StartNew();
            try
            {
                var response = await next();
                sw.Stop();

                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds} ms", requestName, sw.Elapsed.TotalMilliseconds);
                }

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();

                // Si no se calculó antes, hacerlo solo si el nivel Error está habilitado
                if (logger.IsEnabled(LogLevel.Error))
                {
                    requestJson ??= LoggingHelper.SerializeRequest(request);
                    logger.LogError(ex, "Exception handling {RequestName} after {ElapsedMilliseconds} ms - Request: {RequestJson}", requestName, sw.Elapsed.TotalMilliseconds, requestJson);
                }

                throw;
            }
        }
    }

    public sealed class LoggingBehavior<TRequest>(
        ILogger<LoggingBehavior<TRequest>> logger
    ) : IPipelineBehavior<TRequest>
        where TRequest : ICommand
    {
        public async Task Handle(TRequest request, Func<Task> next, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var requestName = typeof(TRequest).Name;

            // Calcular la serialización solo si es necesario para evitar CA1873
            string? requestJson = null;
            if (logger.IsEnabled(LogLevel.Information))
            {
                requestJson = LoggingHelper.SerializeRequest(request);
                logger.LogInformation("Handling {RequestName} - Request: {RequestJson}", requestName, requestJson);
            }

            var sw = Stopwatch.StartNew();
            try
            {
                await next();
                sw.Stop();

                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds} ms", requestName, sw.Elapsed.TotalMilliseconds);
                }

            }
            catch (Exception ex)
            {
                sw.Stop();

                // Si no se calculó antes, hacerlo solo si el nivel Error está habilitado
                if (logger.IsEnabled(LogLevel.Error))
                {
                    requestJson ??= LoggingHelper.SerializeRequest(request);
                    logger.LogError(ex, "Exception handling {RequestName} after {ElapsedMilliseconds} ms - Request: {RequestJson}", requestName, sw.Elapsed.TotalMilliseconds, requestJson);
                }

                throw;
            }
        }
    }

    internal static class LoggingHelper
    {
        static readonly JsonSerializerOptions serializerOptions = new()
        {
            WriteIndented = false
        };

        public static string SerializeRequest<TRequest>(TRequest request)
        {
            try
            {
                return JsonSerializer.Serialize(request, serializerOptions);
            }
            catch
            {
                return request?.ToString() ?? string.Empty;
            }
        }
    }
}