using System;

namespace LT.DigitalOffice.CompanyService.Models.Db
{
  public class DbCompanyChanges
  {
    public const string TableName = "CompanyChanges";

    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
    public string Changes { get; set; }

    public DbCompany Company { get; set; }
  }
}
