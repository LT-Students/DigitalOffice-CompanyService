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
                user = _provider.DepartmentUsers.Include(u => u.Department).FirstOrDefault(u => u.UserId == userId);
            }
            else
            {
                user = _provider.DepartmentUsers.FirstOrDefault(u => u.UserId == userId);
            }

            if (user == null)
            {
                throw new NotFoundException($"There is not user in department with id {userId}");
            }

            return user;
        }

        public IEnumerable<Guid> Find(Guid departmentId, int skipCount, int takeCount, out int totalCount)
        {
            var dbDepartmentUser = _provider.DepartmentUsers.AsQueryable();

            dbDepartmentUser = dbDepartmentUser.Where(x => x.DepartmentId == departmentId);

            totalCount = dbDepartmentUser.Count();

            return dbDepartmentUser.Skip(skipCount * takeCount).Take(takeCount).Select(x => x.UserId).ToList();
        }

        public void Remove(Guid userId)
        {
            DbDepartmentUser user = _provider.DepartmentUsers.FirstOrDefault(u => u.UserId == userId && u.IsActive);

            if (user != null)
            {
                user.IsActive = false;
            }
        }
    }
}
