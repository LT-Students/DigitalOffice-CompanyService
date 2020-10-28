using System;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Data.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20200722235704_InitialCreate")]
    public class InitialCreate : Migration
    {
        private const string ColumnIdName = "Id";

        #region Create tables

        private void CreateTableDeparments(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbDepartment.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DirectorUserId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });
        }
        private void CreateTableDeparmentsUsers(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbDepartmentUser.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    PositionId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentUser_Departments",
                        column: x => x.DepartmentId,
                        principalTable: DbDepartment.TableName,
                        principalColumn: ColumnIdName,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentUser_Positions",
                        column: x => x.PositionId,
                        principalTable: DbPosition.TableName,
                        principalColumn: ColumnIdName,
                        onDelete: ReferentialAction.Cascade);
                });
        }

        private void CreateTablePositions(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: DbPosition.TableName,
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });
        }

        #endregion

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreateTableDeparments(migrationBuilder);

            CreateTablePositions(migrationBuilder);

            CreateTableDeparmentsUsers(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(DbDepartmentUser.TableName);

            migrationBuilder.DropTable(DbPosition.TableName);

            migrationBuilder.DropTable(DbDepartment.TableName);
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
