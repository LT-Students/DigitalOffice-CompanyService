using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210817223800_AddWorkDaysApiUrlColumn")]
    public class AddWorkDaysApiUrlColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "WorkDaysApiUrl",
            table: DbCompany.TableName,
            nullable: false);
        }
    }
}
