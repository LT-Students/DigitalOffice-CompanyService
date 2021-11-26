using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser
{
  public record EditCompanyUserRequest
  {
    public double Rate { get; set; }
    public DateTime StartWorkingAt { get; set; }
  }
}
