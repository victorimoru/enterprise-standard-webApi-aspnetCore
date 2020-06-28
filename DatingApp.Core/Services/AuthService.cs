using DatingApp.Core.DTOs;
using DatingApp.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Repository;
using Shared.Utility;
using Shared.Utility.Common;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public async Task<(string token, string refreshToken)> LoginAsync(string username, string password)
        {

            var userInDB = await repositoryWrapper.User.GetUserAsync(username);
            if (userInDB == null)
                return (null, null);
            if (!VerifyPasswordHash(password, userInDB.PasswordHash, userInDB.PasswordSalt))
                return (null, null);
            var accessToken = new Token()
            {
                AccessToken = GenerateJSONWebToken(userInDB),
                AccessTokenExpiry = DateTime.Now.AddMinutes(60),
                refreshToken = GuidGenerator.Generate(),
                refreshTokenExpiry = DateTime.Now.AddMinutes(65),
                UserId = userInDB.Id
            };
            repositoryWrapper.Token.AddToken(accessToken);
            var saveStatus = await repositoryWrapper.Complete();
            return (GenerateJSONWebToken(userInDB), GuidGenerator.Generate());

        }

        private string GenerateJSONWebToken(User user)
        {
            var claims = new[]
                        {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Gender, user.Gender == Gender.Male ? "Male" : "Female"),
                new Claim(ClaimTypes.Role, "SysAdmin"),
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(3),
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

        public async Task<RefreshTokenServiceResponse> RefreshTokenAsync(string token, string refreshToken)
        {
            // Get Principal from supplied Token
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null) return new RefreshTokenServiceResponse { ErrorMsg = "Invalid Token" };

            var userId = int.Parse(validatedToken.Claims.SingleOrDefault(x => x.Type == "Id").Value);
            // Get and validate the supplied refresh Token with the persisted refresh Token
            var TokenInDB = await repositoryWrapper.Token.GetTokenAsync(refreshToken);
             if(TokenInDB == null) return new RefreshTokenServiceResponse { ErrorMsg = "Invalid Refresh Token" };
            var guardResult = RefreshTokenGuard(TokenInDB, refreshToken);
            if (guardResult.status == false) return new RefreshTokenServiceResponse { ErrorMsg = guardResult.errorMessage };

            Token accessToken = await WriteToDB(userId, TokenInDB);
            var saveStatus = await repositoryWrapper.Complete();

            switch (saveStatus.transactionStatus)
            {
                case false:
                    return new RefreshTokenServiceResponse { ErrorMsg = saveStatus.errorMsg };
                default:
                    return new RefreshTokenServiceResponse
                    {
                        ErrorMsg = null,
                        Token = accessToken.AccessToken,
                        RefreshToken = accessToken.refreshToken
                    };
            }

          
        }

        private async Task<Token> WriteToDB(int userId, Token TokenInDB)
        {
            var userInDB = await repositoryWrapper.User.GetUserByIDAsync(userId, false);
            await repositoryWrapper.Token.DeleteTokenAsync(TokenInDB.refreshToken);
            var accessToken = new Token()
            {
                AccessToken = GenerateJSONWebToken(userInDB),
                AccessTokenExpiry = DateTime.Now.AddMinutes(7),
                refreshToken = GuidGenerator.Generate(),
                refreshTokenExpiry = DateTime.Now.AddMinutes(20),
                UserId = userInDB.Id
            };
            repositoryWrapper.Token.AddToken(accessToken);
            return accessToken;
        }
        private (string errorMessage, bool status) RefreshTokenGuard(Token refreshTokenInDB, string refreshToken)
        {
            if (refreshTokenInDB.refreshToken != refreshToken) return ("Invalid Refresh Token", false);
            if ( refreshTokenInDB.AccessTokenExpiry.CompareTo(DateTime.Now) == 1) return ("Your Access Token is still valid", false);
            if (refreshTokenInDB.refreshTokenExpiry.CompareTo(DateTime.Now) == -1) return ("Your Refresh Token has expired, Login again", false);
            return (null, true);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token,
                   new TokenValidationParameters()
                   {
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                       ValidateAudience = false,
                       ValidateIssuer = false,
                       ValidateLifetime = false,
                       ValidateIssuerSigningKey = true
                   }, out var validatedToken);

                var testToken = validatedToken;

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }


                return principal;
            }
            catch
            {

                return null;
            }
            
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            var firstCondition = validatedToken is JwtSecurityToken jwtSecurityToken;
              if (firstCondition)
            {
                var x = validatedToken as JwtSecurityToken;
                return x.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
            
            
        }

    }
}
