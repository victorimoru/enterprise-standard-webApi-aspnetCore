using Shared.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository.Contracts
{
    public interface ILikeRepository
    {
        Task<Like> GetLike(int userId, int recipientId);
        void CreateLike(Like like);
    }
}
