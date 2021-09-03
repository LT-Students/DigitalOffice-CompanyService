using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IDepartmentUserRepository
    {
        bool Add(DbDepartmentUser departmentUser);

        DbDepartmentUser Get(Guid userId, bool includeDepartment);

        IEnumerable<Guid> Find(IFindDepartmentUsersRequest request, out int totalCount);

        List<DbDepartmentUser> Find(List<Guid> userIds);

        void Remove(Guid userId, Guid removedBy);
    }
}
