using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210715235900_AddUniqueConstraintToPositionAndToDepartmentName")]
    public class AddUniqueConstraintToPositionAndToDepartmentName : Migration
    {
        protected override void Up(MigrationBuilder builder)
        {
            builder.AlterColumn<string>(
                name: nameof(DbPosition.Name),
                table: DbPosition.TableName,
                maxLength: 100);

            builder.AddUniqueConstraint(
                name: $"UX_PositionName_unique",
                table: DbPosition.TableName,
                column: nameof(DbPosition.Name));

            builder.AlterColumn<string>(
                name: nameof(DbDepartment.Name),
                table: DbDepartment.TableName,
                maxLength: 100);

            builder.AddUniqueConstraint(
                name: $"UX_DepartmentName_unique",
                table: DbDepartment.TableName,
                column: nameof(DbDepartment.Name));
        }
    }
}
