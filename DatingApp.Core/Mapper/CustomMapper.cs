﻿using DatingApp.Core.DTOs;
using DatingApp.Core.Extensions;
using Shared.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using Shared.Utility.Security;

namespace DatingApp.Core.Mapper
{
    class CustomMapper : ICustomMapper
    {
        private readonly IAesCryptoEngine aesCryptoEngine;

        public CustomMapper(IAesCryptoEngine aesCryptoEngine)
        {
            this.aesCryptoEngine = aesCryptoEngine;
        }
        public UserDetailsDto MapToUserDetailsDto(User u)
        {
            var result = new UserDetailsDto
            {
                Id = u.Id,
                Username = u.Username,
                Age = u.DateOfBirth.CalculateAge(),
                Created = u.Created.ToShortDateString(),
                Introduction = u.Introduction,
                KnownAs = u.KnownAs,
                LastActive = u.LastActive,
                City = u.City,
                Country = u.Country,
                Gender = u.Gender == Gender.Male ? "Male" : "Female",
                Photos = (from p in u.PhotoSet
                          select new PhotoDetailDto
                          {
                              Id = p.Id,
                              DateAdded = p.DateAdded,
                              Description = p.Description,
                              Url = p.Url
                          }).ToList()
            };
            return result;
        }

        public IEnumerable<UserListDto> MapToUserListDto(IEnumerable<User> userCollection)
        {
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
                          }).ToList();

            return result;
        }

        public User MapUserForUpdateDtoToUser(UserForUpdateDto userForUpdateDto, User user)
        {
            user.Introduction = userForUpdateDto.Introduction;
            user.LookingFor = userForUpdateDto.LookingFor;
            user.City = userForUpdateDto.City;
            user.Country = userForUpdateDto.Country;
            // user.Interest = userForUpdateDto.Country;
            return user;
        }

       
    }
    
}
