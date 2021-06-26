using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbCompany
    {
        public const string TableName = "Companies";

        public Guid Id { get; set; }
        public Guid? LogoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tagline { get; set; }
        public string SiteUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DbDepartment> Departments { get; set; }
        public ICollection<DbPosition> Positions { get; set; }
        public ICollection<DbOffice> Offices { get; set; }

        public DbCompany()
        {
            Departments = new HashSet<DbDepartment>();
            Positions = new HashSet<DbPosition>();
            Offices = new HashSet<DbOffice>();
        }
    }

    public class DbCompanyConfiguration : IEntityTypeConfiguration<DbCompany>
    {
        public void Configure(EntityTypeBuilder<DbCompany> builder)
        {
            builder
                .ToTable(DbCompany.TableName);

            builder
                .Property(c => c.Name)
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
        }
    }
}
