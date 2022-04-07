using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
  [AutoInject]
  public interface ICompanyInfoMapper
  {
    CompanyInfo Map(DbCompany company,
      List<OfficeData> offices,
      GetCompanyFilter filter);
  }
}
