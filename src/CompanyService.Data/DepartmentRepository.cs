using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.AspNetCore.JsonPatch;
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
                    .Include(d => d.Users)
                    .FirstOrDefault(d => d.Id == departmentId.Value);
            }
            else
            {
                dbDepartment = _provider.Departments
                    .Include(d => d.Users.Where(du => du.UserId == userId.Value))
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
                dbDepartments = dbDepartments.Include(d => d.Users);
            }

            return dbDepartments.FirstOrDefault()
                ?? throw new NotFoundException($"Department was not found by specific {filter.DepartmentId}");
        }

        /// <inheritdoc />
        public List<DbDepartment> FindDepartments()
        {
            return _provider.Departments.Include(x => x.Users).ToList();
        }

        public List<DbDepartment> Search(string text)
        {
            return _provider.Departments.ToList().Where(d => d.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool Edit(Guid departmentId, JsonPatchDocument<DbDepartment> request)
        {
            var dbDepartment = _provider.Departments.FirstOrDefault(d => d.Id == departmentId)
                ?? throw new NotFoundException($"Department with this id: '{departmentId}' was not found.");

            request.ApplyTo(dbDepartment);
            _provider.Save();

            return true;
        }

        public bool IsNameExist(string name)
        {
            return _provider.Departments.Any(p => p.Name == name);
        }
    }
}
