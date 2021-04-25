using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    class BaseDepartmentInfoMapperTests
    {
        private IBaseDepartmentInfoMapper _mapper;

        private DbDepartment _newDbDepartment;
        private BaseDepartmentInfo _expectedDepartment;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new BaseDepartmentInfoMapper();

            _newDbDepartment = new DbDepartment()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "TestDescription",
                DirectorUserId = Guid.NewGuid(),
                IsActive = true,
                Users = null
            };

            _expectedDepartment = new BaseDepartmentInfo()
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
