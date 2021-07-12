using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
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
            if (departmentId.HasValue)
            {
                return _provider.Departments.FirstOrDefault(d => d.Id == departmentId.Value);
            }

            if (userId.HasValue)
            {
                return _provider.Departments
                    .Include(d => d.Users.Where(du => du.UserId == userId.Value))
                    .FirstOrDefault();
            }

            throw new BadRequestException("You must specify 'departmentId' or 'userId'.");
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
    }
}
