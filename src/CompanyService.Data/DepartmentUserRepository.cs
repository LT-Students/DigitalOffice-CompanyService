using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
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
        return false;
      }

      _provider.DepartmentUsers.Add(departmentUser);
      _provider.Save();

      return true;
    }

    public bool Add(List<DbDepartmentUser> departmentUsers)
    {
      if (departmentUsers == null || !departmentUsers.Any())
      {
        return false;
      }

      _provider.DepartmentUsers.AddRange(departmentUsers);
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

      dbDepartmentUser = dbDepartmentUser.Where(x => x.DepartmentId == request.DepartmentId);

      if (request.ByEntryDate.HasValue)
      {
        dbDepartmentUser = dbDepartmentUser.Where(x =>
          ((x.CreatedAtUtc.Year * 12 + x.CreatedAtUtc.Month) <=
            (request.ByEntryDate.Value.Year * 12 + request.ByEntryDate.Value.Month)) &&
          (x.IsActive ||
            ((x.LeftAtUts.Value.Year * 12 + x.LeftAtUts.Value.Month) >=
            (request.ByEntryDate.Value.Year * 12 + request.ByEntryDate.Value.Month))));
      }
      else
      {
        dbDepartmentUser = dbDepartmentUser.Where(x => x.IsActive);
      }

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
      DbDepartmentUser dbDepartmentUser = _provider.DepartmentUsers
        .FirstOrDefault(du => du.UserId == userId && du.IsActive);

      if (dbDepartmentUser != null)
      {
        dbDepartmentUser.IsActive = false;
        dbDepartmentUser.ModifiedAtUtc = DateTime.UtcNow;
        dbDepartmentUser.ModifiedBy = removedBy;
        dbDepartmentUser.LeftAtUts = DateTime.UtcNow;

        _provider.Save();
      }
    }

    public void Remove(List<Guid> usersIds, Guid removedBy)
    {
      IEnumerable<DbDepartmentUser> dbDepartmentsUsers = _provider.DepartmentUsers
        .Where(du => du.IsActive && usersIds.Contains(du.UserId));

      if (usersIds != null && usersIds.Any())
      {
        foreach (DbDepartmentUser du in dbDepartmentsUsers)
        {
          du.IsActive = false;
          du.ModifiedAtUtc = DateTime.UtcNow;
          du.ModifiedBy = removedBy;
          du.LeftAtUts = DateTime.UtcNow;
        };

        _provider.Save();
      }
    }

    public bool IsDepartmentDirector(Guid departmentId, Guid userId)
    {
      return _provider.DepartmentUsers
        .FirstOrDefault(x => x.UserId == userId && x.DepartmentId == departmentId && x.IsActive)?
        .Role == (int)DepartmentUserRole.Director;
    }
  }
}
