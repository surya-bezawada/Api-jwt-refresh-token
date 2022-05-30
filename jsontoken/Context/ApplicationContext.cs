using jsontoken.Entities;
using jsontoken.Models;
using Microsoft.EntityFrameworkCore;

namespace jsontoken.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }

        public DbSet<Role> roles { get; set; }

        public DbSet<RoleManager> roleManagers { get; set; }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
