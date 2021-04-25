using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
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

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class PositionMappersTests
    {
        private IDbPositionMapper _mapper;

        private PositionInfo _positionInfo;
        private CreatePositionRequest _createPositionRequest;
        private DbPosition _expectedDbPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new DbPositionMapper();
        }

        [SetUp]
        public void SetUp()
        {
            _positionInfo = new PositionInfo
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description",
                IsActive = true
            };

            _createPositionRequest = new CreatePositionRequest
            {
                Name = "Name",
                Description = "Description",
            };
            _expectedDbPosition = new DbPosition
            {
                Name = _createPositionRequest.Name,
                Description = _createPositionRequest.Description,
                IsActive = true
            };
        }

        #region CreatePositionRequest to DbPosition
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullAddPositionRequestToDbPosition()
        {
            CreatePositionRequest createPositionRequest = null;
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(createPositionRequest));
        }

        public void ShouldMapAddPositionRequestToDbPositionSuccessfully()
        {
            var resultDbPosition = _mapper.Map(_createPositionRequest);

            _expectedDbPosition.Id = resultDbPosition.Id;

            Assert.IsInstanceOf<Guid>(resultDbPosition.Id);
            SerializerAssert.AreEqual(_expectedDbPosition, resultDbPosition);
        }
        #endregion

        #region PositionInfo to DbPosition
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullEditPositionRequestToDbPosition()
        {
            PositionInfo positionInfo = null;
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(positionInfo));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfullyEditPositionRequestToDbPosition()
        {
            var result = _mapper.Map(_positionInfo);

            var expected = new DbPosition
            {
                Id = result.Id,
                Name = _positionInfo.Name,
                Description = _positionInfo.Description,
                IsActive = true
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion
    }
}