using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IOfficeUserRepository
  {
    bool Add(DbOfficeUser user);

    List<DbOfficeUser> Get(List<Guid> userIds);

    void Remove(Guid userId, Guid removedBy);
  }
}
