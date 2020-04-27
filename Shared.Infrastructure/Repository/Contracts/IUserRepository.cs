using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    public interface IUserRepository
    {
        PagedList<User> GetUsersAsync(UserQueryParameters userQueryParameters);
        Task<User> GetUserAsync(string username);
        bool UserExist(string username);
        Task<IEnumerable<User>> GetUsersAsync();
        void CreateUser(User user);
       
    }
}