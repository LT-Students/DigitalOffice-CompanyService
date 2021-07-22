using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Interfaces
{
    /// <summary>
    /// Represents the repository pattern.
    /// Provides methods for working with the database of CompanyService.
    /// </summary>
    [AutoInject]
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Adds new department to the database. Returns its Id.
        /// </summary>
        /// <param name="department">Department to add.</param>
        /// <returns>New department Id.</returns>
        Guid CreateDepartment(DbDepartment department);

        /// <summary>
        /// Get <see cref="DbDepartment"/>.
        /// </summary>
        DbDepartment GetDepartment(Guid? departmentId, Guid? userId);

        DbDepartment GetDepartment(GetDepartmentFilter filter);

        /// <summary>
        /// Find departments in database.
        /// </summary>
        /// <returns>Found departments.</returns>
        List<DbDepartment> FindDepartments(int skipCount, int takeCount, bool includeDeactivated, out int totalCount);

        List<DbDepartment> FindDepartments(List<Guid> departmentsIds);

        /// <summary>
        /// Edits an existing department in the database. Returns whether it was successful to edit
        /// </summary>
        /// <param name="departmentId">Id of edited department.</param>
        /// <param name="request">Edit request.</param>
        /// <returns>Whether it was successful to edit.</returns>
        bool Edit(Guid departmentId, JsonPatchDocument<DbDepartment> request);

        List<DbDepartment> Search(string text);

        bool DoesNameExist(string name);

        bool Contains(Guid departmentId);
    }
}
