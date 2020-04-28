using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Infrastructure.Entities;
using Shared.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Infrastructure.DatabaseConnection
{
    public  class Seed
    {
        
        private readonly DataContext dbContext;
        private readonly IConfiguration configuration;

        public Seed(DataContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public void SeedData()
        {
            if (!dbContext.Users.Any())
            {
                var userData = System.IO.File.ReadAllText(configuration["Seed:PATH"]);
                var userDataCollection = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in userDataCollection)
                {
                    byte[] salt = PasswordHasher.GenerateSalt();
                    var passwordSalt = Convert.ToBase64String(salt);

                    var hashPassword = Convert.ToBase64String(PasswordHasher.HashPasswordWithSalt(Encoding.UTF8.GetBytes("password"), salt));

                    user.Username = user.Username.ToLowerInvariant();
                    user.PasswordSalt = passwordSalt;
                    user.PasswordHash = hashPassword;
                    dbContext.Users.Add(user);
                    dbContext.SaveChangesAsync();
                }
            }


        }
    }
}
