using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    public class TokenRepository : RepositoryBase<Token>, ITokenRepository
    {
        private readonly DataContext ctx;

        public TokenRepository(DataContext ctx) : base(ctx)
        {
            this.ctx = ctx;
        }

       // public DataContext GetContext => this.dataContext as DataContext;
        public void AddToken(Token token)
        {
            Create(token);
        }

        public async Task DeleteTokenAsync(string refreshToken)
        {
            var token = await FindAll().SingleOrDefaultAsync(x => x.refreshToken == refreshToken);
            Delete(token);
        }

        public async Task<Token> GetTokenAsync(string refreshToken)
        {
            var token = await FindAll().SingleOrDefaultAsync(x => x.refreshToken == refreshToken);
            return token;
        }

        public void UpdateToken(Token token)
        {
            Update(token);
        }
    }
}
