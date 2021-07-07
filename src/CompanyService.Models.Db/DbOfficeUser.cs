using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbOfficeUser
    {
        public const string TableName = "OfficeUsers";

        public Guid Id { get; set; }
        public Guid OfficeId { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public DbOffice Office { get; set; }
    }

    public class DbOfficeUserConfiguration : IEntityTypeConfiguration<DbOfficeUser>
    {
        public void Configure(EntityTypeBuilder<DbOfficeUser> builder)
        {
            builder
                .ToTable(DbOfficeUser.TableName);

            builder
                .HasKey(p => p.Id);

            builder
                .HasOne(pu => pu.Office)
                .WithMany(p => p.Users)
                .HasForeignKey(pu => pu.OfficeId);
        }
    }
}
