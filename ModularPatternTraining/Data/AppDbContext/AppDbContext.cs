using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModularPatternTraining.Modules.Authentication.Models;
using ModularPatternTraining.Modules.BookModule.Mapping;
using ModularPatternTraining.Modules.BookModule.Models;
using ModularPatternTraining.Modules.LibraryModule.Models;
using ModularPatternTraining.Modules.UserManagement.Model;

namespace ModularPatternTraining.Data.AppDbContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>   
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TokenBlackListModel>  TokenBlackList { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(BookMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
