#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    /// <summary>
    /// Define el mediador que enruta solicitudes a sus manejadores.
    /// </summary>
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
        Task Send(ICommand request, CancellationToken cancellationToken = default);
    }
}