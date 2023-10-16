using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    var counter = 0;
    while (true)
    {
        Thread.Sleep(1000);
        const string queue = "dev-queue";

        channel.QueueDeclare(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var message = $"Hello word! {counter}";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queue,
            body: body);

        Console.WriteLine($"Message is sent. {counter}");

        counter++;
    }
}