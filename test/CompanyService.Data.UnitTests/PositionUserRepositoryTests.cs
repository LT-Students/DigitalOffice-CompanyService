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

        private DbPosition _position;
        private DbPositionUser _userToAdd;
        private DbPositionUser _expectedDbPositionUser;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateInMemoryDb();

            _position = new()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                IsActive = true
            };

            _userToAdd = new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PositionId = _position.Id,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            _expectedDbPositionUser = new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PositionId = _position.Id,
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

            _provider.Positions.Add(_position);
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
            var result = _repository.Get(_expectedDbPositionUser.UserId);
            Assert.AreEqual(_expectedDbPositionUser.Id, result.Id);
            Assert.AreEqual(_expectedDbPositionUser.UserId, result.UserId);
            Assert.AreEqual(_expectedDbPositionUser.PositionId, result.PositionId);
            Assert.AreEqual(_expectedDbPositionUser.IsActive, result.IsActive);
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenThereAreNotUsersWithNeededId()
        {
            Assert.IsNull(_repository.Get(Guid.NewGuid()));
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
