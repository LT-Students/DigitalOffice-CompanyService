using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Models.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Data
{
  public class ContractSubjectDataMapper : IContractSubjectDataMapper
  {
    public ContractSubjectData Map(DbContractSubject dbContractSubject)
    {
      if (dbContractSubject is null)
      {
        return null;
      }

      return new ContractSubjectData(
        dbContractSubject.Id,
        dbContractSubject.Name);
    }
  }
}
