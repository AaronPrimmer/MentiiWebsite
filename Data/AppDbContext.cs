using MentiiWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MentiiWebsite.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> MentiiUsersTbl { get; set; }

        public DbSet<Post> MentiiPostTbl { get; set; }

        public DbSet<Skill> MentiiSkillsTbl { get; set; }

        public DbSet<Comment> MentiiCommentsTbl { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>(entity =>
            {
                entity.HasOne(p => p.Author)
                      .WithMany()
                      .HasForeignKey(p => p.UserUuid)
                      .HasPrincipalKey(u => u.UserUuid)  // Explicitly specify the principal key
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
