using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.DatabaseConnection
{
    public class LikeEntityConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(x => new { x.LikerId, x.LikeeId });

            builder.HasOne(f => f.Likee)
                .WithMany(f => f.Likers)
                .HasForeignKey(f => f.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Liker)
                .WithMany(f => f.Likees)
                .HasForeignKey(f => f.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
