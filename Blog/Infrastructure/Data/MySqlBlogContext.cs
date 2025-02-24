using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Models;

namespace Blog.Infrastructure.Data
{
    public class MySqlBlogContext : IdentityDbContext<User>
    {
        public MySqlBlogContext() { }
        public MySqlBlogContext(DbContextOptions<MySqlBlogContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = config.GetSection("MySQlConnection:MySQLConnectionString").Value;


                optionsBuilder.UseMySql(
                       connectionString,
                        new MySqlServerVersion(new Version(8, 4, 3)),
                       options => options.EnableRetryOnFailure(2)
                   );
            }
        }
    }
}
