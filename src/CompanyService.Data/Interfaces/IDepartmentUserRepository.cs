using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IDepartmentUserRepository
    {
        public bool Add(DbDepartmentUser departmentUser);
        public DbDepartmentUser Get(Guid userId, bool includeDepartment);
        public void Remove(Guid userId);
    }
}
