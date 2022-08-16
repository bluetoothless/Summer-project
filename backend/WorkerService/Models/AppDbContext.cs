using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkerService.Entities;

namespace WorkerService.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<BarberingService> BarberingServices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Visit> Visits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public void executeAction(string action, string content)
        {
            switch (action)
            {
                case "AddVisit":
                    var visitReceived = JsonConvert.DeserializeObject<Visit>(content);
                    if (visitReceived != null)
                    {
                        var barber = Barbers.First(x => x.name == visitReceived.barber.name);
                        var barberingService = BarberingServices.First(x => x.name == visitReceived.barberingService.name);
                        var clientExists = Clients.Any(x => x.name == visitReceived.client.name);

                        Client client;

                        if (clientExists)
                        {
                           client = Clients.First(x => x.name == visitReceived.client.name);
                        }
                        else {
                            client = new Client
                            {
                                name = visitReceived.client.name
                            };
                        }

                        var visit = new Visit
                        {
                            barber = barber,
                            barberingService = barberingService,
                            date = visitReceived.date,
                            hour = visitReceived.hour,
                            client = client
                        };
                        Thread.Sleep(500);
                        Visits.Add(visit);
                        SaveChanges();
                    }
                    break;
                case "AddClient":
                    break;
                default:
                    Console.WriteLine("No action executed.");
                    break;
            }
        }
        public void AddTestClient()
        {
            var clients = new List<Client>
            {
                new Client() {
                    name = "TestClient1",
                },
            };
            AddRange(clients);
            SaveChanges();
        }
        public void AddTestVisit()
        {
            var visits = new List<Visit>
            {
                new Visit() {
                    barber = Barbers.First(),
                    barberingService = BarberingServices.First(),
                    date = "12.09.2022",
                    hour = 11,
                    client = Clients.First(),
                },
            };
            AddRange(visits);
            SaveChanges();
        }
        public void FillDb()
        {
            var barbers = new List<Barber>
            {
                new Barber() {
                    name = "Anna Nowak",
                    startHour = 8,
                    endHour = 15
                },
                new Barber() {
                    name = "Jan Kowalski",
                    startHour = 7,
                    endHour = 14
                },
                new Barber() {
                    name = "Adam Nowicki",
                    startHour = 9,
                    endHour = 17
                },
            };
            var barberingServices = new List<BarberingService>
            {
                new BarberingService() {
                    name = "strzyżenie",
                    price = 40,
                    duration = 1
                },
                new BarberingService() {
                    name = "farbowanie",
                    price = 80,
                    duration = 3
                },
                new BarberingService() {
                    name = "stylizacja",
                    price = 50,
                    duration = 2
                }
            };
            AddRange(barbers);
            AddRange(barberingServices);
            SaveChanges();
        }
    }
}
