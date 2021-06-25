using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    public class ShortDepartmentsInfoMapperTests
    {
        private IShortDepartmentInfoMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new ShortDepartmentInfoMapper();
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            DbDepartment department = new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CompanyId = Guid.NewGuid(),
                Description = "Description",
                IsActive = true
            };

            ShortDepartmentInfo expected = new()
            {
                Id = department.Id,
                Name = "Name",
                Description = "Description",
                IsActive = true
            };

            SerializerAssert.AreEqual(expected, _mapper.Map(department));
        }
    }
}
