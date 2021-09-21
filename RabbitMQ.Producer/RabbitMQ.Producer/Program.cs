using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Producer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                "demo-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            for (int i = 0; i < 5000; i++)
            {
                var message = new { Name = $"Producer {i+1}", Message = "Hello!" };

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", "demo-queue", null, body);
            }
        }
    }
}
