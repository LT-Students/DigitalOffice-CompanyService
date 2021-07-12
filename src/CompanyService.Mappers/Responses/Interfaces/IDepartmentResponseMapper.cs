using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces
{
    [AutoInject]
    public interface IDepartmentResponseMapper
    {
        DepartmentResponse Map(
            DbDepartment dbDepartment,
            List<UserData> userData,
            List<DbPositionUser> dbPositionUsers,
            List<ImageData> userImages,
            List<ProjectInfo> projectsInfo,
            GetDepartmentFilter filter);
    }
}
