using API;
using API.Models;
using API.Services;
using API.Services.Abstract;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          //policy.WithOrigins("http://localhost:4200");
                          policy.AllowAnyOrigin();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddScoped<IBarberRepository, BarberRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IVisitService, VisitService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

var rabbitMqConnection = new RabbitMqConnection();
rabbitMqConnection.Connect();

IConnection? connection;
IModel? channel;
var message = "";
using (connection = rabbitMqConnection.factory.CreateConnection())
using (channel = connection.CreateModel())
{
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message);
    };
    channel.BasicConsume(queue: "fromBackendQueue",
                         autoAck: true,
                         consumer: consumer);
}
var mes = JsonConvert.DeserializeObject<Messaging>(message);
if (mes != null)
{
    var action = mes.Name;
    var content = mes.Message;
    if (content == "Success")
    {
        Console.WriteLine("Action successfully executed!");
    }
}

app.Run();
