using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.PagingHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
       // public DataContext GetDataContext => this.dataContext as DataContext;

        public void CreateUser(User user)
        {
            Create(user);
        }
        public void UpdateUser(User user)
        {
            Update(user);
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = Get(filter: f => f.Username == username).SingleOrDefaultAsync();
            return await user;
        }
        public async Task<User> GetUserByIDAsync(int id, bool includePhoto = true)
        {
           if(includePhoto)
            {
                var user = await Get(filter: u => u.Id == id, includeProperties: p => p.PhotoSet).SingleOrDefaultAsync();
                return user;
            }
           else 
            {
                var user = await Get(filter: u => u.Id == id).SingleOrDefaultAsync();
                return user;
            }
            
        }

        public async Task<PagedList<User>> GetUsersAsync(UserQueryParameters userQueryParameters)
        {
            var columnMap = new Dictionary<string, Expression<Func<User, object>>>
            {
                ["username"] = u => u.Username,
                ["city"] = u => u.City,
                ["id"] = u => u.Id,
                ["country"] = u => u.Country,
                ["lastActive"] = u => u.LastActive,
            };

           var  query = Get(includeProperties: new Expression<Func<User, object>> [] { p => p.PhotoSet } , filter: x => x.Id != userQueryParameters.Id);
            
            query = query.ApplyOrdering(userQueryParameters, columnMap);

            var filterArray = new  List<Expression<Func<User, bool>>>();

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
              
                filterArray.Add(x => x.Gender == y);
              
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
                filterArray.Add(x => x.Gender == y);
            }

            if( userQueryParameters.minAge != 18 || userQueryParameters.minAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userQueryParameters.maxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userQueryParameters.minAge);

                filterArray.Add(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            }

            query = query.ApplyFiltering(userQueryParameters, filterArray.ToArray());
            return await PagedList<User>.CreateAsync(query, userQueryParameters.PageNumber, userQueryParameters.PageSize);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query =  Get(includeProperties: p => p.PhotoSet);
            return await query.ToListAsync();
        }

        public bool UserExist(string username)
        {
            var x = Get(filter: u => u.Username == username).Any();
            return x;
        }

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
