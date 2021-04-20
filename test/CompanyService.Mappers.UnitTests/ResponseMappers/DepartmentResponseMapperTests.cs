using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.ResponseMappers
{
    class DepartmentResponseMapperTests
    {
        private IDepartmentResponseMapper _mapper;

        private DepartmentInfo _expectedDepartment;
        private DbDepartment _dbDepartment;
        private User _director;
        private User _worker;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DepartmentResponseMapper();

            var directorGuid = Guid.NewGuid();
            var workerGuid = Guid.NewGuid();

            _dbDepartment = new DbDepartment()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "TestDescription",
                DirectorUserId = directorGuid,
                IsActive = true,
                Users = null
            };

            _director = new User
            {
                FirstName = "Spartak",
                LastName = "Ryabtsev",
                MiddleName = "Alexandrovich"
            };

            _worker = new User
            {
                FirstName = "Pavel",
                LastName = "Kostin",
                MiddleName = "Alexandrovich"
            };

            _expectedDepartment = new DepartmentInfo()
            {
                Id = _dbDepartment.Id,
                Name = _dbDepartment.Name,
                Description = _dbDepartment.Description,
                Director = _director,
                Users = new List<User> { _director, _worker }
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            DbDepartment request = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(request, _director, new List<User> { _director, _worker }));
        }
        [Test]
        public void ShouldReturnDepartmentSuccessfully()
        {
            DbDepartment dbDepartment = _dbDepartment;
            SerializerAssert.AreEqual(_expectedDepartment, _mapper.Map(dbDepartment, _director, new List<User> { _director, _worker }));
        }
    }
}
