using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
  public record CreateCompanyRequest
  {
    public string PortalName { get; set; }
    public SmtpInfo SmtpInfo { get; set; }
    public string CompanyName { get; set; }
    public string SiteUrl { get; set; }
    public AdminInfo AdminInfo { get; set; }
    public bool IsDepartmentModuleEnabled { get; set; }
    public string WorkDaysApiUrl { get; set; }
    public string LogoContent { get; set; }
    public string LogoExtension { get; set; }
  }
}
