using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

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
    Guid Create(DbDepartment department);

    /// <summary>
    /// Get <see cref="DbDepartment"/>.
    /// </summary>
    DbDepartment Get(Guid? departmentId, Guid? userId);

    List<DbDepartment> Get(List<Guid> departmentsIds, bool includeUsers = false);

    DbDepartment Get(GetDepartmentFilter filter);

    List<Guid> AreDepartmentsExist(List<Guid> departmentIds);

    /// <summary>
    /// Find departments in database.
    /// </summary>
    /// <returns>Found departments.</returns>
    List<DbDepartment> Find(int skipCount, int takeCount, bool includeDeactivated, out int totalCount);

    bool Edit(DbDepartment department, JsonPatchDocument<DbDepartment> request);

    List<DbDepartment> Search(string text);

    bool DoesNameExist(string name);

    bool Contains(Guid departmentId);
  }
}
