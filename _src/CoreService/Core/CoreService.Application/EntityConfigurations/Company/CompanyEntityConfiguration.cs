using CoreService.Domain.Entities.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreService.Application.EntityConfigurations.Company
{
    public class CompanyEntityConfiguration : IEntityTypeConfiguration<CompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasMany(c => c.Profiles).WithOne(p => p.Company);
            builder.Property(c => c.Name).HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
            builder.Property(c => c.Description).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(c => c.Adress).HasColumnType("varchar(100)").HasMaxLength(100);
        }
    }
}