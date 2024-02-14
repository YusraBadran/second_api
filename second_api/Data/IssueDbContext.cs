using System;
using Microsoft.EntityFrameworkCore;

namespace second_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Issue> Issues { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Education> Education { get; set; }
        public DbSet<Models.Address> Address { get; set; }

        //  public DbSet<Models.UserImage> UserImage { get; set; }
    }
}
