using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces
{
  [AutoInject]
  public interface ICompanyResponseMapper
  {
    CompanyResponse Map(DbCompany company,
      List<OfficeData> offices,
      GetCompanyFilter filter);
  }
}
