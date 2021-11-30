using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyUserRepository
  {
    Task<Guid?> CreateAsync(DbCompanyUser dbCompanyUser);

    Task<bool> EditAsync(Guid userId, JsonPatchDocument<DbCompanyUser> request);

    Task<DbCompanyUser> GetAsync(Guid userId);

    Task RemoveAsync(Guid userId, Guid removedBy);
  }
}
