using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialApp.infrastructure.Data.Entities;

namespace SocialApp.infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Members { get; set; } = null!;

        public DbSet<Comment> Comments { get; set; } = null!;

        public DbSet<CommentLike> CommentsLikes { get; set; } = null!;

        public DbSet<Post> Posts { get; set; } = null!;

        public DbSet<PostLike> PostsLikes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Comment>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<PostLike>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.LikedPosts)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CommentLike>()
                .HasOne(c => c.Owner)
                .WithMany(u => u.LikedComments)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
