using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkerService.Entities;
using WorkerService.Models;
using WorkerService.Services;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public ConnectionFactory factory;
        public IModel? channel;
        public IConnection? connection;
        public IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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
                /*else
                {
                    DisplayUserInformation(users);
                }*/

                var messageString = await rabbitMqConnection.ReceiveMessage(_logger);
                if (messageString != "")
                {
                    var message = JsonConvert.DeserializeObject<Messaging>(messageString);
                    if(message != null)
                    {
                        var action = message.Name;
                        var content = message.Message;
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            context.executeAction(action, content);
                        }
                    }
                    rabbitMqConnection.SendMessage("Result", "Success");
                }

                await Task.Delay(5000, stoppingToken);
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