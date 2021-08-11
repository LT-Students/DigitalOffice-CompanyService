using CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    internal class DbDepartmentMapperTests
    {
        private AutoMocker _mocker;
        private IDbDepartmentMapper _mapper;

        private CreateDepartmentRequest _request;
        private DbDepartment _expectedDbDepartment;
        private Guid _companyId;
        private Guid _userId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _companyId = Guid.NewGuid();
            _userId = Guid.NewGuid();

            _mocker = new();
            _mapper = _mocker.CreateInstance<DbDepartmentMapper>();

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _mocker
                .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
                .Returns(_items);

            var newUsers = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _request = new CreateDepartmentRequest
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid(),
                Users = newUsers
            };

            _expectedDbDepartment = new DbDepartment
            {
                Name = _request.Name,
                CompanyId = _companyId,
                Description = _request.Description,
                CreatedBy = _userId,
                IsActive = true
            };

            foreach (var userId in newUsers)
            {
                _expectedDbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        UserId = userId,
                        Role = (int)DepartmentUserRole.Employee,
                        IsActive = true,
                        CreatedBy = _userId
                    });
            }

            _expectedDbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        UserId = _request.DirectorUserId.Value,
                        Role = (int)DepartmentUserRole.Director,
                        CreatedBy = _userId,
                        IsActive = true
                    });
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            CreateDepartmentRequest request = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(request, _companyId));
        }

        [Test]
        public void ShouldReturnDbDepartmentWithoutUserSuccessfully()
        {
            CreateDepartmentRequest request = new()
            {
                Name = _request.Name,
                Description = _request.Description
            };

            var expectedDbDepartment = new DbDepartment
            {
                Name = _request.Name,
                CompanyId = _companyId,
                Description = _request.Description,
                IsActive = true,
                CreatedBy = _userId,
                Users = new List<DbDepartmentUser>()
            };

            var dbDepartment = _mapper.Map(request, _companyId);

            expectedDbDepartment.Id = dbDepartment.Id;
            expectedDbDepartment.CreatedAtUtc = dbDepartment.CreatedAtUtc;

            SerializerAssert.AreEqual(expectedDbDepartment, dbDepartment);
        }

        [Test]
        public void ShouldReturnDbDepartmentSuccessfully()
        {
            var dbDepartment = _mapper.Map(_request, _companyId);

            _expectedDbDepartment.Id = dbDepartment.Id;
            _expectedDbDepartment.CreatedAtUtc = dbDepartment.CreatedAtUtc;

            for (int i = 0; i < dbDepartment.Users.Count; i++)
            {
                _expectedDbDepartment.Users.ElementAt(i).CreatedAtUtc =
                    dbDepartment.Users.ElementAt(i).CreatedAtUtc;

                _expectedDbDepartment.Users.ElementAt(i).Id =
                    dbDepartment.Users.ElementAt(i).Id;

                _expectedDbDepartment.Users.ElementAt(i).DepartmentId = dbDepartment.Id;
            }

            SerializerAssert.AreEqual(_expectedDbDepartment, dbDepartment);
        }
    }
}
