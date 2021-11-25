using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser
{
  public class EditCompanyUserRequest
  {
    public Guid UserId { get; set; }
    public double Rate { get; set; }
    public DateTime StartWorkingAt { get; set; }
  }
}
