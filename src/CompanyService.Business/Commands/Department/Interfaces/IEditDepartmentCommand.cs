using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces
{
    [AutoInject]
    public interface IEditDepartmentCommand
    {
        OperationResultResponse<bool> Execute(Guid departmentId, JsonPatchDocument<EditDepartmentRequest> request);
    }
}
