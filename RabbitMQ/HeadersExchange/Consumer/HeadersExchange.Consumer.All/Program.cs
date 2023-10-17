using Consumer;

var headers = new Dictionary<string, object>
{
    { "x-match", "all" },
    { "car", "Tesla" },
    { "color", "black" }
};
ConsumerBuilder.ReceiveMessages(headers);