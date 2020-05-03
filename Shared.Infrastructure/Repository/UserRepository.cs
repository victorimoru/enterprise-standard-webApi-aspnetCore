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

        public void CreateUser(User user)
        {
            GetDataContext.Add(user);
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await FindByCondition(u => u.Username == username).Include(p => p.PhotoSet).SingleAsync();
            return user;
        }
        public async Task<User> GetUserByIDAsync(int id)
        {
            var user = await FindByCondition(u => u.Id == id).Include(p=> p.PhotoSet).SingleAsync();
            return user;
        }

        public async Task<PagedList<User>> GetUsersAsync(UserQueryParameters userQueryParameters)
        {
            var query = FindAll().Include(p => p.PhotoSet);
            if ((string.IsNullOrEmpty(userQueryParameters.OrderBy) && userQueryParameters.OrderBy.ToLowerInvariant().Equals("username")))
            {
                query.OrderBy(x => x.Username);
            }

            return await PagedList<User>.CreateAsync(query, userQueryParameters.PageNumber, userQueryParameters.PageSize);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query = FindAll().Include(p => p.PhotoSet);
            return await query.ToListAsync(); throw new System.NotImplementedException();
        }

        public bool UserExist(string username) => FindAll().Any(u => u.Username == username);

        /// <summary>
        /// Check if User table in the database is empty. 
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<string> VerifyAsync()
        {
            var  isUserTableEmpty = await FindAll().AnyAsync();
            if (isUserTableEmpty == false)
            {
                return "00";
            }
            else
            {
                return "22";
            }
        }
    }
}
