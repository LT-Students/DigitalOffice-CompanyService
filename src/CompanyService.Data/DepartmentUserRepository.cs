using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Guid> Find(Guid departmentId, int skipCount, int takeCount, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new ArgumentException("Skip count can't be less than 0.");
            }

            if (takeCount <= 0)
            {
                throw new ArgumentException("Take count can't be equal or less than 0.");
            }

            var dbDepartmentUser = _provider.DepartmentUsers.AsQueryable();

            dbDepartmentUser = dbDepartmentUser.Where(x => x.IsActive && x.DepartmentId == departmentId);

            totalCount = dbDepartmentUser.Count();

            return dbDepartmentUser.Skip(skipCount).Take(takeCount).Select(x => x.UserId).ToList();
        }

        public List<DbDepartmentUser> Find(List<Guid> userIds)
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
