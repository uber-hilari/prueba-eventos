using Amazon.SQS;
using HS.Eventos.Dominio.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Messaging.SQS
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqsMessageSender(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
            services.AddScoped<IMessageSender, MessageSender>();
            return services;
        }
    }
}
