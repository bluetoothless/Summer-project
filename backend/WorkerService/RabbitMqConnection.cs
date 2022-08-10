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
                /*MyConsumer consumer = new MyConsumer(channel);
                channel.BasicQos(0, 1, false);
                channel.BasicConsume(qName, false, consumer);
                Console.ReadKey();*/


                //      WIADOMOSC DO KOLEJKI
                /*var message = new { Name = "Sklep", Message = "Hello!" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("", "fromWebQueue", null, body);*/
                //sendMessage("Message1", "Message sent from Worker");

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
            }
        }
        public string ReceiveMessage(ILogger<Worker> logger)
        {
            logger.LogInformation("Tu");
            using (connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                var message = "";
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                    logger.LogInformation(message);
                };
                channel.BasicConsume("fromWebQueue", true, consumer);
                return message;
            }
        }
    }
}
