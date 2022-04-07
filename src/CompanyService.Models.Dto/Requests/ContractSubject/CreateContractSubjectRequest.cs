using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject
{
  public record CreateContractSubjectRequest
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
