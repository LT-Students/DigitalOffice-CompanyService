using LT.DigitalOffice.Kernel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters
{
  public record FindContractSubjectFilter : BaseFindFilter
  {
    [FromQuery(Name = "IsActive")]
    public bool? IsActive { get; set; }
  }
}
