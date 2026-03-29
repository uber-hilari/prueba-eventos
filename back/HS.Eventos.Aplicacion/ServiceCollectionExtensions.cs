using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Aplicacion
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAplicacion(this IServiceCollection services)
        {
            services.AddMediator(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
