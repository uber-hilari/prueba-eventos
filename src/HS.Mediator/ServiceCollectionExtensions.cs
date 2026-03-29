using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace HS
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                assemblies = [Assembly.GetCallingAssembly()];

            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IEventSender, EventSender>();

            foreach (var assembly in assemblies)
            {
                var handlerTypes = assembly.GetTypes()
                    .Where(t => t is { IsClass: true, IsAbstract: false } &&
                               t.GetInterfaces().Any(i =>
                                   i.IsGenericType && (
                                       i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                                       i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                                       i.GetGenericTypeDefinition() == typeof(IEventHandler<>) ||
                                       i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>) ||
                                       i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<>)
                                   )))
                    .ToList();

                foreach (var handlerType in handlerTypes)
                {
                    var interfaces = handlerType.GetInterfaces()
                        .Where(i => i.IsGenericType && (
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                        ));

                    foreach (var @interface in interfaces)
                    {
                        services.AddTransient(@interface, handlerType);
                    }

                    interfaces = handlerType.GetInterfaces()
                        .Where(i => i.IsGenericType && (
                            i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>) ||
                            i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<>)
                        ));

                    foreach (var @interface in interfaces)
                    {
                        services.AddScoped(@interface, handlerType);
                    }
                }
            }

            return services;
        }
    }

}