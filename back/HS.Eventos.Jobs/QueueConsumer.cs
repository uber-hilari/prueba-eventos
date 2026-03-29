using Amazon.SQS;
using Amazon.SQS.Model;
using HS.Eventos.Dominio.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace HS.Eventos.Jobs
{
    public class QueueConsumer (IAmazonSQS client) : BackgroundService
    {
        const string QueueName = "PruebaEventosQueue";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueUrl = await GetQueueUrl();
            var reciveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20
            };
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await client.ReceiveMessageAsync(reciveMessageRequest, stoppingToken);
                if (response.Messages != null)
                {
                    foreach (var message in response.Messages)
                    {
                        Console.WriteLine($"Received message: {message.Body}");
                        await client.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
                    }
                }
            }
        }

        private async Task<string> GetQueueUrl()
        {
            try
            {
                var response = await client.GetQueueUrlAsync(QueueName);
                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                var response = await client.CreateQueueAsync(QueueName);
                return response.QueueUrl;
            }
        }
    }
}
