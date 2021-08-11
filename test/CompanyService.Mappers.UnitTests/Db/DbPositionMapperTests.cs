using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class PositionMappersTests
    {
        private IDbPositionMapper _mapper;
        private AutoMocker _mocker;

        private PositionInfo _positionInfo;
        private CreatePositionRequest _createPositionRequest;
        private DbPosition _expectedDbPosition;
        private Guid _companyId;
        private Guid _userId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _companyId = Guid.NewGuid();
            _userId = Guid.NewGuid();

            _mocker = new();
            _mapper = _mocker.CreateInstance<DbPositionMapper>();

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _mocker
                .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
                .Returns(_items);
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
                CreatedBy = _userId,
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

        [Test]
        public void ShouldMapAddPositionRequestToDbPositionSuccessfully()
        {
            var resultDbPosition = _mapper.Map(_createPositionRequest, _companyId);

            _expectedDbPosition.Id = resultDbPosition.Id;
            _expectedDbPosition.CreatedAtUtc = resultDbPosition.CreatedAtUtc;

            Assert.IsInstanceOf<Guid>(resultDbPosition.Id);
            SerializerAssert.AreEqual(_expectedDbPosition, resultDbPosition);
        }
        #endregion
    }
}