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
    public string PortalName { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string SiteUrl { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LogoContent { get; set; }
    public string LogoExtension { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public bool IsDepartmentModuleEnabled { get; set; }
    public bool IsActive { get; set; }
    public string WorkDaysApiUrl { get; set; }

    [JsonIgnore]
    public ICollection<DbCompanyChanges> Changes { get; set; }

    [JsonIgnore]
    public ICollection<DbCompanyUser> Users { get; set; }

    public DbCompany()
    {
      Changes = new HashSet<DbCompanyChanges>();

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

      builder
        .HasMany(c => c.Users)
        .WithOne(cu => cu.Company);
    }
  }
}
