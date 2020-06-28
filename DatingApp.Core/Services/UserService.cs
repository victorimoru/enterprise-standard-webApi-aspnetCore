using DatingApp.Core.DTOs;
using DatingApp.Core.Extensions;
using DatingApp.Core.Mapper;
using DatingApp.Core.ServiceContracts;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.LoggingHandler;
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

        public async Task<Like> GetLike(int userId, int recipientId) => await repositoryWrapper.Like.GetLike(userId, recipientId);

        public Task<User> GetUserByIDAsync(int id, bool includePhoto = true)
        { 
           var userInDB = repositoryWrapper.User.GetUserByIDAsync(id, includePhoto);
            return userInDB;
        }

        public Task GetUserByIDAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(int id)
        {
            var user = await repositoryWrapper.User.GetUserByIDAsync(id);
            
            return (user == null) ? null : customMapper.MapToUserDetailsDto(user);

        }

        public async Task<Like> LikeUserAsync(int userId, int recipientId)
        {
            
            var like = new Like
            {
                LikerId = userId,
                LikeeId = recipientId
            };
            repositoryWrapper.Like.CreateLike(like);
            var saveStatus = await repositoryWrapper.Complete();
            switch (saveStatus.transactionStatus)
            {
                case false:
                    return null;
                default:
                    return like;
            }
        }

        public async Task<(string errorMsg, bool transactionStatus)> UpdateUserAsync(UserForUpdateDto userForUpdateDto, User user)
        {
            repositoryWrapper.User.UpdateUser(customMapper.MapUserForUpdateDtoToUser(userForUpdateDto, user));
            var result = await repositoryWrapper.Complete();
            return result;
        }
    }
}
