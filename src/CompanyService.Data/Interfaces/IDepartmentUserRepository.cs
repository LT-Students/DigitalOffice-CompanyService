using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    [AutoInject]
    public interface IDepartmentUserRepository
    {
        public bool Add(DbDepartmentUser departmentUser);
    }
}
