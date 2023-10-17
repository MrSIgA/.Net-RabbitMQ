using System.Text;
using RabbitMQ.Client;

var headers = new Dictionary<string, object>
{
    { "car", "Tesla" },
    { "color", "black" }
};
Task.Run(PublishMessages(headers));
Task.Run(PublishMessages(headers));

Console.ReadLine();

static Func<Task> PublishMessages(Dictionary<string, object> headers)
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

                const string exchange = "headers_exchange";

                channel.ExchangeDeclare(
                    exchange: exchange,
                    type: ExchangeType.Headers);

                var message = $"Hello word! {counter}. Exchange - {exchange}";
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;

                channel.BasicPublish(
                    exchange: exchange,
                    routingKey: string.Empty,
                    basicProperties: properties,
                    body: body);

                Console.WriteLine($"Message is sent {counter}. Exchange - {exchange}");

                counter++;
            }
        }
    };
}