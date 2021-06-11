using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IDepartmentUserRepository
    {
        bool Add(DbDepartmentUser departmentUser);

        DbDepartmentUser Get(Guid userId, bool includeDepartment);
        
        void Remove(Guid userId);
    }
}
