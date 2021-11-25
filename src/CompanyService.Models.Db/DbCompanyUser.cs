using System;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
  public record DbCompanyUser
  {
    public const string TableName = "CompaniesUsers";

    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public double? Rate { get; set; }
    public DateTime? StartWorkingAt { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public DbCompany Company { get; set; }
  }
}
