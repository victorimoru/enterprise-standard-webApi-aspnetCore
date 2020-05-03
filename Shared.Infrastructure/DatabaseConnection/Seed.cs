using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Repository;
using Shared.Utility.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.DatabaseConnection
{
    public  class Seed
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IConfiguration configuration;

        public Seed(IRepositoryWrapper repositoryWrapper, IConfiguration configuration)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.configuration = configuration;
        }

        public async Task SeedDataAsync()
        {
            if (await repositoryWrapper.User.VerifyAsync() == "00")
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
                    repositoryWrapper.User.CreateUser(user);
                   await repositoryWrapper.Complete();
                }
            }


        }
    }
}
