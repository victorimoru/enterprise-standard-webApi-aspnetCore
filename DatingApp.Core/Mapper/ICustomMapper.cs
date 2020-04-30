using DatingApp.Core.DTOs;
using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Mapper
{
    public interface ICustomMapper
    {
        IEnumerable<UserListDto> MapToUserListDto(IEnumerable<User> users);
    }
}
