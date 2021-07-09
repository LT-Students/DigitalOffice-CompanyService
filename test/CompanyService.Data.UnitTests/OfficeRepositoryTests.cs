using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    public class OfficeRepositoryTests
    {
        private IDataProvider _provider;
        private IOfficeRepository _repository;
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
            _repository = new OfficeRepository(_provider);
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
            DbOffice office = new()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Address = "Address",
                City = "City",
                Name = "Name",
                CompanyId = Guid.NewGuid()
            };

            _repository.Add(office);

            Assert.IsTrue(_provider.Offices.Contains(office));
        }

        #endregion

        #region Find Tests

        [Test]
        public void ShouldFindOfficeSuccessfuly()
        {
            DbOffice office1 = new()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Address = "Address",
                City = "City",
                Name = "Name",
                CompanyId = Guid.NewGuid()
            };
            DbOffice office2 = new()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = false,
                Address = "Address",
                City = "City",
                Name = "Name",
                CompanyId = Guid.NewGuid()
            };

            _provider.Offices.Add(office1);
            _provider.Offices.Add(office2);
            _provider.Save();

            SerializerAssert.AreEqual(new List<DbOffice>() { office1, office2 }, _repository.Find(0, 2, true, out int totalCount));
            Assert.AreEqual(2, totalCount);
        }

        [Test]
        public void ShouldThrowBadRequestExceptionWhenTakeCountLessThanOne()
        {
            Assert.Throws<BadRequestException>(() => _repository.Find(0, 0, true, out int totalCount));
        }

        #endregion
    }
}
