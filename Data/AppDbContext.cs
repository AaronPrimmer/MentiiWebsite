using Microsoft.EntityFrameworkCore;
using MentiiWebsite.Models;

namespace MentiiWebsite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> MentiiUsersTbl { get; set; }

        public DbSet<Post> MentiiPostTbl { get; set; }

        public DbSet<Skill> MentiiSkillsTbl { get; set; }

        public DbSet<Comment> MentiiCommentsTbl { get; set; }
    }
}
