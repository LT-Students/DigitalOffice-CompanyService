using System;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(CompanyServiceDbContext))]
  [Migration("2022032820150000_Initial")]
  public class Initial : Migration
  {
    #region CreateTables
    private void CreateCompaniesTable(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbCompany.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          Description = table.Column<string>(nullable: true),
          Tagline = table.Column<string>(nullable: true),
          Contacts = table.Column<string>(nullable: true),
          LogoContent = table.Column<string>(nullable: true),
          LogoExtension = table.Column<string>(nullable: true),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PR_Companies", x => x.Id);
        });
    }

    private void CreateCompaniesUsersTable(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbCompanyUser.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          CompanyId = table.Column<Guid>(nullable: false),
          UserId = table.Column<Guid>(nullable: false),
          ContractSubjectId = table.Column<Guid>(nullable: true),
          ContractTermType = table.Column<int>(nullable: false),
          Rate = table.Column<decimal>(nullable: true),
          StartWorkingAt = table.Column<DateTime>(nullable: false),
          EndWorkingAt = table.Column<DateTime>(nullable: true),
          Probation = table.Column<DateTime>(nullable: true),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
          IsActive = table.Column<bool>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PR_CompaniesUsers", x => x.Id);
        });
    }

    private void CreateCompaniesChangesTable(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbCompanyChanges.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          CompanyId = table.Column<Guid>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
          Changes = table.Column<string>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PR_CompaniesChanges", x => x.Id);
        });
    }

    private void CreateContractSubjectsTable(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbContractSubject.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          CompanyId = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(nullable: false),
          Description = table.Column<string>(nullable: true),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true),
          IsActive = table.Column<bool>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PR_ContractSubjects", x => x.Id);
        });
    }
    #endregion

    protected override void Up(MigrationBuilder migrationBuilder)
    {
      CreateCompaniesTable(migrationBuilder);

      CreateCompaniesUsersTable(migrationBuilder);

      CreateCompaniesChangesTable(migrationBuilder);

      CreateContractSubjectsTable(migrationBuilder);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: DbCompany.TableName);
      migrationBuilder.DropTable(name: DbCompanyUser.TableName);
      migrationBuilder.DropTable(name: DbCompanyChanges.TableName);
      migrationBuilder.DropTable(name: DbContractSubject.TableName);
    }
  }
}
