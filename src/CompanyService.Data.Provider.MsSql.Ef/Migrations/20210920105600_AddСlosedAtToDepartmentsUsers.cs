using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(CompanyServiceDbContext))]
  [Migration("20210920105600_AddСlosedAtToDepartmentsUsers")]
  public class AddСlosedAtToDepartmentsUsers : Migration
  {
    protected override void Up(MigrationBuilder builder)
    {
      builder.AddColumn<DateTime>(
      name: "LeftAtUts",
      table: "DepartmentsUsers",
      nullable: true);
    }

    protected override void Down(MigrationBuilder builder)
    {
      builder.DropColumn(
      name: "LeftAtUts",
      table: "DepartmentsUsers");
    }
  }
}
