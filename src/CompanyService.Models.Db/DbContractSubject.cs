using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
  public class DbContractSubject
  {
    public const string TableName = "ContractSubjects";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbCompany Company { get; set; }

    [JsonIgnore]
    public ICollection<DbCompanyUser> Users { get; set; }

    public DbContractSubject()
    {
      Users = new HashSet<DbCompanyUser>();
    }
  }

  public class DbContractSubjectConfiguration : IEntityTypeConfiguration<DbContractSubject>
  {
    public void Configure(EntityTypeBuilder<DbContractSubject> builder)
    {
      builder
        .ToTable(
          DbContractSubject.TableName);

      builder
        .HasKey(cs => cs.Id);

      builder
        .Property(cs => cs.Name)
        .IsRequired();

      builder
        .HasMany(ct => ct.Users)
        .WithOne(cu => cu.ContractSubject);

      builder
        .HasOne(cs => cs.Company)
        .WithMany(c => c.ContractSubjects);
    }
  }
}
