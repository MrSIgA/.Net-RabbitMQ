using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public static class ConsumerBuilder
    {
        public static void ReceiveMessages(Dictionary<string, object> headers)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                const string exchange = "headers_exchange";

                channel.ExchangeDeclare(
                    exchange: exchange,
                    type: ExchangeType.Headers);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(
                    queue: queueName,
                    exchange: exchange,
                    routingKey: string.Empty,
                    arguments: headers);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Recived message - {message}. QueueName - {queueName}");
                };

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine($"Attach consumer to queue - {queueName}");

                Console.ReadLine();
            }
        }
    }
}