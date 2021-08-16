using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbPosition
    {
        public const string TableName = "Positions";

        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }

        public DbCompany Company { get; set; }
        public ICollection<DbPositionUser> Users { get; set; }

        public DbPosition()
        {
            Users = new HashSet<DbPositionUser>();
        }
    }

    public class DbPositionConfiguration : IEntityTypeConfiguration<DbPosition>
    {
        public void Configure(EntityTypeBuilder<DbPosition> builder)
        {
            builder
                .ToTable(DbPosition.TableName);

            builder
                .HasKey(p => p.Id);

            builder
                .HasMany(p => p.Users)
                .WithOne(u => u.Position);

            builder
                .HasOne(p => p.Company)
                .WithMany(c => c.Positions)
                .HasForeignKey(p => p.CompanyId);
        }
    }
}