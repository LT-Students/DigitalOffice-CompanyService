using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyDataMapper
  {
    CompanyData Map(DbCompany dbCompany);
  }
}
