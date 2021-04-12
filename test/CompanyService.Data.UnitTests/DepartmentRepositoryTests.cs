﻿using LT.DigitalOffice.CompanyService.Data;
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

namespace CompanyService.Data.UnitTests
{
    public class DepartmentRepositoryTests
    {
        private IDataProvider _provider;
        private IDepartmentRepository _repository;

        private DbDepartment _departmentToAdd;
        private DbDepartment _dbDepartment;
        private DbContextOptions<CompanyServiceDbContext> _dbOptions;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateMemoryDb();

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
                        StartTime = DateTime.UtcNow
                    }
                }
            };

            _dbDepartment = new DbDepartment
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
                        StartTime = DateTime.UtcNow
                    }
                }
            };
        }

        public void CreateMemoryDb()
        {
            _dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                   .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                   .Options;
        }

        [SetUp]
        public void SetUp()
        {
            _provider = new CompanyServiceDbContext(_dbOptions);
            _repository = new DepartmentRepository(_provider);

            _provider.Departments.Add(_dbDepartment);
            _provider.Save();
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        [Test]
        public void ShouldAddDepartmentInDb()
        {
            var guidOfAddedDepartment = _repository.CreateDepartment(_departmentToAdd);

            Assert.AreEqual(_departmentToAdd.Id, guidOfAddedDepartment);
            Assert.AreEqual(_departmentToAdd, _provider.Departments.Find(_departmentToAdd.Id));
        }

        [Test]
        public void ShouldNotFoundExceptionWhenDepartmentIdNotFound()
        {
            var departmentId = Guid.NewGuid();

            Assert.Throws<NotFoundException>(() => _repository.GetDepartment(departmentId));
        }

        [Test]
        public void ShouldGetDepartmenByIdSuccessfully()
        {
            var dbDepartment = _repository.GetDepartment(_dbDepartment.Id);

            foreach (var user in dbDepartment.Users)
            {
                user.Department = null;
            }

            SerializerAssert.AreEqual(_dbDepartment, dbDepartment);
        }
    }
}
