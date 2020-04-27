using DatingApp.Core.DTOs;
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

        Task<UserDetailsDto> GetUserDetailsAsync(int id);
    }
}
