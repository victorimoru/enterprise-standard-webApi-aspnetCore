using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    class UserRepository: RepositoryBase<User>, IUserRepository
    {
        private readonly DataContext ctx;

        public UserRepository(DataContext ctx):base(ctx)
        {
            this.ctx = ctx;
        }
        public DataContext GetDataContext => this.dataContext as DataContext;
        public async Task<User> GetUserAsync(string username)
        {
            var user = await FindByCondition(u => u.Username == username).SingleAsync();
            return user;
        }

        public PagedList<User> GetUsersAsync(UserQueryParameters userQueryParameters)
        {
            var query = FindAll().Include(p => p.PhotoSet);
            if ((string.IsNullOrEmpty(userQueryParameters.OrderBy) && userQueryParameters.OrderBy.ToLowerInvariant().Equals("username")))
            {
                query.OrderBy(x => x.Username);
            }

            return PagedList<User>.Create(query, userQueryParameters.PageNumber, userQueryParameters.PageSize);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query = FindAll().Include(p => p.PhotoSet);
            return await query.ToListAsync(); throw new System.NotImplementedException();
        }

        public bool UserExist(string username) => FindAll().Any(u => u.Username == username);
    }
}
