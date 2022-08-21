using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace API
{
    public class RabbitMqConnection
    {
        public ConnectionFactory factory;
        public IModel? channel;
        public IConnection? connection;
        public IHubContext<ConfirmationMessageHub> _hubContext;
        public RabbitMqConnection(IHubContext<ConfirmationMessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void Connect()
        {
            Console.WriteLine("Web server");

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
                channel.BasicPublish("", "fromWebQueue", null, body1);
            }
        }

        public void ReceiveMessage()
        {
            IConnection? connection;
            IModel? channel;
            var message = "";
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Thread.Sleep(100);
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                var mes = JsonConvert.DeserializeObject<Messaging>(message);
                if (mes != null)
                {
                    var action = mes.Name;
                    var content = mes.Message;
                    if (content == "Success")
                    {
                        _hubContext.Clients.All.SendAsync(content);
                        Console.WriteLine("Action successfully executed!");
                    }
                    channel.Close();
                    connection.Close();
                }
            };
            channel.BasicConsume(queue: "fromBackendQueue",
                                autoAck: true,
                                consumer: consumer);
        }
    }
}
