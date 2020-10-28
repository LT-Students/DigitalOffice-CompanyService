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
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<DbDepartmentUser> Users { get; set; }

        public DbPosition()
        {
            Users = new HashSet<DbDepartmentUser>();
        }
    }

    public class DbPositionUserConfiguration : IEntityTypeConfiguration<DbPosition>
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
        }
    }
}