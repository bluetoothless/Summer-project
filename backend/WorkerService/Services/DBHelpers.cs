using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkerService.Entities;
using WorkerService.Models;

namespace WorkerService.Services
{
    public class DbHelper
    {
        private AppDbContext _dbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(AppSettings.ConnectionString);

            return optionsBuilder.Options;
        }

        public List<Client> GetUsers()
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                try
                {
                    var users = _dbContext.Clients.ToList();

                    if (users == null)
                        throw new InvalidOperationException("No user data is found!");

                    return users;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        // Seed Data - When no data is in the db, we want to populate with data
        public void SeedUsers()
        {
            using (_dbContext = new AppDbContext(GetAllOptions()))
            {
                _dbContext.Clients.AddRange(ListOfUsers());

                _dbContext.SaveChanges();
            }
        }

        private List<Client> ListOfUsers()
        {
            List<Client> users = new List<Client> {
                new Client
                {
                    name = "Natalia Cyrklaff"
                }

            };

            return users;
        }
    }
}