using CoreService.Application.EntityConfigurations.Company;
using CoreService.Application.EntityConfigurations.Profile;
using CoreService.Application.EntityConfigurations.Role;
using CoreService.Application.EntityConfigurations.User;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Company;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.Role;
using CoreService.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CompanyEntity> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void OnBeforeSaving()
        {
            var updatedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            foreach (var currentEntry in updatedEntries)
            {
                if (currentEntry is ISoftDelete softDeleteEntity)
                {
                    softDeleteEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
            var deletedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            foreach (var currentEntry in deletedEntries)
            {
                if (currentEntry is ISoftDelete softDeleteEntity)
                {
                    currentEntry.State = EntityState.Modified;
                    softDeleteEntity.DeletedAt = DateTime.UtcNow;
                    softDeleteEntity.IsDeleted = true;
                    softDeleteEntity.IsActive = false;
                }
            }
        }
    }
}