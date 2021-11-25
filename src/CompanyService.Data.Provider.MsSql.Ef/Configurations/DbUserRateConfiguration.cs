using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Configurations
{
  public class DbUserRateConfiguration : IEntityTypeConfiguration<DbCompanyUser>
  {
    public void Configure(EntityTypeBuilder<DbCompanyUser> builder)
    {
      builder
        .ToTable(DbCompanyUser.TableName);

      builder
        .HasKey(u => u.Id);

      builder
        .HasOne(cu => cu.Company)
        .WithMany(c => c.Users);
    }
  }
}
