using System.Text;
using RabbitMQ.Client;

Task.Run(PublishMessages());

Console.ReadLine();

static Func<Task> PublishMessages()
{
    return () =>
    {
        var cars = new List<string> { "Tesla", "BMW", "Audi" };
        var colors = new List<string> { "black", "white", "red" };

        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var counter = 0;
            while (true)
            {
                var timeToSleep = new Random().Next(1000, 3000);
                Thread.Sleep(timeToSleep);

                const string exchange = "topic_exchange";

                channel.ExchangeDeclare(
                    exchange: exchange,
                    type: ExchangeType.Topic);

                var random = new Random();

                var carIndex = random.Next(0, 2);
                var colorIndex = random.Next(0, 2);

                var routingKey = $"{cars[carIndex]}.{colors[colorIndex]}";

                var message = $"Hello word! {counter}. RoutingKey - {routingKey}. Exchange - {exchange}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: exchange,
                    routingKey: routingKey,
                    body: body);

                Console.WriteLine($"Message is sent {counter}. RoutingKey - {routingKey}. Exchange - {exchange}");

                counter++;
            }
        }
    };
}