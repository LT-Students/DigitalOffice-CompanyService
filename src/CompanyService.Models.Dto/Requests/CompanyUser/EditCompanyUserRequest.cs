using System;
using LT.DigitalOffice.Models.Broker.Enums;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser
{
  public record EditCompanyUserRequest
  {
    public Guid? ContractSubjectId { get; set; }
    public ContractTerm ContractTermType { get; set; }
    public double? Rate { get; set; }
    public DateTime StartWorkingAt { get; set; }
    public DateTime? EndWorkingAt { get; set; }
    public DateTime? Probation { get; set; }
  }
}
