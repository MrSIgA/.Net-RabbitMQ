using System.Text;
using RabbitMQ.Client;

Task.Run(PublishMessages("first"));
Task.Run(PublishMessages("second"));
Task.Run(PublishMessages("third"));

Console.ReadLine();

static Func<Task> PublishMessages(string routingKey)
{
    return () =>
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var counter = 0;
            while (true)
            {
                var timeToSleep = new Random().Next(1000, 3000);
                Thread.Sleep(timeToSleep);

                const string exchange = "direct_exchange";

                channel.ExchangeDeclare(
                    exchange: exchange,
                    type: ExchangeType.Direct);

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