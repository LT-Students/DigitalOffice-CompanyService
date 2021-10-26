using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(CompanyServiceDbContext))]
  [Migration("20211026161700_DropOldTables")]
  public class DropOldTables : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        name: "FK_DepartmentUser_Departments",
        table: "DepartmentsUsers");

      migrationBuilder.DropTable("Positions");
      migrationBuilder.DropTable("Departments");
      migrationBuilder.DropTable("DepartmentsUsers");
      migrationBuilder.DropTable("PositionUsers");
    }
  }
}
