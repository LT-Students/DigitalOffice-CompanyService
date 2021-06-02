using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;

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

            _provider.DepartmentsUsers.Add(departmentUser);
            _provider.Save();

            return true;
        }
    }
}
