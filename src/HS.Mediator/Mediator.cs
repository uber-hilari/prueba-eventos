using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    public class Mediator(
        IServiceScopeFactory scopeFactory
    ) : IMediator
    {
        public async Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();
            var handlerInterfaceType = typeof(ICommandHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var behaviorInterfaceType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));

            using var scope = scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService(handlerInterfaceType)
                ?? throw new InvalidOperationException($"No handler registered for request type '{requestType.FullName}'");

            var handleMethod = FindHandleMethod(handler, requestType)
                ?? throw new InvalidOperationException($"Handler for '{requestType.FullName}' does not have a suitable Handle method");

            // delegados finales que invocan el handler
            Func<Task<TResponse>> handlerInvoke = async () =>
            {
                var taskObj = handleMethod.Invoke(handler, [request, cancellationToken])
                    ?? throw new InvalidOperationException("Handler invocation returned null");

                if (taskObj is not Task<TResponse> typedTask)
                    throw new InvalidOperationException($"Handler returned a task that cannot be cast to Task<{typeof(TResponse).Name}>");

                return await typedTask.ConfigureAwait(false);
            };

            // Resolver behaviours (si hay registros abiertos/registrados) y componer la cadena
            var behaviors = scope.ServiceProvider.GetServices(behaviorInterfaceType).ToList();
            if (behaviors.Count > 0)
            {
                // Componer en orden: el primer behavior registrado será el primero en ejecutarse
                foreach (var behavior in behaviors.AsEnumerable().Reverse())
                {
                    var behaviorObj = behavior;
                    var method = behaviorInterfaceType.GetMethod("Handle") ?? throw new InvalidOperationException("Behavior does not expose Handle method");

                    var next = handlerInvoke; // captura
                    handlerInvoke = () =>
                    {
                        // Invocar behavior.Handle(request, cancellationToken, next)
                        var resultObj = method.Invoke(behaviorObj, [request, next, cancellationToken]);
                        if (resultObj is not Task<TResponse> resultTask)
                            throw new InvalidOperationException("Behavior returned a non compatible task for response pipeline");
                        return resultTask;
                    };
                }
            }

            return await handlerInvoke().ConfigureAwait(false);
        }

        public async Task Send(ICommand request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();
            var handlerInterfaceType = typeof(ICommandHandler<>).MakeGenericType(requestType);
            var behaviorInterfaceType = typeof(IPipelineBehavior<>).MakeGenericType(requestType);

            using var scope = scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService(handlerInterfaceType)
                ?? throw new InvalidOperationException($"No handler registered for request type '{requestType.FullName}'");

            var handleMethod = FindHandleMethod(handler, requestType)
                ?? throw new InvalidOperationException($"Handler for '{requestType.FullName}' does not have a suitable Handle method");

            // delegados finales que invocan el handler (void)
            Func<Task> handlerInvoke = async () =>
            {
                var taskObj = handleMethod.Invoke(handler, [request, cancellationToken])
                    ?? throw new InvalidOperationException("Handler invocation returned null");

                if (taskObj is not Task task)
                    throw new InvalidOperationException("Handler returned a non-task result for a void request");

                await task.ConfigureAwait(false);
            };

            // Resolver behaviours y componer la cadena para void pipeline
            var behaviors = scope.ServiceProvider.GetServices(behaviorInterfaceType).ToList();
            if (behaviors.Count > 0)
            {
                foreach (var behavior in behaviors.AsEnumerable().Reverse())
                {
                    var behaviorObj = behavior;
                    var method = behaviorInterfaceType.GetMethod("Handle") ?? throw new InvalidOperationException("Behavior does not expose Handle method");

                    var next = handlerInvoke;
                    handlerInvoke = () =>
                    {
                        var resultObj = method.Invoke(behaviorObj, [request, next, cancellationToken]);
                        if (resultObj is not Task resultTask)
                            throw new InvalidOperationException("Behavior returned a non-task result for void pipeline");
                        return resultTask;
                    };
                }
            }

            await handlerInvoke().ConfigureAwait(false);
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