using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210715235900_AddUniqueConstraintToPositionName")]
    public class AddUniqueConstraintToPositionName : Migration
    {
        protected override void Up(MigrationBuilder builder)
        {
            builder.AlterColumn<string>(
                name: nameof(DbPosition.Name),
                table: DbPosition.TableName,
                maxLength: 100);

            builder.AddUniqueConstraint(
                name: $"UX_Name_unique",
                table: DbPosition.TableName,
                column: nameof(DbPosition.Name));
        }
    }
}
