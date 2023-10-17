using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public static class ConsumerBuilder
    {
        public static void ReceiveMessages(string routingKey)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                const string exchange = "topic_exchange";

                channel.ExchangeDeclare(
                    exchange: exchange,
                    type: ExchangeType.Topic);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(
                    queue: queueName,
                    exchange: exchange,
                    routingKey: routingKey);

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