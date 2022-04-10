using System;
using LT.DigitalOffice.Models.Broker.Enums;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser
{
  public class CreateCompanyUserRequest
  {
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? ContractSubjectId { get; set; }
    public ContractTerm ContractTermType { get; set; }
    public double? Rate { get; set; }
    public DateTime StartWorkingAt { get; set; }
    public DateTime? EndWorkingAt { get; set; }
    public DateTime? Probation { get; set; }
  }
}
