using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class DbPositionUserMapperTests
    {
        private DbPositionUserMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new();
        }

        [Test]
        public void ShouldMapSuccessful()
        {
            var positionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var response = _mapper.Map(positionId, userId);

            var expectedResponse = new DbPositionUser
            {
                Id = response.Id,
                PositionId = positionId,
                UserId = userId,
                IsActive = true,
                CreatedAtUtc = response.CreatedAtUtc
            };

            SerializerAssert.AreEqual(expectedResponse, response);
        }
    }
}
