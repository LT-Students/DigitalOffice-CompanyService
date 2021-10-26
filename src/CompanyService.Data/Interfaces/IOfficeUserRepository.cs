using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IOfficeUserRepository
  {
    Task<bool> CreateAsync(DbOfficeUser user);

    Task<List<DbOfficeUser>> GetAsync(List<Guid> userIds);

    Task<Guid?> RemoveAsync(Guid userId, Guid removedBy);
  }
}
