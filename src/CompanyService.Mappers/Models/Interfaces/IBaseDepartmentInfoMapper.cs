using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    /// <summary>
    /// Represents mapper. Provides methods for converting an object of <see cref="DbDepartment"/>
    /// type into an object of <see cref="BaseDepartmentInfo"/> type according to some rule.
    /// </summary>
    [AutoInject]
    public interface IBaseDepartmentInfoMapper
    {
        BaseDepartmentInfo Map(DbDepartment dbDepartment);
    }
}
