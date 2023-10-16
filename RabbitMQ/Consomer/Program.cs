using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    const string queue = "dev-queue";

    channel.QueueDeclare(
        queue: queue,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, e) =>
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Console.WriteLine($"Recived message - {message}");
    };

    channel.BasicConsume(
        queue: queue,
        autoAck: true,
        consumer: consumer);

    Console.WriteLine($"Attach consumer to queue - {queue}");

    Console.ReadLine();
}