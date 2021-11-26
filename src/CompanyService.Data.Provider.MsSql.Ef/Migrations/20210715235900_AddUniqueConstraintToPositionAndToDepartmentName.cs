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
                name: "Name",
                table: "Positions",
                maxLength: 100);

            builder.AddUniqueConstraint(
                name: $"UX_PositionName_unique",
                table: "Positions",
                column: "Name");

            builder.AlterColumn<string>(
                name: "Name",
                table: "Departments",
                maxLength: 100);

            builder.AddUniqueConstraint(
                name: $"UX_DepartmentName_unique",
                table: "Departments",
                column: "Name");
        }
    }
}
