using LT.DigitalOffice.CompanyService.Data;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyService.Data.UnitTests
{
    public class DepartmentRepositoryTests
    {
        private IDataProvider _provider;
        private IDepartmentRepository _repository;
        private Mock<IHttpContextAccessor> _accessorMock;

        private Guid _userId;
        private DbDepartment _departmentToAdd;
        private DbDepartment _expectedDbDepartment;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateInMemoryDb();

            _userId = Guid.NewGuid();

            _accessorMock = new();
            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _accessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(_items);

            _departmentToAdd = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                IsActive = true,
                Users = new List<DbDepartmentUser>()
                {
                    new DbDepartmentUser
                    {
                        UserId  = Guid.NewGuid(),
                        IsActive = true,
                        CreatedAtUtc = DateTime.UtcNow
                    }
                }
            };

            _expectedDbDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = "Digital solution",
                Description = "Description",
                IsActive = true,
                Users = new List<DbDepartmentUser>()
                {
                    new DbDepartmentUser
                    {
                        UserId  = Guid.NewGuid(),
                        IsActive = true,
                        CreatedAtUtc = DateTime.UtcNow
                    }
                }
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
            _repository = new DepartmentRepository(_provider, _accessorMock.Object);

            _provider.Departments.Add(_expectedDbDepartment);
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

        #region CreateDepartment

        [Test]
        public void ShouldAddDepartmentInDb()
        {
            var guidOfAddedDepartment = _repository.CreateDepartment(_departmentToAdd);

            Assert.AreEqual(_departmentToAdd.Id, guidOfAddedDepartment);
            Assert.AreEqual(_departmentToAdd, _provider.Departments.Find(_departmentToAdd.Id));
        }

        #endregion

        #region GetRepartment

        // TODO fix
        //[Test]
        //public void ShouldNotFoundExceptionWhenDepartmentIdNotFound()
        //{
        //    var departmentId = Guid.NewGuid();

        //    Assert.Throws<NotFoundException>(() => _repository.GetDepartment(departmentId, null));
        //}

        [Test]
        public void ShouldGetDepartmenByIdSuccessfully()
        {
            var dbDepartment = _repository.GetDepartment(_expectedDbDepartment.Id, null);

            foreach (var user in dbDepartment.Users)
            {
                user.Department = null;
            }

            SerializerAssert.AreEqual(_expectedDbDepartment, dbDepartment);
        }

        #endregion

        #region FindDepartments

        [Test]
        public void ShouldFindDepartmentsSuccessfully()
        {
            var dbDepartment = _repository.FindDepartments(0, 1, true, out _).First();

            _expectedDbDepartment.Users.First().Department = null;
            dbDepartment.Users.First().Department = null;

            SerializerAssert.AreEqual(_expectedDbDepartment, dbDepartment);

        }

        #endregion
    }
}
