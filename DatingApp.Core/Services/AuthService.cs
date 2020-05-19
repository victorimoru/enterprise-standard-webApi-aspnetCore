using DatingApp.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Repository;
using Shared.Utility.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IConfiguration config;

        public AuthService(IRepositoryWrapper repositoryWrapper, IConfiguration configuration)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.config = configuration;
        }
        public bool DoesUserExist(string username)
        {
            return this.repositoryWrapper.User.UserExist(username);
        }

        public async Task<string> LoginAsync(string username, string password)
        {

            var userInDB = await repositoryWrapper.User.GetUserAsync(username);
            if (userInDB == null)
                return null;
            if (!VerifyPasswordHash(password, userInDB.PasswordHash, userInDB.PasswordSalt))
                return null;
            return GenerateJSONWebToken(userInDB);

        }

        private string GenerateJSONWebToken(User user)
        {
            var claims = new[]
                        {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Gender", user.Gender == Gender.Male ? "Male" : "Female"),
                new Claim(ClaimTypes.Role, "SysAdmin"),
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {

            var passwordHashToVerify = Convert.ToBase64String(PasswordHasher.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password),
                                                            Convert.FromBase64String(passwordSalt)));
            if (string.Compare(passwordHashToVerify, passwordHash) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            byte[] salt = PasswordHasher.GenerateSalt();
            var passwordSalt = Convert.ToBase64String(salt);

            var hashPassword = Convert.ToBase64String(PasswordHasher.HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), salt));

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = hashPassword;
            repositoryWrapper.User.CreateUser(user);
            await repositoryWrapper.Complete();
            return user;
        }

    }
}
