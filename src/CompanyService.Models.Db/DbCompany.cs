using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbCompany
    {
        [JsonIgnore]
        public const string TableName = "Companies";

        public Guid Id { get; set; }
        public Guid? LogoId { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public bool IsDepartmentModuleEnabled { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public ICollection<DbDepartment> Departments { get; set; }
        [JsonIgnore]
        public ICollection<DbPosition> Positions { get; set; }
        [JsonIgnore]
        public ICollection<DbOffice> Offices { get; set; }
        [JsonIgnore]
        public ICollection<DbCompanyChanges> Changes { get; set; }

        public DbCompany()
        {
            Departments = new HashSet<DbDepartment>();
            Positions = new HashSet<DbPosition>();
            Offices = new HashSet<DbOffice>();
            Changes = new HashSet<DbCompanyChanges>();
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
                .HasMany(c => c.Offices)
                .WithOne(o => o.Company);

            builder
                .HasMany(c => c.Departments)
                .WithOne(d => d.Company);

            builder
                .HasMany(c => c.Positions)
                .WithOne(d => d.Company);

            builder
                .HasMany(c => c.Changes)
                .WithOne(ch => ch.Company);
        }
    }
}
