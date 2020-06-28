using DatingApp.Core.DTOs;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<(IEnumerable<UserListDto>, PagingMetadata)> GetAllUsersAsync(UserQueryParameters userQueryParameters);
        Task<IEnumerable<UserListDto>> GetAllUsersAsync();
        Task<User> GetUserByIDAsync(int id, bool includePhoto = true);
        Task<UserDetailsDto> GetUserDetailsAsync(int id);
        Task GetUserByIDAsync();
        Task<(string errorMsg, bool transactionStatus)> UpdateUserAsync(UserForUpdateDto userForUpdateDto, User user);
        Task<Like> GetLike (int userId, int recipientId);
        Task<Like> LikeUserAsync(int userId, int recipientId);
    }
}
