using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
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

        /// <summary>
        /// Find departments in database.
        /// </summary>
        /// <returns>Found departments.</returns>
        List<DbDepartment> FindDepartments();

        List<DbDepartment> Search(string text);
    }
}
