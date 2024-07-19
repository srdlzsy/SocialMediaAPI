using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using entity.Models;

namespace Repository.EfCore
{
    public class MyContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<AppUser> users { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Like> likes { get; set; }

    }
}
