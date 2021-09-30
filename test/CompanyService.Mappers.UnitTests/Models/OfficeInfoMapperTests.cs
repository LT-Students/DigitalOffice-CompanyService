using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    public class OfficeInfoMapperTests
    {
        private IOfficeInfoMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new OfficeInfoMapper();
        }

/*        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }*/

        [Test]
        public void ShouldMapSuccessfuly()
        {
            DbOffice office = new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Address = "address",
                City = "company",
                CompanyId = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            OfficeInfo expected = new()
            {
                Id = office.Id,
                Name = "Name",
                Address = "address",
                City = "company",
                IsActive = true
            };

            SerializerAssert.AreEqual(expected, _mapper.Map(office));
        }
    }
}
