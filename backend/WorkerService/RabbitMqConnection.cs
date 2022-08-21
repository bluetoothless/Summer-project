using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService
{
    internal class RabbitMqConnection
    {
        public ConnectionFactory factory;
        public IModel? channel;
        public IConnection? connection;
        public RabbitMqConnection()
        {

        }
        public void Connect()
        {
            Console.WriteLine("Backend");

            string fromWebQName = "fromWebQueue";
            string fromBackendQName = "fromBackendQueue";
            factory = new ConnectionFactory()
            {
                UserName = "xzghooiz",
                Password = "W_2R6sH6OLg5WU86J0upqHdStyDIUyyW",
                HostName = "hawk.rmq.cloudamqp.com",
                VirtualHost = "xzghooiz"
            };

            using (connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                channel.QueueDeclare(fromWebQName, true, false, false, null);
                channel.QueueDeclare(fromBackendQName, true, false, false, null);

            }
        }

        public void SendMessage(string messageName, string content)
        {
            using (connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                var message1 = new { Name = $"{messageName}", Message = $"{content}" };
                var body1 = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message1));
                channel.BasicPublish("", "fromBackendQueue", null, body1);
                Console.WriteLine("Sending confirmation message...");
            }
        }
        public Task<string> ReceiveMessage(ILogger<Worker> logger)
        {
            logger.LogInformation("Checking for messages...");
            using (connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                var data = channel.BasicGet("fromWebQueue", true);
                if (data == null)
                {
                    return Task.FromResult("");
                }
                var message = Encoding.UTF8.GetString(data.Body.ToArray());
                logger.LogInformation(message);
                return Task.FromResult(message);
            }
        }
    }
}
