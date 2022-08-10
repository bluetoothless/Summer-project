using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerService.Entities;
using WorkerService.Services;

namespace WorkerService
{
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