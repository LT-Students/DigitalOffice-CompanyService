using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters
{
  public record GetCompanyFilter
  {
    [FromQuery(Name = "includeoffices")]
    public bool IncludeOffices { get; set; } = false;
  }
}
