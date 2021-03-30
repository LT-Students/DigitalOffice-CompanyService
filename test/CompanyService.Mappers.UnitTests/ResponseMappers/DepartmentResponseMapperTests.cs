using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.ResponseMappers
{
    class DepartmentResponseMapperTests
    {
        private IDepartmentResponseMapper _mapper;

        private Department _expectedDepartment;
        private DbDepartment _newDbDepartment;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DepartmentResponseMapper();

            _newDbDepartment = new DbDepartment()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "TestDescription",
                DirectorUserId = Guid.NewGuid(),
                IsActive = true,
                Users = null
            };

            _expectedDepartment = new Department
            {
                Id = _newDbDepartment.Id,
                Name = _newDbDepartment.Name,
                Description = _newDbDepartment.Description,
                DirectorUserId = _newDbDepartment.DirectorUserId
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            DbDepartment request = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(request));
        }
        [Test]
        public void ShouldReturnDepartmentSuccessfully()
        {
            DbDepartment dbDepartment = _newDbDepartment;
            SerializerAssert.AreEqual(_expectedDepartment, _mapper.Map(dbDepartment));
        }
    }
}
