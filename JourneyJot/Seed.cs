using System;
using JourneyJot.Data;
using JourneyJot.Models;
using System.Diagnostics.Metrics;

namespace JourneyJot
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        Username = "johndoe",
                        Password = "Test123",
                        Email = "johndoe@email.com"
                    }
                };
           
                     
                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            }
        }
    }
}

