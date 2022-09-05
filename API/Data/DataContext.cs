using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser>? Users { get; set; }
        public DbSet<UserLike>? Likes { get; set; }
        public DbSet<Message>? Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId }); // У лайков primary key составной

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser) // У UserLike есть один SourceUser
                .WithMany(l => l.LikedUsers) // И много LikedUsers
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении SourceUser удаляются и его лайки (При использовании SQL Server тут нужен .NoAction)

            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser) // У UserLike есть один SourceUser
                .WithMany(l => l.LikedByUsers) // И много LikedUsers
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении User удаляются и его лайки

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);                
        }
    }
}