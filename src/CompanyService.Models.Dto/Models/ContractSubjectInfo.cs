using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
  public record ContractSubjectInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}
