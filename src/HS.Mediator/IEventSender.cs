#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public interface IEventSender
    {
        // Publicación de eventos (puede tener múltiples handlers)
        Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent;
    }
}
