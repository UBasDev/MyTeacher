using CoreService.Domain.Entities.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.EntityConfigurations.Profile
{
    public class ProfileEntityConfiguration : IEntityTypeConfiguration<ProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Age).HasColumnType("smallint").IsRequired();
            builder.Property(p => p.Firstname).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            builder.Property(p => p.Lastname).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            builder.Property(p => p.BirthDate).IsRequired();
        }
    }
}