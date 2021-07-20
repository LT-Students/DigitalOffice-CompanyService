using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    /// <inheritdoc />
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDataProvider _provider;

        public DepartmentRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public Guid CreateDepartment(DbDepartment department)
        {
            _provider.Departments.Add(department);
            _provider.Save();

            return department.Id;
        }

        /// <inheritdoc />
        public DbDepartment GetDepartment(Guid? departmentId, Guid? userId)
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
                dbDepartment = _provider.Departments
                    .Include(d => d.Users.Where(du => du.UserId == userId.Value && du.IsActive))
                    .FirstOrDefault();
            }

            if (dbDepartment == null)
            {
                string valueMessage = departmentId.HasValue ? $"department id: {departmentId.Value}" : $"user id: {userId.Value}";

                throw new NotFoundException($"Department was not found by specific {valueMessage}");
            }

            return dbDepartment;
        }

        public DbDepartment GetDepartment(GetDepartmentFilter filter)
        {
            IQueryable<DbDepartment> dbDepartments = _provider.Departments.AsQueryable();

            dbDepartments = dbDepartments.Where(d => d.Id == filter.DepartmentId);

            if (filter.IsIncludeUsers)
            {
                dbDepartments = dbDepartments.Include(d => d.Users.Where(u => u.IsActive));
            }

            return dbDepartments.FirstOrDefault()
                ?? throw new NotFoundException($"Department was not found by specific {filter.DepartmentId}");
        }

        /// <inheritdoc />
        public List<DbDepartment> FindDepartments(int skipCount, int takeCount, bool includeDeactivated, out int totalCount)
        {
            if (skipCount < 0)
            {
                throw new BadRequestException("Skip count can't be less than 0.");
            }

            if (takeCount <= 0)
            {
                throw new BadRequestException("Take count can't be equal or less than 0.");
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

            return dbDepartments.Skip(skipCount).Take(takeCount).Include(d => d.Users.Where(u => u.IsActive)).ToList();
        }

        public List<DbDepartment> FindDepartments(List<Guid> departmentIds)
        {
            return _provider.Departments.Where(d => departmentIds.Contains(d.Id)).ToList();
        }

        public List<DbDepartment> Search(string text)
        {
            return _provider.Departments.ToList().Where(d => d.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool Edit(Guid departmentId, JsonPatchDocument<DbDepartment> request)
        {
            DbDepartment dbDepartment;

            Operation<DbDepartment> deactivatedOperation = request.Operations.FirstOrDefault(o => o.path.EndsWith(nameof(DbDepartment.IsActive), StringComparison.OrdinalIgnoreCase));
            if (deactivatedOperation != null && !bool.Parse(deactivatedOperation.value.ToString()))
            {
                dbDepartment = _provider.Departments.Include(d => d.Users.Where(u => u.IsActive)).FirstOrDefault(d => d.Id == departmentId)
                    ?? throw new NotFoundException($"Department with this id: '{departmentId}' was not found.");

                foreach (var user in dbDepartment?.Users)
                {
                    user.IsActive = false;
                }
            }
            else
            {
                dbDepartment = _provider.Departments.FirstOrDefault(d => d.Id == departmentId)
                    ?? throw new NotFoundException($"Department with this id: '{departmentId}' was not found.");
            }

            request.ApplyTo(dbDepartment);
            _provider.Save();

            return true;
        }

        public bool IsNameExist(string name)
        {
            return _provider.Departments.Any(d => d.Name == name);
        }
    }
}
