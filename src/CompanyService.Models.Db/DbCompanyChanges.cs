using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
  public class DbCompanyChanges
  {
    public const string TableName = "CompanyChanges";

    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
    public string Changes { get; set; }

    public DbCompany Company { get; set; }
  }

  public class DbCompanyChangesConfiguration : IEntityTypeConfiguration<DbCompanyChanges>
  {
    public void Configure(EntityTypeBuilder<DbCompanyChanges> builder)
    {
      builder
        .ToTable(DbCompanyChanges.TableName);

      builder
        .HasKey(t => t.Id);

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
