using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entities.Identities;
using Infrastructure.Data.DbContext.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareLoanApp.Domain.Entities;

namespace Infrastructure.Data.DbContext
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid,
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // customize identity tables to utilize custom names
            UserConfiguration.ApplyUserIdentityConfigurations(builder);
            
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

        /// <summary>
        /// This overrides the base SaveChanges Async to perform basic Auditing business logic
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Auditable details entity pre-processing
            Audit();

            return await base.SaveChangesAsync(cancellationToken);
        }
        
        private void Audit()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is IAuditableEntity
                                                             && (x.State == EntityState.Modified || x.State == EntityState.Added));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added )
                {
                    ((IAuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((IAuditableEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }            
        }
        public override DbSet<User> Users { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Tokens> Tokens { get; set; }
    }
}
