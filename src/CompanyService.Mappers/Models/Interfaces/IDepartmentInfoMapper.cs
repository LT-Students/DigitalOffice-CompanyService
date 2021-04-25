using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IDepartmentInfoMapper
    {
        DepartmentInfo Map(DbDepartment dbDepartment, UserInfo director, List<UserInfo> users);
    }
}
