using System;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(CompanyServiceDbContext))]
  [Migration("20211125172500_AddCompaniesUsersTable")]
  public class AddCompaniesUsersTable : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbCompanyUser.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          UserId = table.Column<Guid>(nullable: false),
          CompanyId = table.Column<Guid>(nullable: false),
          Rate = table.Column<double>(nullable: false),
          StartWorkingAt = table.Column<DateTime>(nullable: false),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_CompaniesUsers", u => u.Id);
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(DbCompanyUser.TableName);
    }
  }
}
