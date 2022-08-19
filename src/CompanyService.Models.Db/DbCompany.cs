using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
  public class DbCompany
  {
    [JsonIgnore]
    public const string TableName = "Companies";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string Contacts { get; set; }
    public string LogoContent { get; set; }
    public string LogoExtension { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public bool IsActive { get; set; }

    [JsonIgnore]
    public ICollection<DbCompanyUser> Users { get; set; }

    public DbCompany()
    {
      Users = new HashSet<DbCompanyUser>();
    }
  }

  public class DbCompanyConfiguration : IEntityTypeConfiguration<DbCompany>
  {
    public void Configure(EntityTypeBuilder<DbCompany> builder)
    {
      builder
        .ToTable(DbCompany.TableName);

      builder
        .HasKey(t => t.Id);

      builder
        .Property(c => c.Name)
        .IsRequired();

      builder
        .HasMany(c => c.Users)
        .WithOne(cu => cu.Company);
    }
  }
}
