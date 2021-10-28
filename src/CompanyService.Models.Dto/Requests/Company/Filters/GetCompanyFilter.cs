using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters
{
  public record GetCompanyFilter
  {
    [FromQuery(Name = "includedepartments")]
    public bool IncludeDepartments { get; set; } = false;

    [FromQuery(Name = "includepositions")]
    public bool IncludePositions { get; set; } = false;

    [FromQuery(Name = "includeoffices")]
    public bool IncludeOffices { get; set; } = false;

    [FromQuery(Name = "includesmtpcredentials")]
    public bool IncludeSmtpCredentials { get; set; } = false;
  }
}
