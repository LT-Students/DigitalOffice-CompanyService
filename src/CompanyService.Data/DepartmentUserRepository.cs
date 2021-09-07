using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Data
{
  public class DepartmentUserRepository : IDepartmentUserRepository
  {
    private readonly IDataProvider _provider;

    public DepartmentUserRepository(
      IDataProvider provider)
    {
      _provider = provider;
    }

    public bool Add(DbDepartmentUser departmentUser)
    {
      if (departmentUser == null)
      {
        throw new ArgumentNullException(nameof(departmentUser));
      }

      _provider.DepartmentUsers.Add(departmentUser);
      _provider.Save();

      return true;
    }

    public DbDepartmentUser Get(Guid userId, bool includeDepartment)
    {
      DbDepartmentUser user = null;

      if (includeDepartment)
      {
        user = _provider.DepartmentUsers.Include(u => u.Department).FirstOrDefault(u => u.IsActive && u.UserId == userId);
      }
      else
      {
        user = _provider.DepartmentUsers.FirstOrDefault(u => u.IsActive && u.UserId == userId);
      }

      if (user == null)
      {
        throw new NotFoundException($"There is not user in department with id {userId}");
      }

      return user;
    }

    public List<Guid> Get(IGetDepartmentUsersRequest request, out int totalCount)
    {
      var dbDepartmentUser = _provider.DepartmentUsers.AsQueryable();

      dbDepartmentUser = dbDepartmentUser.Where(x => x.IsActive && x.DepartmentId == request.DepartmentId);

      totalCount = dbDepartmentUser.Count();

      if (request.SkipCount.HasValue)
      {
        dbDepartmentUser = dbDepartmentUser.Skip(request.SkipCount.Value);
      }

      if (request.TakeCount.HasValue)
      {
        dbDepartmentUser = dbDepartmentUser.Take(request.TakeCount.Value);
      }

      return dbDepartmentUser.Select(x => x.UserId).ToList();
    }

    public List<DbDepartmentUser> Get(List<Guid> userIds)
    {
      return _provider.DepartmentUsers
        .Include(du => du.Department)
        .Where(u => u.IsActive && userIds.Contains(u.UserId))
        .ToList();
    }

    public void Remove(Guid userId, Guid removedBy)
    {
      DbDepartmentUser user = _provider.DepartmentUsers.FirstOrDefault(u => u.IsActive && u.UserId == userId);

      if (user != null)
      {
        user.IsActive = false;
        user.ModifiedAtUtc = DateTime.UtcNow;
        user.ModifiedBy = removedBy;
      }

      _provider.Save();
    }
  }
}
