using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    public class CompanyRepositoryTests
    {
        private IDataProvider provider;
        private ICompanyRepository repository;

        private DbCompany dbCompanyInDb;
        private DbCompany dbCompanyToAdd;
        private DbCompany dbCompanyToUpdate;
        private DbCompany expectedDbCompanyAfterUpdate;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
            provider = new CompanyServiceDbContext(dbOptions);

            repository = new CompanyRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            dbCompanyInDb = new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = "Lanit-Tercom",
                IsActive = true
            };

            provider.Companies.Add(dbCompanyInDb);
            provider.SaveChanges();

            AddCompanySetUp();
            UpdateCompanySetUp();
        }

        private void AddCompanySetUp()
        {
            dbCompanyToAdd = new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = "Lanit-Tercom2",
                IsActive = true
            };
        }

        private void UpdateCompanySetUp()
        {
            var name = dbCompanyInDb.Name + "abracadabra";
            var changedIsActive = !dbCompanyInDb.IsActive;

            dbCompanyToUpdate = new DbCompany
            {
                Id = dbCompanyInDb.Id,
                Name = name,
                IsActive = changedIsActive
            };

            expectedDbCompanyAfterUpdate = new DbCompany
            {
                Id = dbCompanyInDb.Id,
                Name = name,
                IsActive = changedIsActive
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

        #region GetCompanyById
        [Test]
        public void ShouldThrowExceptionWhenCompanyDoesNotExist()
        {
            Assert.Throws<Exception>(() => repository.GetCompanyById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldRightGetCompanyById()
        {
            var actualCompany = repository.GetCompanyById(dbCompanyInDb.Id);

            var expectedCompany = provider.Companies.Find(dbCompanyInDb.Id);
            SerializerAssert.AreEqual(expectedCompany, actualCompany);
        }
        #endregion

        #region GetCompaniesList
        [Test]
        public void ShouldRightGetCompaniesList()
        {
            var actualCompaniesList = repository.GetCompaniesList();
            var expectedCompaniesList = provider.Companies.ToList();

            Assert.AreEqual(actualCompaniesList.Count, expectedCompaniesList.Count);
            foreach (var company in expectedCompaniesList)
            {
                Assert.IsTrue(actualCompaniesList.Contains(company));
            }
        }
        #endregion

        #region AddCompany
        [Test]
        public void ShouldReturnMatchingIdAndRightAddCompanyInDb()
        {
            var guidOfNewCompany = repository.AddCompany(dbCompanyToAdd);

            Assert.AreEqual(dbCompanyToAdd.Id, guidOfNewCompany);
            Assert.NotNull(provider.Companies.Find(dbCompanyToAdd.Id));
        }
        #endregion

        #region UpdateCompany
        [Test]
        public void ShouldThrowExceptionWhenCompanyForUpdateDoesNotExist()
        {
            Assert.Throws<Exception>(() => repository.UpdateCompany(
                new DbCompany() { Id = Guid.Empty }));
        }

        [Test]
        public void ShouldUpdateCompany()
        {
            provider.MakeEntityDetached(dbCompanyInDb);
            var result = repository.UpdateCompany(dbCompanyToUpdate);
            var resultCompany = provider.Companies
                .FirstOrDefaultAsync(x => x.Id == dbCompanyToUpdate.Id)
                .Result;

            Assert.IsTrue(result);
            SerializerAssert.AreEqual(expectedDbCompanyAfterUpdate, resultCompany);
        }
        #endregion
    }
}