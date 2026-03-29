#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    /// <summary>
    /// Pipeline behavior para solicitudes que devuelven respuesta.
    /// </summary>
    /// <typeparam name="TRequest">Tipo de la solicitud.</typeparam>
    /// <typeparam name="TResponse">Tipo de la respuesta.</typeparam>
    public interface IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        /// <summary>
        /// Ejecuta el behavior envolviendo la llamada al siguiente elemento de la cadena.
        /// </summary>
        /// <param name="request">Solicitud.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="next">Delegado que ejecuta el siguiente eslabón (handler o siguiente behavior).</param>
        Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Pipeline behavior para solicitudes que no devuelven respuesta.
    /// </summary>
    /// <typeparam name="TRequest">Tipo de la solicitud.</typeparam>
    public interface IPipelineBehavior<TRequest>
        where TRequest : ICommand
    {
        /// <summary>
        /// Ejecuta el behavior envolviendo la llamada al siguiente elemento de la cadena.
        /// </summary>
        /// <param name="request">Solicitud.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <param name="next">Delegado que ejecuta el siguiente eslabón (handler o siguiente behavior).</param>
        Task Handle(TRequest requestx, Func<Task> next, CancellationToken cancellationToken);
    }

}