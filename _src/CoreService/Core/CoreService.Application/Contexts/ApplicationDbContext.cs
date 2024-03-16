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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();
        public DbSet<RoleEntity> Roles => Set<RoleEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(userOption =>
            {
                userOption.HasKey(u => u.Id);
                userOption.HasIndex(u => u.Username).IsUnique();
                userOption.HasIndex(u => u.Email).IsUnique();
                userOption.HasOne(u => u.Profile).WithOne(p => p.User).HasForeignKey<ProfileEntity>(p => p.UserId);
            });
            modelBuilder.Entity<ProfileEntity>(profileOption =>
            {
                profileOption.HasKey(p => p.Id);
            });
            modelBuilder.Entity<RoleEntity>(roleOption =>
            {
                roleOption.HasMany(r => r.Users).WithOne(u => u.Role);
                roleOption.HasIndex(r => r.Name).IsUnique();
                roleOption.HasIndex(r => r.ShortCode).IsUnique();
                roleOption.HasIndex(r => r.Level).IsUnique();
            });
            modelBuilder.Entity<CompanyEntity>(companyOption =>
            {
                companyOption.HasMany(c => c.Profiles).WithOne(p => p.Company);
            });
        }
    }
}
