using BlogDemoApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogDemoApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
       : base(options)
        {
        }

        public DbSet<PostEntity> Post { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<CommentEntity> Comment { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            base.OnModelCreating(modelBuilder);
            // Post - User (bir user'ın birçok post'u olabilir)
           modelBuilder.Entity<PostEntity>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post - Category (bir category birçok post'a sahip olabilir)
            modelBuilder.Entity<PostEntity>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment - User
            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment - Post
            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CategoryEntity>()
                .HasKey(c => c.CategoryId);
        }

    }
}
