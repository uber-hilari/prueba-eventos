#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    /// <summary>
    /// Define un manejador para una solicitud que devuelve respuesta.
    /// </summary>
    public interface ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Define un manejador para una solicitud que no devuelve respuesta.
    /// </summary>
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand request, CancellationToken cancellationToken);
    }

}