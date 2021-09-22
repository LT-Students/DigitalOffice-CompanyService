using System;
using LT.DigitalOffice.CompanyService.Models.Db;
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
      name: nameof(DbDepartmentUser.LeftAtUts),
      table: DbDepartmentUser.TableName,
      nullable: true);
    }

    protected override void Down(MigrationBuilder builder)
    {
      builder.DropColumn(
      name: nameof(DbDepartmentUser.LeftAtUts),
      table: DbDepartmentUser.TableName);
    }
  }
}
