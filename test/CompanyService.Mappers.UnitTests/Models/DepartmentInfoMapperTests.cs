using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.ResponseMappers
{
    class DepartmentInfoMapperTests
    {
        private IDepartmentInfoMapper _mapper;

        private DbDepartment _dbDepartment;
        private UserInfo _director;
        private UserInfo _worker;
        private DepartmentInfo _expectedDepartment;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DepartmentInfoMapper();

            var directorGuid = Guid.NewGuid();
            var workerGuid = Guid.NewGuid();

            _dbDepartment = new DbDepartment()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "TestDescription",
                IsActive = true,
                Users = new List<DbDepartmentUser>
                {
                    new DbDepartmentUser
                    {
                        Id = directorGuid,
                        Role = (int)DepartmentUserRole.Director
                    },
                    new DbDepartmentUser
                    {
                        Id = workerGuid,
                        Role = (int)DepartmentUserRole.Employee
                    }
                }
            };

            _director = new UserInfo
            {
                FirstName = "Spartak",
                LastName = "Ryabtsev",
                MiddleName = "Alexandrovich"
            };

            _worker = new UserInfo
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
                IsActive = _dbDepartment.IsActive,
                CountUsers = 2
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            DbDepartment request = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(request, _director));
        }
        [Test]
        public void ShouldReturnDepartmentSuccessfully()
        {
            DbDepartment dbDepartment = _dbDepartment;
            SerializerAssert.AreEqual(_expectedDepartment, _mapper.Map(dbDepartment, _director));
        }
    }
}
