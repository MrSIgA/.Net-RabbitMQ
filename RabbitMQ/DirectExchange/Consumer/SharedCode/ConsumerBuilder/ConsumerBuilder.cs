using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public static class ConsumerBuilder
    {
        public static void ReceiveMessages(List<string> routingKeys)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                const string exchange = "direct_exchange";

                channel.ExchangeDeclare(
                    exchange: exchange,
                    type: ExchangeType.Direct);

                var queueName = channel.QueueDeclare().QueueName;

                foreach (var key in routingKeys)
                {
                    channel.QueueBind(
                        queue: queueName,
                        exchange: exchange,
                        routingKey: key);
                }

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