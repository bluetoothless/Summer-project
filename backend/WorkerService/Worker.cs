using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerService.Entities;
using WorkerService.Services;

namespace WorkerService
{
    /*class MyConsumer : DefaultBasicConsumer
    {
        ILogger<Worker> _logger;
        public MyConsumer(IModel model, ILogger<Worker> logger) : base(model) 
        { _logger = logger; }
        
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());

            if (properties.Headers != null)
            {
                string student = Encoding.UTF8.GetString((byte[])properties.Headers["student"]);
                int index = (int)properties.Headers["index"];
                _logger.LogInformation($"{student} {index} {message}");
            }
            else
            {
                _logger.LogInformation(message);
            }
            Model.BasicAck(deliveryTag, false);
        }
    }*/
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public ConnectionFactory factory;
        public IModel? channel;
        public IConnection? connection;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rabbitMqConnection = new RabbitMqConnection();
            rabbitMqConnection.Connect();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                DbHelper dbHelper = new DbHelper();

                // fetch user data
                List<Client> users = dbHelper.GetUsers();

                if (users.Count == 0)
                {
                    dbHelper.SeedUsers();
                }
                else
                {
                    DisplayUserInformation(users);
                }


                /*
                Console.WriteLine("Backend");

                string fromWebQName = "fromWebQueue";
                factory = new ConnectionFactory()
                {
                    UserName = "xzghooiz",
                    Password = "W_2R6sH6OLg5WU86J0upqHdStyDIUyyW",
                    HostName = "hawk.rmq.cloudamqp.com",
                    VirtualHost = "xzghooiz"
                };

                var message = "";
                using (connection = factory.CreateConnection())
                using (channel = connection.CreateModel())
                {
                    channel.QueueDeclare(fromWebQName, true, false, false, null);
                    var consumer = new MyConsumer(channel, _logger);
                    channel.BasicQos(0, 1, false);
                    channel.BasicConsume(fromWebQName, false, consumer);
                }*/



                var message = rabbitMqConnection.ReceiveMessage(_logger);
                if (message != "")
                {
                    _logger.LogInformation(message);
                    Console.WriteLine(message);
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
        private void DisplayUserInformation(List<Client> users)
        {
            users?.ForEach(user =>
            {
                _logger.LogInformation($"User Information\nUser: {user.name}");/*\t Email: {user.Email}*/
            });
        }

        
    }
}