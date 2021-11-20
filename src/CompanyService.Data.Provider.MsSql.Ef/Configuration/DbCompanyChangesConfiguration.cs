using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Configuration
{
  public class DbCompanyChangesConfiguration : IEntityTypeConfiguration<DbCompanyChanges>
  {
    public void Configure(EntityTypeBuilder<DbCompanyChanges> builder)
    {
      builder
        .ToTable(DbCompanyChanges.TableName);

      builder
        .Property(x => x.Changes)
        .IsRequired();

      builder
        .HasOne(x => x.Company)
        .WithMany(c => c.Changes)
        .HasForeignKey(x => x.CompanyId);
    }
  }
}
