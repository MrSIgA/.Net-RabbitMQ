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

        const string exchange = "fanout_exchange";

        channel.ExchangeDeclare(
            exchange: exchange,
            type: ExchangeType.Fanout);

        var message = $"Hello word! {counter}";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: exchange,
            routingKey: string.Empty,
            body: body);

        Console.WriteLine($"Message is sent. {counter}");

        counter++;
    }
}