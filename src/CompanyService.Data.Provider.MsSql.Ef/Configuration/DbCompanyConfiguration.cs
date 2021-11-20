using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Configuration
{
  public class DbCompanyConfiguration : IEntityTypeConfiguration<DbCompany>
  {
    public void Configure(EntityTypeBuilder<DbCompany> builder)
    {
      builder
        .ToTable(DbCompany.TableName);

      builder
        .Property(c => c.PortalName)
        .IsRequired();

      builder
        .Property(c => c.CompanyName)
        .IsRequired();

      builder
        .Property(c => c.Host)
        .IsRequired();

      builder
        .Property(c => c.Email)
        .IsRequired();

      builder
        .Property(c => c.Password)
        .IsRequired();

      builder
        .Property(c => c.SiteUrl)
        .IsRequired();

      builder
        .Property(c => c.WorkDaysApiUrl)
        .IsRequired();

      builder
        .HasMany(c => c.Changes)
        .WithOne(ch => ch.Company);
    }
  }
}
