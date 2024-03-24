using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.EntityConfigurations.User
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Username);
            builder.HasIndex(u => u.Email);
            builder.HasOne(u => u.Profile).WithOne(p => p.User).HasForeignKey<ProfileEntity>(p => p.UserId);
            builder.Property(u => u.Username).HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
            builder.Property(u => u.Email).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.PasswordSalt).IsRequired();
        }
    }
}