using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20210811141300_AddTimeColumns")]
    public class AddTimeColumns : Migration
    {
        private void UpdateCompanyTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbCompany.TableName);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: DbCompany.TableName,
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbCompany.TableName,
                nullable: true);
        }

        private void UpdateCompanyChangesTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                newName: "ModifiedAtUtc",
                table: DbCompanyChanges.TableName);
        }

        private void UpdateDepartmentTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: DbDepartment.TableName,
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbDepartment.TableName,
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: DbDepartment.TableName,
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbDepartment.TableName,
                nullable: true);
        }

        private void UpdateDepartmentUserTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                newName: "CreatedAtUtc",
                table: DbDepartmentUser.TableName);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbDepartmentUser.TableName,
                nullable: false);

            migrationBuilder.RenameColumn(
                name: "EndTime",
                newName: "ModifiedAtUtc",
                table: DbDepartmentUser.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbDepartmentUser.TableName,
                nullable: true);
        }

        private void UpdateOfficeTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbOffice.TableName);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbOffice.TableName,
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: DbOffice.TableName,
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbOffice.TableName,
                nullable: true);
        }

        private void UpdateOfficeUserTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: DbOfficeUser.TableName);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: DbOfficeUser.TableName,
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbOfficeUser.TableName,
                nullable: true);
        }

        private void UpdatePositionTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: DbPosition.TableName,
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbPosition.TableName,
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: DbPosition.TableName,
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbPosition.TableName,
                nullable: true);
        }

        private void UpdatePositionUserTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                newName: "CreatedAtUtc",
                table: DbPositionUser.TableName);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: DbPositionUser.TableName,
                nullable: false);

            migrationBuilder.RenameColumn(
                name: "EndTime",
                newName: "ModifiedAtUtc",
                table: DbPositionUser.TableName);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: DbPositionUser.TableName,
                nullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateCompanyTable(migrationBuilder);
            UpdateCompanyChangesTable(migrationBuilder);
            UpdateDepartmentTable(migrationBuilder);
            UpdateDepartmentUserTable(migrationBuilder);
            UpdateOfficeTable(migrationBuilder);
            UpdateOfficeUserTable(migrationBuilder);
            UpdatePositionTable(migrationBuilder);
            UpdatePositionUserTable(migrationBuilder);
        }
    }
}
