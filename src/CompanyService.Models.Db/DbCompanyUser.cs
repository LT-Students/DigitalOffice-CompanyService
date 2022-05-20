using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
  public record DbCompanyUser
  {
    public const string TableName = "CompaniesUsers";
    public const string HistoryTableName = "CompaniesUsersHistroy";

    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ContractSubjectId { get; set; }
    public int ContractTermType { get; set; }
    public double? Rate { get; set; }
    public DateTime StartWorkingAt { get; set; }
    public DateTime? EndWorkingAt { get; set; }
    public DateTime? Probation { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }

    public DbCompany Company { get; set; }
    public DbContractSubject ContractSubject { get; set; }
  }

  public class DbCompanyUserConfiguration : IEntityTypeConfiguration<DbCompanyUser>
  {
    public void Configure(EntityTypeBuilder<DbCompanyUser> builder)
    {
      builder
        .ToTable(
          DbCompanyUser.TableName,
          cu => cu.IsTemporal(b =>
          {
            b.UseHistoryTable(DbCompanyUser.HistoryTableName);
          }));

      builder
        .HasKey(t => t.Id);

      builder
        .HasOne(u => u.Company)
        .WithMany(c => c.Users);

      builder
        .HasOne(u => u.ContractSubject)
        .WithMany(cs => cs.Users);
    }
  }
}
