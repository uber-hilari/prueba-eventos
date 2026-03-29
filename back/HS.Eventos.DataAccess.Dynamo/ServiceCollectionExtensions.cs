using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using HS.Eventos.Dominio.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.DataAccess.Dynamo
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessForDynamo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();

            services.AddTransient<IEventoRepository, Repositories.EventoRepository>();

            return services;
        }
    }
}
