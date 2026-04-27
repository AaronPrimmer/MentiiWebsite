using MentiiWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MentiiWebsite.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<UserModel> MentiiUsersTbl { get; set; }

        public DbSet<Post> MentiiPostTbl { get; set; }

        public DbSet<Skill> MentiiSkillsTbl { get; set; }

        public DbSet<Comment> MentiiCommentsTbl { get; set; }

        public DbSet<PostLike> MentiiPostLikeTbl { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Post to UserModel relationship
            builder.Entity<Post>(entity =>
            {
                entity.HasOne(p => p.Author)
                      .WithMany()
                      .HasForeignKey(p => p.UserUuid)
                      .HasPrincipalKey(u => u.UserUuid)  // Explicitly specify the principal key
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PostLike>(entity =>
            {
                entity.ToTable("mentii_post_likes_tbl");

                // FK to Post
                entity.HasOne(l => l.Post)
                      .WithMany(p => p.Likes)
                      .HasForeignKey(l => l.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(l => l.User)
                      .WithMany()
                      .HasForeignKey(l => l.UserUuid)
                      .OnDelete(DeleteBehavior.Restrict);

                // Make sure the user can't like the same post twice
                entity.HasIndex(l => new { l.PostId, l.UserUuid }).IsUnique();
            });

            builder.Entity<UserModel>(entity =>
            {
                entity.HasMany(u => u.Skills)
                      .WithOne()
                      .HasForeignKey(s => s.UserUuid)
                      .HasPrincipalKey(u => u.UserUuid)  // Explicitly specify the principal key
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Skill>(entity =>
            {
                entity.ToTable("mentii_skills_tbl");

                entity.HasOne<UserModel>()
                      .WithMany(u => u.Skills)
                      .HasForeignKey(s => s.UserUuid)
                      .HasPrincipalKey(u => u.UserUuid)  // Explicitly specify the principal key
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
