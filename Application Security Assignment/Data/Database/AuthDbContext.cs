using Application_Security_Assignment.Data.Enums;
using Application_Security_Assignment.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application_Security_Assignment.Data.Database
{

    namespace WebApp_Core_Identity.Model
    {
        public class AuthDbContext : IdentityDbContext<ApplicationUser>
        {
            //private readonly IConfiguration _configuration;
            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.Entity<ApplicationUser>().Property(e => e.Gender).HasConversion(v => v.ToString(), v => (Gender)Enum.Parse(typeof(Gender), v));
                base.OnModelCreating(builder);

     

            }

            public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
            {
               
            }
          /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
            }*/
        }
    }
}


