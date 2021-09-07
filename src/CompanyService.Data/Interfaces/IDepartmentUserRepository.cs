using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
  [AutoInject]
  public interface IDepartmentUserRepository
  {
    bool Add(DbDepartmentUser departmentUser);

    DbDepartmentUser Get(Guid userId, bool includeDepartment);

    List<Guid> Get(IGetDepartmentUsersRequest request, out int totalCount);

    List<DbDepartmentUser> Get(List<Guid> userIds);

    void Remove(Guid userId, Guid removedBy);
  }
}
