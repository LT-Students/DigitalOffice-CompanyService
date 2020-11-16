using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for adding a new department.
    /// </summary>
    public interface IAddDepartmentCommand
    {
        /// <summary>
        /// Adds a new department. Returns id of the added department.
        /// </summary>
        /// <param name="request">Department data.</param>
        /// <returns>Id of the added department.</returns>
        /// <exception cref="ValidationException">Thrown when department data is incorrect.</exception>
        Guid Execute(NewDepartmentRequest request);
    }
}
