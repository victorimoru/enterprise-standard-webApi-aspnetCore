using DatingApp.Core.DTOs;
using DatingApp.Core.Extensions;
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
        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }
        public Task<(IEnumerable<UserListDto>, PagingMetadata)> GetAllUsersAsync(UserQueryParameters userQueryParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
        {
            var userCollection = await repositoryWrapper.User.GetUsersAsync();
            var result = (from u in userCollection
                          select new UserListDto
                          {
                              Id = u.Id,
                              Username = u.Username,
                              Age = u.DateOfBirth.CalculateAge(),
                              KnownAs = u.KnownAs,
                              LastActive = u.LastActive,
                              City = u.City,
                              Country = u.Country,
                              Gender = u.Gender == Gender.Male ? "Male" : "Female",
                              photoUrl = u.PhotoSet.FirstOrDefault(x => x.IsMain).Url
                          }).OrderBy(a => a.Id).ToList();

            return result;
        }

        public Task<UserDetailsDto> GetUserDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
