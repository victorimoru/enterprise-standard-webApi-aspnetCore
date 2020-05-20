using DatingApp.Core.DTOs;
using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.ServiceContracts
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(User user, string password);
        Task<(string token, string refreshToken)> LoginAsync(string username, string password);
        bool DoesUserExist(string username);
        Task<RefreshTokenServiceResponse> RefreshTokenAsync(string token, string refreshToken);
    }
}
