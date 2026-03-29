#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}