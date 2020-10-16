using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;

namespace LT.DigitalOffice.CompanyService.Data
{
    /// <inheritdoc cref="IDepartmentRepository"/>
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IDataProvider provider;

        public DepartmentRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public Guid AddDepartment(DbDepartment department)
        {
            provider.Departments.Add(department);
            provider.Save();

            return department.Id;
        }
    }
}
