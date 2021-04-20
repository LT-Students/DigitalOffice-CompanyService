using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces
{
    [AutoInject]
    public interface IDepartmentResponseMapper
    {
        DepartmentInfo Map(DbDepartment dbDepartment, User director, List<User> users);
    }
}
