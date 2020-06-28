using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Repository.Contracts;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    public class LikeRepository : RepositoryBase<Like>, ILikeRepository
    {
        private readonly DataContext context;

        public LikeRepository(DataContext context):base(context)
        {
            this.context = context;
        }

        public void CreateLike(Like like)
        {
            Create(like);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
           
            var like = await FindAll().FirstOrDefaultAsync(x => x.LikerId == userId && x.LikeeId == recipientId);
            return like;
        }
    }
}
