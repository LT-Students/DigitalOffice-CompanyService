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

        private Guid _departmentId;
        private List<DbDepartmentUser> _usersToAdd;
        private DbDepartmentUser _expectedDbDepartmentUser;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateInMemoryDb();

            _departmentId = Guid.NewGuid();
            _usersToAdd = new List<DbDepartmentUser>
            {
                new DbDepartmentUser
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    DepartmentId = _departmentId,
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true
                },
                new DbDepartmentUser
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    DepartmentId = _departmentId,
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true
                }
            };

            _expectedDbDepartmentUser = new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
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

    /*[Test]
    public void ShouldAddUserSuccessful()
    {
      Assert.IsTrue(_repository.Add(new List<Guid>() { _usersToAdd[0])});
            Assert.IsTrue(_provider.DepartmentUsers.Contains(_usersToAdd[0]));
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
            _repository.Remove(_expectedDbDepartmentUser.UserId, Guid.NewGuid());
            Assert.IsTrue(_provider.DepartmentUsers.Contains(_expectedDbDepartmentUser));
            Assert.IsFalse(_expectedDbDepartmentUser.IsActive);
        }

        /*[Test]
        public void ShouldFindUserIdsByDepartmentId()
        {
            int totalCount;
            var userIds = new List<Guid>
            {
                _usersToAdd[0].UserId,
                _usersToAdd[1].UserId
            };

            _provider.DepartmentUsers.AddRange(_usersToAdd);
            _provider.Save();

            SerializerAssert.AreEqual(userIds, _repository.Find(_departmentId, skipCount: 0, takeCount: userIds.Count(), out totalCount));
            Assert.AreEqual(userIds.Count, totalCount);
        }*/
    }
}
