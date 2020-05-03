using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetUsersAsync(UserQueryParameters userQueryParameters);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserByIDAsync(int id);
        bool UserExist(string username);
        Task<IEnumerable<User>> GetUsersAsync();
        void CreateUser(User user);
        Task<string> VerifyAsync();
    }
}