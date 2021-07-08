using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class PositionMappersTests
    {
        private IDbPositionMapper _mapper;

        private PositionInfo _positionInfo;
        private CreatePositionRequest _createPositionRequest;
        private DbPosition _expectedDbPosition;
        private Guid _companyId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _companyId = Guid.NewGuid();

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
                CompanyId = _companyId,
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
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(createPositionRequest, _companyId));
        }

        public void ShouldMapAddPositionRequestToDbPositionSuccessfully()
        {
            var resultDbPosition = _mapper.Map(_createPositionRequest, _companyId);

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
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(positionInfo, _companyId));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfullyEditPositionRequestToDbPosition()
        {
            var result = _mapper.Map(_positionInfo, _companyId);

            var expected = new DbPosition
            {
                Id = result.Id,
                CompanyId = _companyId,
                Name = _positionInfo.Name,
                Description = _positionInfo.Description,
                IsActive = true
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion
    }
}