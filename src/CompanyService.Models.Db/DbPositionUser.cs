using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbPositionUser
    {
        public const string TableName = "PositionUsers";

        public Guid Id { get; set; }
        public Guid PositionId { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public DbPosition Position { get; set; }
    }

    public class DbPositionUserConfiguration : IEntityTypeConfiguration<DbPositionUser>
    {
        public void Configure(EntityTypeBuilder<DbPositionUser> builder)
        {
            builder
                .ToTable(DbPositionUser.TableName);

            builder
                .HasKey(p => p.Id);

            builder
                .HasOne(pu => pu.Position)
                .WithMany(p => p.Users)
                .HasForeignKey(pu => pu.PositionId);
        }
    }
}
