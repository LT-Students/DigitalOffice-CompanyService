using CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    internal class DbDepartmentMapperTests
    {
        private IDbDepartmentMapper _mapper;

        private CreateDepartmentRequest _request;
        private BaseDepartmentInfo _newDepartment;
        private DbDepartment _expectedDbDepartment;
        private Guid _companyId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _companyId = Guid.NewGuid();

            _mapper = new DbDepartmentMapper();

            _newDepartment = new BaseDepartmentInfo()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid()
            };

            var newUsers = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _request = new CreateDepartmentRequest
            {
                Info = _newDepartment,
                Users = newUsers
            };

            _expectedDbDepartment = new DbDepartment
            {
                Name = _newDepartment.Name,
                CompanyId = _companyId,
                Description = _newDepartment.Description,
                IsActive = true,
                DirectorUserId = _newDepartment.DirectorUserId
            };

            foreach (var userId in newUsers)
            {
                _expectedDbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        UserId = userId,
                        IsActive = true
                    });
            }
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
            var newDepartment = new CreateDepartmentRequest
            {
                Info = _newDepartment
            };

            var expectedDbDepartment = new DbDepartment
            {
                Name = _newDepartment.Name,
                CompanyId = _companyId,
                Description = _newDepartment.Description,
                IsActive = true,
                DirectorUserId = _newDepartment.DirectorUserId,
                Users = null
            };

            var dbDepartment = _mapper.Map(newDepartment, _companyId);

            expectedDbDepartment.Id = dbDepartment.Id;

            SerializerAssert.AreEqual(expectedDbDepartment, dbDepartment);
        }

        [Test]
        public void ShouldReturnDbDepartmentSuccessfully()
        {
            var dbDepartment = _mapper.Map(_request, _companyId);

            _expectedDbDepartment.Id = dbDepartment.Id;

            for (int i = 0; i < dbDepartment.Users.Count; i++)
            {
                _expectedDbDepartment.Users.ElementAt(i).StartTime =
                    dbDepartment.Users.ElementAt(i).StartTime;

                _expectedDbDepartment.Users.ElementAt(i).Id =
                    dbDepartment.Users.ElementAt(i).Id;

                _expectedDbDepartment.Users.ElementAt(i).DepartmentId = dbDepartment.Id;
            }

            SerializerAssert.AreEqual(_expectedDbDepartment, dbDepartment);
        }
    }
}
