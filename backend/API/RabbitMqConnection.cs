using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace API
{
    class MyConsumer : DefaultBasicConsumer
    {
        public MyConsumer(IModel model) : base(model) { }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());

            if (properties.Headers == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                string autor = Encoding.UTF8.GetString((byte[])properties.Headers["autor"]);
                int index = (int)properties.Headers["index"];
                Console.WriteLine($"autor: {autor}\nindeks: {index} \nwiadomosc: {message}\n");
            }
            System.Threading.Thread.Sleep(2000);
            Model.BasicAck(deliveryTag, false);
        }
    }
    public class RabbitMqConnection
    {
        public ConnectionFactory factory;
        public IModel? channel;
        public IConnection? connection;
        public RabbitMqConnection()
        {

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
                //var message = receiveMessage();
                /*MyConsumer consumer = new MyConsumer(channel);
                channel.BasicQos(0, 1, false);
                channel.BasicConsume(qName, false, consumer);
                Console.ReadKey();*/


                /*var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                };
                channel.BasicConsume("kolejka1", true, consumer);*/
                //Console.ReadLine();

                /*      WIADOMOSC DO KOLEJKI*/
                /*var message1 = new { Name = "Barbers", Message = $"{barbers}" };
                var body1 = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message1));
                channel.BasicPublish("", "kolejka1", null, body1);
                Console.ReadLine();*/
                //sendMessage("Barbers", barbers, channel);
                //Console.ReadLine();
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

        public Task<string> ReceiveMessage()
        {
            using (connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                /*var data = channel.BasicGet("fromBackendQueue", true);
                if (data == null)
                {
                    return Task.FromResult("");
                }
                var message = Encoding.UTF8.GetString(data.Body.ToArray());
                Console.WriteLine(message);*/

                var consumer = new EventingBasicConsumer(channel);
                var message = "";
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(message);
                };
                channel.BasicConsume(queue: "fromBackendQueue",
                                     autoAck: true,
                                     consumer: consumer);
                return Task.FromResult(message);
            }
        }
    }
}
