using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbCompanyMapper : IDbCompanyMapper
  {
    public DbCompany Map(CreateCompanyRequest request)
    {
      if (request is null)
      {
        return null;
      }

      return new DbCompany
      {
        Id = Guid.NewGuid(),
        PortalName = request.PortalName,
        CompanyName = request.CompanyName,
        SiteUrl = request.SiteUrl,
        CreatedAtUtc = DateTime.UtcNow,
        Host = request.SmtpInfo.Host,
        Port = request.SmtpInfo.Port,
        EnableSsl = request.SmtpInfo.EnableSsl,
        Email = request.SmtpInfo.Email,
        Password = request.SmtpInfo.Password,
        IsDepartmentModuleEnabled = request.IsDepartmentModuleEnabled,
        IsActive = true,
        WorkDaysApiUrl = request.WorkDaysApiUrl,
        LogoContent = request.Logo?.Content,
        LogoExtension = request.Logo?.Extension
      };
    }
  }
}
