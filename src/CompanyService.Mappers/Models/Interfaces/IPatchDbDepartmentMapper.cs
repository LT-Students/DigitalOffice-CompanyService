using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IPatchDbDepartmentMapper
    {
        JsonPatchDocument<DbDepartment> Map(JsonPatchDocument<EditDepartmentRequest> request);
    }
}
