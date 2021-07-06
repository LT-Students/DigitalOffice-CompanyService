using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210705102500_AddSmtpToCompanyAndHistoryTable")]
    public class AddSmtpToCompanyAndHistoryTable : Migration
    {
        private void UpdateCompanyTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: DbCompany.TableName,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: DbCompany.TableName,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableSsl",
                table: DbCompany.TableName,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: DbCompany.TableName,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: DbCompany.TableName,
                nullable: true);
        }

        private void AddCompanyChangesTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbCompanyChanges.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    ModifiedBy = table.Column<Guid?>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Changes = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyChanges", x => x.Id);
                });
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateCompanyTable(migrationBuilder);
            AddCompanyChangesTable(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Host",
                table: DbCompany.TableName);

            migrationBuilder.DropColumn(
                name: "Port",
                table: DbCompany.TableName);

            migrationBuilder.DropColumn(
                name: "EnableSsl",
                table: DbCompany.TableName);

            migrationBuilder.DropColumn(
                name: "Email",
                table: DbCompany.TableName);

            migrationBuilder.DropColumn(
                name: "Password",
                table: DbCompany.TableName);

            migrationBuilder.DropTable(DbCompanyChanges.TableName);
        }
    }
}
