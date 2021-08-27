using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  /// <inheritdoc />
  public class DepartmentRepository : IDepartmentRepository
  {
    private readonly IDataProvider _provider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepartmentRepository(
        IDataProvider provider,
        IHttpContextAccessor httpContextAccessor)
    {
      _provider = provider;
      _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public Guid Create(DbDepartment department)
    {
      _provider.Departments.Add(department);
      _provider.Save();

      return department.Id;
    }

    /// <inheritdoc />
    public DbDepartment Get(Guid? departmentId, Guid? userId)
    {
      DbDepartment dbDepartment = null;

      if (!(departmentId.HasValue || userId.HasValue))
      {
        throw new BadRequestException("You must specify 'departmentId' or 'userId'.");
      }

      if (departmentId.HasValue)
      {
        dbDepartment = _provider.Departments
            .Include(d => d.Users.Where(u => u.IsActive))
            .FirstOrDefault(d => d.Id == departmentId.Value);
      }
      else
      {
        dbDepartment = _provider.DepartmentUsers
            .Include(du => du.Department)
            .FirstOrDefault(du => du.IsActive && du.UserId == userId)?.Department;
      }

      if (dbDepartment == null)
      {
        string valueMessage = departmentId.HasValue ? $"department id: {departmentId.Value}" : $"user id: {userId.Value}";

        throw new NotFoundException($"Department was not found by specific {valueMessage}");
      }

      return dbDepartment;
    }

    public DbDepartment Get(GetDepartmentFilter filter)
    {
      IQueryable<DbDepartment> dbDepartments = _provider.Departments.AsQueryable();

      dbDepartments = dbDepartments.Where(d => d.Id == filter.DepartmentId);

      if (filter.IsIncludeUsers)
      {
        dbDepartments = dbDepartments.Include(d => d.Users.Where(u => u.IsActive));
      }

      return dbDepartments.FirstOrDefault();
    }

    public List<Guid> AreDepartmentsExist(List<Guid> departmentIds)
    {
      return _provider.Departments
          .Where(d => departmentIds.Contains(d.Id) && d.IsActive)
          .Select(d => d.Id)
          .ToList();
    }

    /// <inheritdoc />
    public List<DbDepartment> Find(int skipCount, int takeCount, bool includeDeactivated, out int totalCount)
    {
      if (skipCount < 0)
      {
        throw new ArgumentException("Skip count can't be less than 0.");
      }

      if (takeCount <= 0)
      {
        throw new ArgumentException("Take count can't be equal or less than 0.");
      }

      IQueryable<DbDepartment> dbDepartments = _provider.Departments.AsQueryable();

      if (includeDeactivated)
      {
        totalCount = _provider.Departments.Count();
      }
      else
      {
        totalCount = _provider.Departments.Count(d => d.IsActive);
        dbDepartments = dbDepartments.Where(d => d.IsActive);
      }

      return dbDepartments
          .Skip(skipCount)
          .Take(takeCount)
          .Include(d => d.Users.Where(u => u.IsActive))
          .ToList();
    }

    public List<DbDepartment> Find(List<Guid> departmentIds)
    {
      return _provider.Departments.Where(d => departmentIds.Contains(d.Id)).ToList();
    }

    public List<DbDepartment> Search(string text)
    {
      return _provider.Departments.ToList().Where(d => d.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public bool Edit(DbDepartment department, JsonPatchDocument<DbDepartment> request)
    {
      if (department == null)
      {
        throw new ArgumentNullException(nameof(department));
      }

      Guid editorId = _httpContextAccessor.HttpContext.GetUserId();

      Operation<DbDepartment> deactivatedOperation = request.Operations
        .FirstOrDefault(o => o.path.EndsWith(nameof(DbDepartment.IsActive), StringComparison.OrdinalIgnoreCase));
      if (deactivatedOperation != null && !bool.Parse(deactivatedOperation.value.ToString()))
      {
        List<DbDepartmentUser> users = _provider.DepartmentUsers
          .Where(u => u.IsActive && u.DepartmentId == department.Id)
          .ToList();

        foreach (var user in users)
        {
          user.IsActive = false;
          user.ModifiedAtUtc = DateTime.UtcNow;
          user.ModifiedBy = editorId;
        }
      }

      request.ApplyTo(department);
      department.ModifiedBy = editorId;
      department.ModifiedAtUtc = DateTime.UtcNow;
      _provider.Save();

      return true;
    }

    public bool DoesNameExist(string name)
    {
      return _provider.Departments.Any(d => d.Name == name);
    }

    public bool Contains(Guid departmentId)
    {
      return _provider.Departments.Any(d => d.Id == departmentId);
    }
  }
}
