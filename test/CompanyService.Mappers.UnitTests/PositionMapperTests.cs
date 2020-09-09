using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests
{
    public class PositionMappersTests
    {
        private IMapper<AddPositionRequest, DbPosition> mapperAddPositionRequest;
        private IMapper<DbPosition, Position> mapperDbPositionToPosition;
        private IMapper<EditPositionRequest, DbPosition> mapperEditPositionRequestToDbPosition;

        private DbCompanyUser dbUserIds;
        private DbPosition dbPosition;

        private EditPositionRequest editPositionRequest;
        private AddPositionRequest addPositionRequest;
        private DbPosition expectedDbPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapperDbPositionToPosition = new PositionMapper();
            mapperEditPositionRequestToDbPosition = new PositionMapper();
            mapperAddPositionRequest = new PositionMapper();
        }

        [SetUp]
        public void SetUp()
        {
            editPositionRequest = new EditPositionRequest
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description"
            };

            addPositionRequest = new AddPositionRequest
            {
                Name = "Name",
                Description = "Description"
            };
            expectedDbPosition = new DbPosition
            {
                Name = addPositionRequest.Name,
                Description = addPositionRequest.Description,
                IsActive = true
            };

            dbUserIds = new DbCompanyUser
            {
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                IsActive = true,
                StartTime = new DateTime()
            };
            dbPosition = new DbPosition
            {
                Id = dbUserIds.PositionId,
                Name = "Position",
                Description = "Description",
                IsActive = true,
                UserIds = new List<DbCompanyUser> { dbUserIds }
            };
        }

        #region AddPositionRequest to DbPosition
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullAddPositionRequestToDbPosition()
        {
            Assert.Throws<ArgumentNullException>(() => mapperAddPositionRequest.Map(null));
        }

        public void ShouldMapAddPositionRequestToDbPositionSuccessfully()
        {
            var resultDbPosition = mapperAddPositionRequest.Map(addPositionRequest);

            expectedDbPosition.Id = resultDbPosition.Id;

            Assert.IsInstanceOf<Guid>(resultDbPosition.Id);
            SerializerAssert.AreEqual(expectedDbPosition, resultDbPosition);
        }
        #endregion

        #region DbPosition to Position
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullDbPositionToPosition()
        {
            Assert.Throws<ArgumentNullException>(() => mapperDbPositionToPosition.Map(null));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfully()
        {
            var result = mapperDbPositionToPosition.Map(dbPosition);

            var expected = new Position
            {
                Name = dbPosition.Name,
                Description = dbPosition.Description,
                IsActive = dbPosition.IsActive,
                UserIds = dbPosition.UserIds?.Select(x => x.UserId).ToList()
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion

        #region EditPositionRequest to DbPosition
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullEditPositionRequestToDbPosition()
        {
            Assert.Throws<ArgumentNullException>(() => mapperEditPositionRequestToDbPosition.Map(null));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfullyEditPositionRequestToDbPosition()
        {
            var result = mapperEditPositionRequestToDbPosition.Map(editPositionRequest);

            var expected = new DbPosition
            {
                Id = result.Id,
                Name = editPositionRequest.Name,
                Description = editPositionRequest.Description
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion
    }
}