using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210720102800_UpdateDepartmentsAndDepartmentUsersTables")]
    public class UpdateDepartmentsAndDepartmentUsersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectorUserId",
                table: DbDepartment.TableName);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: DbDepartmentUser.TableName,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: DbDepartmentUser.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "DirectorUserId",
                table: DbDepartment.TableName,
                nullable: true);
        }
    }
}
