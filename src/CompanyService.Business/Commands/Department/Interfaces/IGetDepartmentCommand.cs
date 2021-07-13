using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for getting department model by id.
    /// </summary>
    [AutoInject]
    public interface IGetDepartmentCommand
    {
        /// <summary>
        /// Returns the department model with the specified id.
        /// </summary>
        /// <param name="departmentId">Specified id of department.</param>
        /// <returns></returns>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when departmen is not found.</exception>
        OperationResultResponse<DepartmentResponse> Execute(GetDepartmentFilter filter);
    }
}
