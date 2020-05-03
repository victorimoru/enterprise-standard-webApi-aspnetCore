using DatingApp.Core.DTOs;
using DatingApp.Core.Extensions;
using DatingApp.Core.Mapper;
using DatingApp.Core.ServiceContracts;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using Shared.Infrastructure.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly ICustomMapper customMapper;

        public UserService(IRepositoryWrapper repositoryWrapper, ICustomMapper customMapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.customMapper = customMapper;
        }
        public async Task<(IEnumerable<UserListDto>, PagingMetadata)> GetAllUsersAsync(UserQueryParameters userQueryParameters)
        {
            var userCollection = await repositoryWrapper.User.GetUsersAsync(userQueryParameters);
            var result = customMapper.MapToUserListDto(userCollection);

            var metadata = new PagingMetadata
            {
                CurrentPage = userCollection.CurrentPage,
                TotalPages = userCollection.TotalPages,
                PageSize = userCollection.PageSize,
                TotalCount = userCollection.TotalCount,
                HasNext = userCollection.HasNext,
                HasPrevious = userCollection.HasPrevious
            };
            return (result, metadata);
        }

        public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
        {
            
            var userCollection = await repositoryWrapper.User.GetUsersAsync();
            return customMapper.MapToUserListDto(userCollection);

        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(int id)
        {
            var user = await repositoryWrapper.User.GetUserByIDAsync(id);
            return customMapper.MapToUserDetailsDto(user);

        }
    }
}
