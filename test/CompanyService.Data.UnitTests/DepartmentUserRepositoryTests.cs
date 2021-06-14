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
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    public class DepartmentUserRepositoryTests
    {
        private IDataProvider _provider;
        private IDepartmentUserRepository _repository;

        private DbDepartmentUser _userToAdd;
        private DbDepartmentUser _expectedDbDepartmentUser;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateInMemoryDb();

            _userToAdd = new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                IsActive = true
            };

            _expectedDbDepartmentUser = new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                IsActive = true
            };
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
            _repository = new DepartmentUserRepository(_provider);

            _provider.DepartmentUsers.Add(_expectedDbDepartmentUser);
            _provider.Save();
        }

        [TearDown]
        public void CleanInMemoryDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        [Test]
        public void ShouldAddUserSuccessful()
        {
            Assert.IsTrue(_repository.Add(_userToAdd));
            Assert.IsTrue(_provider.DepartmentUsers.Contains(_userToAdd));
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelToAddIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Test]
        public void ShouldGetUserSuccessful()
        {
            SerializerAssert.AreEqual(_expectedDbDepartmentUser, _repository.Get(_expectedDbDepartmentUser.UserId, false));
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenThereAreNotUsersWithNeededId()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get(Guid.NewGuid(), false));
        }

        [Test]
        public void ShouldRemoveUserSuccessful()
        {
            _repository.Remove(_expectedDbDepartmentUser.UserId);
            Assert.IsTrue(_provider.DepartmentUsers.Contains(_expectedDbDepartmentUser));
            Assert.IsFalse(_expectedDbDepartmentUser.IsActive);
        }
    }
}
