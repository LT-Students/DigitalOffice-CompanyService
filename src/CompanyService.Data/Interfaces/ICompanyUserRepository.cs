using System;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Publishing;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyUserRepository
  {
    Task CreateAsync(DbCompanyUser dbCompanyUser);

    Task<bool> EditAsync(Guid userId, JsonPatchDocument<DbCompanyUser> request);

    Task<DbCompanyUser> GetAsync(Guid userId);

    Task<Guid?> RemoveAsync(Guid userId, Guid removedBy);

    Task<Guid?> ActivateAsync(IActivateUserPublish request);

    Task<bool> DoesExistAsync(Guid userId);

    Task<bool> RemoveContractSubjectAsync(Guid contractSubjectId);
  }
}
