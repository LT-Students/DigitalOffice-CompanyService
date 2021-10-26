using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
  public record CompanyInfo
  {
    public Guid Id { get; set; }
    public string PortalName { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string SiteUrl { get; set; }
    public bool IsDepartmentModuleEnabled { get; set; }
    public string LogoContent { get; set; }
    public string LogoExtension { get; set; }
    public SmtpInfo SmtpInfo { get; set; }
    //TODO
    //public List<DepartmentInfo> Departments { get; set; }
    //public List<PositionInfo> Positions { get; set; }
    public List<OfficeInfo> Offices { get; set; }
  }
}
