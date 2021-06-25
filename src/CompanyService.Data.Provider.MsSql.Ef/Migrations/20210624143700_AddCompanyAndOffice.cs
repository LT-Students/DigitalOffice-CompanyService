using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210624143700_AddCompanyAndOffice")]
    public class AddCompanyAndOffice : Migration
    {
        public void CreateCompanyTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbCompany.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LogoId = table.Column<Guid?>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Tagline = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });
        }

        public void CreateOfficeTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbOffice.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });
        }

        public void UpdateDepartmentTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: DbDepartment.TableName);
        }

        public void UpdatePositionTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: DbPosition.TableName);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreateCompanyTable(migrationBuilder);
            CreateOfficeTable(migrationBuilder);
            UpdateDepartmentTable(migrationBuilder);
            UpdatePositionTable(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: DbCompany.TableName);
            migrationBuilder.DropTable(name: DbOffice.TableName);
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: DbDepartment.TableName);
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: DbPosition.TableName);
        }
    }
}
