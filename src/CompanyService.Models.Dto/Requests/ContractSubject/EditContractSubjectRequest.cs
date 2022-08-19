using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject
{
  public record EditContractSubjectRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
  }
}
