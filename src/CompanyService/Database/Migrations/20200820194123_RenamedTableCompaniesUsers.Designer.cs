﻿// <auto-generated />
using System;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.CompanyService.Database.Migrations
{
    [DbContext(typeof(CompanyServiceDbContext))]
    [Migration("20200820194123_RenamedTableCompaniesUsers")]
    partial class RenamedTableCompaniesUsers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbCompany", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbCompanyUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "CompanyId", "PositionId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PositionId");

                    b.ToTable("CompaniesUsers");
                });

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbDepartment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DirectorUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbDepartmentUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("DbDepartmentUser");
                });

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbPosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbCompanyUser", b =>
                {
                    b.HasOne("LT.DigitalOffice.CompanyService.Database.Entities.DbCompany", "Company")
                        .WithMany("UserIds")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LT.DigitalOffice.CompanyService.Database.Entities.DbPosition", "Position")
                        .WithMany("UserIds")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LT.DigitalOffice.CompanyService.Database.Entities.DbDepartmentUser", b =>
                {
                    b.HasOne("LT.DigitalOffice.CompanyService.Database.Entities.DbDepartment", "Department")
                        .WithMany("UserIds")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
