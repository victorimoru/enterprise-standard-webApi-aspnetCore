using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using System;
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
        public void UpdateUser(User user)
        {
            GetDataContext.Update(user);
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await FindByCondition(u => u.Username == username).Include(p => p.PhotoSet).SingleOrDefaultAsync();
            return user;
        }
        public async Task<User> GetUserByIDAsync(int id, bool includePhoto = true)
        {
           if(includePhoto)
            {
                var user = await FindByCondition(u => u.Id == id).Include(p => p.PhotoSet).SingleOrDefaultAsync();
                return user;
            }
           else 
            {
                var user = await FindByCondition(u => u.Id == id).SingleOrDefaultAsync();
                return user;
            }
            
        }

        public async Task<PagedList<User>> GetUsersAsync(UserQueryParameters userQueryParameters)
        {
            var query = FindAll().Include(p => p.PhotoSet).AsQueryable();
            if ((string.IsNullOrEmpty(userQueryParameters.OrderBy) && userQueryParameters.OrderBy.ToLowerInvariant().Equals("username")))
            {
                query.OrderBy(x => x.Username);
            }
           

            query = query.Where(x => x.Id != userQueryParameters.Id);

            if (string.IsNullOrEmpty(userQueryParameters.gender))
            {
                Gender y;
                if (userQueryParameters.loggedInUserGender.ToLowerInvariant() == "female")
                {
                    y = Gender.Male;
                }
                else
                {
                    y = Gender.Female;
                }
                query = query.Where(x => x.Gender == y);
            } 
            else
            {
                Gender y;
                if (userQueryParameters.gender.ToLowerInvariant() == "female")
                {
                    y = Gender.Female;
                }
                else
                {
                    y = Gender.Male;
                }
                query = query.Where(x => x.Gender == y);
            }

            if( userQueryParameters.minAge != 18 || userQueryParameters.minAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userQueryParameters.maxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userQueryParameters.minAge);

                query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
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
