using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    const string exchange = "fanout_exchange";

    channel.ExchangeDeclare(
        exchange: exchange,
        type: ExchangeType.Fanout);

    var queueName = channel.QueueDeclare().QueueName;

    channel.QueueBind(
        queue: queueName,
        exchange: exchange,
        routingKey: string.Empty);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, e) =>
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Console.WriteLine($"Recived message - {message}");
    };

    channel.BasicConsume(
        queue: queueName,
        autoAck: true,
        consumer: consumer);

    Console.WriteLine($"Attach consumer to queue - {queueName}");

    Console.ReadLine();
}