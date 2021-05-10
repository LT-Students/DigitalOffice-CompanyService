using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Responses
{
    class PositionResponseMapperTests
    {
        private IPositionResponseMapper _mapper;

        private DbDepartmentUser _dbUserIds;
        private DbPosition _expectedDbPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new PositionResponseMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _dbUserIds = new DbDepartmentUser
            {
                UserId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                IsActive = true,
                StartTime = new DateTime()
            };
            _expectedDbPosition = new DbPosition
            {
                Id = _dbUserIds.PositionId,
                Name = "Position",
                Description = "Description",
                IsActive = true,
                Users = new List<DbDepartmentUser> { _dbUserIds }
            };
        }

        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullDbPositionToPosition()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfully()
        {
            var result = _mapper.Map(_expectedDbPosition);

            var expected = new PositionResponse
            {
                Info = new PositionInfo
                {
                    Id = _expectedDbPosition.Id,
                    Name = _expectedDbPosition.Name,
                    Description = _expectedDbPosition.Description,
                    IsActive = _expectedDbPosition.IsActive
                },
                UserIds = _expectedDbPosition.Users?.Select(x => x.UserId).ToList()
            };

            SerializerAssert.AreEqual(expected, result);
        }
    }
}
