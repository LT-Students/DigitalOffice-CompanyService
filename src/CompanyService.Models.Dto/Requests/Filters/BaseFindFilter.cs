using LT.DigitalOffice.Kernel.Validators.Models;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters
{
  public record BaseFindFilter : BaseFindRequest
  {
    [FromQuery(Name = "includeDeactivated")]
    public bool IncludeDeactivated { get; set; } = false;
  }
}
