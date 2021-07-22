using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbOffice
    {
        public const string TableName = "Offices";

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public DbCompany Company { get; set; }
        public ICollection<DbOfficeUser> Users { get; set; }

        public DbOffice()
        {
            Users = new HashSet<DbOfficeUser>();
        }
    }

    public class DbOfficeConfiguration : IEntityTypeConfiguration<DbOffice>
    {
        public void Configure(EntityTypeBuilder<DbOffice> builder)
        {
            builder
                .ToTable(DbOffice.TableName);

            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.City)
                .IsRequired();

            builder
                .Property(o => o.Address)
                .IsRequired();

            builder
                .HasOne(o => o.Company)
                .WithMany(c => c.Offices)
                .HasForeignKey(o => o.CompanyId);

            builder
                .HasMany(o => o.Users)
                .WithOne(u => u.Office);
        }
    }
}
