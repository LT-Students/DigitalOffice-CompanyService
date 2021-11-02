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
                table: "Departments",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Departments",
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: "Departments",
                nullable: true);
        }

        private void UpdateDepartmentUserTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                newName: "CreatedAtUtc",
                table: "DepartmentUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DepartmentsUsers",
                nullable: false);

            migrationBuilder.RenameColumn(
                name: "EndTime",
                newName: "ModifiedAtUtc",
                table: "DepartmentsUsers");

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: "DepartmentsUsers",
                nullable: true);
        }

        private void UpdateOfficeTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: "Offices");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Offices",
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: "Offices",
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: "Offices",
                nullable: true);
        }

        private void UpdateOfficeUserTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                newName: "CreatedAtUtc",
                table: "OfficeUsers");

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: "OfficeUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: "OfficeUsers",
                nullable: true);
        }

        private void UpdatePositionTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Positions",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Positions",
                nullable: false);

            migrationBuilder.AddColumn<DateTime?>(
                name: "ModifiedAtUtc",
                table: "Positions",
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: "Positions",
                nullable: true);
        }

        private void UpdatePositionUserTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                newName: "CreatedAtUtc",
                table: "PositionUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PositionUsers",
                nullable: false);

            migrationBuilder.RenameColumn(
                name: "EndTime",
                newName: "ModifiedAtUtc",
                table: "PositionUsers");

            migrationBuilder.AddColumn<Guid?>(
                name: "ModifiedBy",
                table: "PositionUsers",
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
