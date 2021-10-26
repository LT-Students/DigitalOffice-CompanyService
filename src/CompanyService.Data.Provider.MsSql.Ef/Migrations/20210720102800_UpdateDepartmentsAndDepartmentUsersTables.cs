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
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "DepartmentUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "DepartmentUsers");

            migrationBuilder.AddColumn<Guid?>(
                name: "DirectorUserId",
                table: "Departments",
                nullable: true);
        }
    }
}
