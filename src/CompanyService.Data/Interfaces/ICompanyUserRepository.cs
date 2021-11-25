using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyUserRepository
  {
    Task<Guid?> CreateAsync(DbCompanyUser dbCompanyUser);

    Task<bool> EditAsync(EditCompanyUserRequest request);

    Task<bool> DoesExistAsync(Guid userId);

    Task<List<DbCompanyUser>> GetAsync(IGetCompaniesRequest request);

    Task<DbCompanyUser> GetAsync(Guid userId);

    Task RemoveAsync(Guid userId, Guid removedBy);
  }
}
