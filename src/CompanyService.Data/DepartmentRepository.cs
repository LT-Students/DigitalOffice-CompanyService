using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    /// <inheritdoc />
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDataProvider provider;

        public DepartmentRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        /// <inheritdoc />
        public Guid CreateDepartment(DbDepartment department)
        {
            provider.Departments.Add(department);
            provider.Save();

            return department.Id;
        }

        /// <inheritdoc />
        public DbDepartment GetDepartment(Guid id)
        {
            var result = provider.Departments.FirstOrDefault(d => d.Id == id);

            if (result == null)
            {
                throw new NotFoundException($"Department with id: '{id}' was not found.");
            }

            return result;
        }
    }
}
