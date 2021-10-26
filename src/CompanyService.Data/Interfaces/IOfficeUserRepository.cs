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
    bool Add(DbOfficeUser user);

    List<DbOfficeUser> Get(List<Guid> userIds);

    Task<Guid?> RemoveAsync(Guid userId, Guid removedBy);
  }
}
