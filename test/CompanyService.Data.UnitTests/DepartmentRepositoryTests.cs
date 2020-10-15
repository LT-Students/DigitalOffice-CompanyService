using LT.DigitalOffice.CompanyService.Data;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace CompanyService.Data.UnitTests
{
    public class DepartmentRepositoryTests
    {
        private IDataProvider provider;
        private IDepartmentRepository repository;

        private DbDepartment departmentToAdd;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            provider = new CompanyServiceDbContext(dbOptions);

            repository = new DepartmentRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            departmentToAdd = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                CompanyId = Guid.NewGuid(),
                IsActive = true
            };
        }

        [TearDown]
        public void CleanDb()
        {
            if (provider.IsInMemory())
            {
                provider.EnsureDeleted();
            }
        }

        #region AddDepartment
        [Test]
        public void ShouldAddDepartmentInDb()
        {
            var guidOfAddedDepartment = repository.AddDepartment(departmentToAdd);

            Assert.AreEqual(departmentToAdd.Id, guidOfAddedDepartment);
            Assert.AreEqual(departmentToAdd, provider.Departments.Find(departmentToAdd.Id));
        }
        #endregion
    }
}
