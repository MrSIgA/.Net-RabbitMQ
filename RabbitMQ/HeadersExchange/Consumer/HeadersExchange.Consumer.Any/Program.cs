using Consumer;

var headers = new Dictionary<string, object>
{
    { "x-match", "any" },
    { "car", "Tesla" }
};
ConsumerBuilder.ReceiveMessages(headers);