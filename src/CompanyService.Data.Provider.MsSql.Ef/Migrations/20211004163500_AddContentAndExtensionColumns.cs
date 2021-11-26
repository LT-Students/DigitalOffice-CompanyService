using System;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{

  [DbContext(typeof(CompanyServiceDbContext))]
  [Migration("20211004163500_AddContentAndExtensionColumns")]
  public class AddContentAndExtensionColumns : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        name: nameof(DbCompany.LogoContent),
        table: DbCompany.TableName,
        nullable: true);

      migrationBuilder.AddColumn<string>(
        name: nameof(DbCompany.LogoExtension),
        table: DbCompany.TableName,
        nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        name: nameof(DbCompany.LogoContent),
        table: DbCompany.TableName);

      migrationBuilder.DropColumn(
        name: nameof(DbCompany.LogoExtension),
        table: DbCompany.TableName);
    }
  }
}
