﻿using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for adding a new department.
    /// </summary>
    [AutoInject]
    public interface ICreateDepartmentCommand
    {
        /// <summary>
        /// Adds a new department. Returns id of the added department.
        /// </summary>
        /// <param name="request">Department data.</param>
        /// <returns>Id of the added department.</returns>
        /// <exception cref="ValidationException">Thrown when department data is incorrect.</exception>
        OperationResultResponse<Guid> Execute(CreateDepartmentRequest request);
    }
}
