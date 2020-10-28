using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
    public class DbDepartmentUser
    {
        public const string TableName = "DepartmentsUsers";

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PositionId { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public DbDepartment Department { get; set; }
        public DbPosition Position { get; set; }
    }

    public class DbDepartmentUserConfiguration : IEntityTypeConfiguration<DbDepartmentUser>
    {
        public void Configure(EntityTypeBuilder<DbDepartmentUser> builder)
        {
            builder
                .ToTable(DbDepartmentUser.TableName);

            builder
                .HasKey(u => u.Id);

            builder
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId);

            builder
                .HasOne(u => u.Position)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PositionId);
        }
    }
}