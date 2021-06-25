using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    public class CompanyRepositoryTests
    {
        private IDataProvider _provider;
        private ICompanyRepository _repository;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateInMemoryDb();
        }

        public void CreateInMemoryDb()
        {
            _dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
        }

        [SetUp]
        public void SetUp()
        {
            _provider = new CompanyServiceDbContext(_dbOptions);
            _repository = new CompanyRepository(_provider);
        }

        [TearDown]
        public void CleanInMemoryDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region Add tests

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelsToAddIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Test]
        public void ShouldAddSuccessfuly()
        {
            DbCompany company = new()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Description = "Desc",
                LogoId = Guid.NewGuid(),
                Name = "Name",
                Tagline = "tagline"
            };

            _repository.Add(company);

            Assert.IsTrue(_provider.Companies.Contains(company));
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenCompanyExist()
        {
            DbCompany company1 = new();
            DbCompany company2 = new();

            _repository.Add(company1);

            Assert.Throws<BadRequestException>(() => _repository.Add(company2));
        }

        #endregion

        #region Get Tests

        [Test]
        public void ShouldGetCompanySuccessfuly()
        {
            DbCompany company = new DbCompany();

            _provider.Companies.Add(company);
            _provider.Save();

            Assert.AreEqual(company, _repository.Get());
        }

        [Test]
        public void ShouldThrowExceptionWhetCompanyDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get());
        }

        #endregion
    }
}
