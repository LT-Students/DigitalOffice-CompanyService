using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
  [Migration("20211102154800_DropOfficesTable")]
  public class DropOfficesTable : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "Offices");
      migrationBuilder.DropTable(name: "OfficeUsers");
    }
  }
}
