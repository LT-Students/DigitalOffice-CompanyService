using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210601145730_AddDbPositionUser")]
    public class AddDbPositionUser : Migration
    {
        private void CreatePositionUsersTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PositionUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PositionId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime?>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionUsers", x => x.Id);
                });
        }

        private void UpdateDepartmentUsersTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentUser_Positions",
                table: "DepartmentsUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "DepartmentsUsers");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreatePositionUsersTable(migrationBuilder);
            UpdateDepartmentUsersTable(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PositionUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "PositionId",
                table: "DepartmentsUsers",
                nullable: false);
        }
    }
}
