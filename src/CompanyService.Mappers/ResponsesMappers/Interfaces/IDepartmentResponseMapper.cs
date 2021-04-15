using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces
{
    [AutoInject]
    public interface IDepartmentResponseMapper
    {
        DepartmentResponse Map(DbDepartment dbDepartment, User director, List<User> users);
    }
}
