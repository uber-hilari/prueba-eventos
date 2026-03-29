using Amazon.SQS;
using Amazon.SQS.Model;
using HS.Eventos.Dominio.Services;

namespace HS.Eventos.Messaging.SQS
{
    public class MessageSender (
        IAmazonSQS client
    ) : IMessageSender
    {
        const string QueueName = "PruebaEventosQueue";

        public async Task<string> Send<T>(Message<T> msg)
        {
            var queueUrl = await GetQueueUrl();
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = System.Text.Json.JsonSerializer.Serialize(msg)
            };
            var response = await client.SendMessageAsync(sendMessageRequest);
            return response.MessageId;
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
