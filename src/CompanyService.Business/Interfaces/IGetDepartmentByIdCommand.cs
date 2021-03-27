using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for getting department model by id.
    /// </summary>
    public interface IGetDepartmentByIdCommand
    {
        /// <summary>
        /// Returns the department model with the specified id.
        /// </summary>
        /// <param name="id">Specified id of department.</param>
        /// <returns></returns>
        /// /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when departmen is not found.</exception>
        Department Execute(Guid id);
    }
}
