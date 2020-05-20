using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository.Contracts
{
    public interface ITokenRepository
    {
         Task<Token> GetTokenAsync(string refreshToken);
        void AddToken(Token token);
        void UpdateToken(Token token);
        Task DeleteTokenAsync(string refreshToken);
    }
}
