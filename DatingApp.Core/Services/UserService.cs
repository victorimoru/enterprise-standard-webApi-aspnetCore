using DatingApp.Core.DTOs;
using DatingApp.Core.Extensions;
using DatingApp.Core.Mapper;
using DatingApp.Core.ServiceContracts;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.PagingHelper;
using Shared.Infrastructure.Repository;
using System;
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
        public Task<(IEnumerable<UserListDto>, PagingMetadata)> GetAllUsersAsync(UserQueryParameters userQueryParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
        {
            var userCollection = await repositoryWrapper.User.GetUsersAsync();
            return customMapper.MapToUserListDto(userCollection);

        }

        public Task<UserDetailsDto> GetUserDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
