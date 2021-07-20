using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IDepartmentInfoMapper
    {
        DepartmentInfo Map(DbDepartment dbDepartment, DepartmentUserInfo director);
    }
}
