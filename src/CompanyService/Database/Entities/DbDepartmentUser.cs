using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.CompanyService.Database.Entities
{
    public class DbDepartmentUser
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public DbDepartment Department { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class DepartmentUserConfiguration : IEntityTypeConfiguration<DbDepartmentUser>
    {
        public void Configure(EntityTypeBuilder<DbDepartmentUser> builder)
        {
            builder.HasKey(user => new {user.UserId, user.DepartmentId});

            builder.HasOne(user => user.Department)
                .WithMany(department => department.UserIds)
                .HasForeignKey(user => user.DepartmentId);
        }
    }
}