using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    public class PositionUserRepositoryTests
    {
        private IDataProvider _provider;
        private IPositionUserRepository _repository;

        private DbPositionUser _userToAdd;
        private DbPositionUser _expectedDbPositionUser;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateInMemoryDb();

            _userToAdd = new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            _expectedDbPositionUser = new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
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
            _repository = new PositionUserRepository(_provider);

            _provider.PositionUsers.Add(_expectedDbPositionUser);
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

        /*[Test]
        public void ShouldAddUserSuccessful()
        {
            Assert.IsTrue(_repository.Add(_userToAdd));
            Assert.IsTrue(_provider.PositionUsers.Contains(_userToAdd));
        }*/

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelToAddIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
        }

        [Test]
        public void ShouldGetUserSuccessful()
        {
            SerializerAssert.AreEqual(_expectedDbPositionUser, _repository.Get(_expectedDbPositionUser.UserId, false));
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenThereAreNotUsersWithNeededId()
        {
            Assert.Throws<NotFoundException>(() => _repository.Get(Guid.NewGuid(), false));
        }

        /*[Test]
        public void ShouldRemoveUserSuccessful()
        {
            _repository.Remove(_expectedDbPositionUser.UserId);
            Assert.IsTrue(_provider.PositionUsers.Contains(_expectedDbPositionUser));
            Assert.IsFalse(_expectedDbPositionUser.IsActive);
        }*/
    }
}
