using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    public class EventSender (
        IServiceProvider serviceProvider
    ) : IEventSender
    {

        public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            var eventType = @event.GetType();
            var handlerInterfaceType = typeof(IEventHandler<>).MakeGenericType(eventType);

            var handlers = serviceProvider.GetServices(handlerInterfaceType).ToList();

            if (handlers.Count == 0)
                return; // no hay handlers registrados, no hacer nada

            foreach (var handler in handlers)
            {
                var handleMethod = FindHandleMethod(handler!, eventType)
                    ?? throw new InvalidOperationException($"Handler for event '{eventType.FullName}' does not expose a suitable Handle method");

                var taskObj = handleMethod.Invoke(handler, [@event, cancellationToken])
                    ?? throw new InvalidOperationException("Handler invocation returned null");

                if (taskObj is not Task task)
                    throw new InvalidOperationException("Event handler returned a non-task result");

                await task;
            }
        }

        // Busca el método Handle aceptando (TRequest, CancellationToken).
        private static MethodInfo? FindHandleMethod(object handlerInstance, Type requestType)
        {
            var binding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            // 1) Buscar método concreto con la firma esperada
            var method = handlerInstance.GetType().GetMethod("Handle", binding, null, [requestType, typeof(CancellationToken)], null);
            if (method is not null)
                return method;

            // 2) Buscar implementación explícita de la interfaz (nombre mangled)
            //    Recorremos las interfaces del tipo y comprobamos sus mapas de método.
            foreach (var iface in handlerInstance.GetType().GetInterfaces())
            {
                if (!iface.IsGenericType)
                    continue;

                var def = iface.GetGenericTypeDefinition();
                if (def == typeof(ICommandHandler<,>) || def == typeof(ICommandHandler<>) || def == typeof(IEventHandler<>))
                {
                    var map = handlerInstance.GetType().GetInterfaceMap(iface);
                    for (int i = 0; i < map.TargetMethods.Length; i++)
                    {
                        var target = map.TargetMethods[i];
                        if (target.Name.Contains("Handle", StringComparison.OrdinalIgnoreCase))
                        {
                            // Confirmar que la firma coincide por parámetros
                            var parms = target.GetParameters();
                            if (parms.Length == 2 &&
                                parms[0].ParameterType == requestType &&
                                parms[1].ParameterType == typeof(CancellationToken))
                            {
                                return target;
                            }
                        }
                    }
                }
            }

            // 3) Fallbacks: (object, CancellationToken) o (requestType)
            method = handlerInstance.GetType().GetMethod("Handle", binding, null, [typeof(object), typeof(CancellationToken)], null);
            if (method is not null)
                return method;

            method = handlerInstance.GetType().GetMethod("Handle", binding, null, [requestType], null);
            return method;
        }
    }
}
