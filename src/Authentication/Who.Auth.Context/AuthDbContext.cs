using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Who.Auth.Entities;

namespace Who.Auth.Context
{
    public class AuthDbContext: DbContext
    {

        public DbSet<Client> WhoClients { get; set; }
        public DbSet<Authorization> WhoAuthorizations { get; set; }
        public DbSet<Scope> WhoScopes { get; set; }
        public DbSet<Token> WhoTokens { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }
        //public DbSet<MExternalClaim> ExternalClaims { get; set; }

        //public DbSet<MUserLogin> UserLogins { get; set; }

        //public DbSet<MUserSecret> UserSecrets { get; set; }

        public DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().ToTable("WhoClients");
            modelBuilder.Entity<Authorization>().ToTable("WhoAuthorizations");
            modelBuilder.Entity<Scope>().ToTable("WhoScopes");
            modelBuilder.Entity<Token>().ToTable("WhoTokens");

            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Subject)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            //modelBuilder.Entity<WhoRole>().ToTable("WhoRoles");


            //modelBuilder
            //    .Entity<WhoAuthenticationProvider>()
            //    .Property(p => p.Parameters);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var updatedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .Where(e => e.Entity is IConcurrencyAware)
                //.OfType<IConcurrencyAware>()
                .ToList();

            foreach (var entry in updatedEntries)
            {
                var en = entry.Entity as IConcurrencyAware;
                en.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            return base.SaveChangesAsync(cancellationToken);    
        }
    }
}
