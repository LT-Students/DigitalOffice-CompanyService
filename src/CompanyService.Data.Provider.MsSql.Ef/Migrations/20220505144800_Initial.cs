using System;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(CompanyServiceDbContext))]
  [Migration("20220505144800_Initial")]
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
          Name = table.Column<string>(maxLength: 150, nullable: false),
          Description = table.Column<string>(nullable: true),
          Tagline = table.Column<string>(nullable: true),
          Contacts = table.Column<string>(nullable: true),
          LogoContent = table.Column<string>(nullable: true),
          LogoExtension = table.Column<string>(nullable: true),
          IsActive = table.Column<bool>(nullable: false),
          CreatedBy = table.Column<Guid>(nullable: false),
          CreatedAtUtc = table.Column<DateTime>(nullable: false),
          ModifiedBy = table.Column<Guid>(nullable: true),
          ModifiedAtUtc = table.Column<DateTime>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PR_Companies", x => x.Id);
          table.UniqueConstraint("UC_Companies_Name", x => x.Name);
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
          ModifiedBy = table.Column<Guid>(nullable: true),
          IsActive = table.Column<bool>(nullable: false),
          PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
          PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
        },
        constraints: table =>
        {
          table.PrimaryKey("PR_CompaniesUsers", x => x.Id);
        })
        .Annotation("SqlServer:IsTemporal", true)
        .Annotation("SqlServer:TemporalHistoryTableName", "CompanyUsersHistory")
        .Annotation("SqlServer:TemporalHistoryTableSchema", null)
        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
    }

    private void CreateContractSubjectsTable(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: DbContractSubject.TableName,
        columns: table => new
        {
          Id = table.Column<Guid>(nullable: false),
          Name = table.Column<string>(maxLength: 150, nullable: false),
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
          table.UniqueConstraint("UC_PR_ContractSubjects_Name", x => x.Name);
        });
    }
    #endregion

    protected override void Up(MigrationBuilder migrationBuilder)
    {
      CreateCompaniesTable(migrationBuilder);

      CreateCompaniesUsersTable(migrationBuilder);

      CreateContractSubjectsTable(migrationBuilder);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: DbCompany.TableName);
      migrationBuilder.DropTable(name: DbCompanyUser.TableName);
      migrationBuilder.DropTable(name: DbContractSubject.TableName);
    }
  }
}
