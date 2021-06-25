using LT.DigitalOffice.CompanyService.Business.Commands.Office;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Office
{
    public class CreateOfficeCommandTests
    {
        private AutoMocker _mocker;
        private ICreateOfficeCommand _command;

        private CreateOfficeRequest _request;
        private DbOffice _office;

        [SetUp]
        public void SetUp()
        {
            _request = new CreateOfficeRequest
            {
                Name = "name",
                Address = "address",
                City = "City",
                CompanyId = Guid.NewGuid()
            };

            _office = new DbOffice
            {
                Id = Guid.NewGuid(),
                Address = _request.Address,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                City = _request.City,
                CompanyId = _request.CompanyId,
                Name = _request.Name
            };

            _mocker = new();
            _command = _mocker.CreateInstance<CreateOfficeCommand>();

            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(true);
        }

        [Test]
        public void ShouldThrowForbiddenException()
        {
            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _mocker.Verify<IOfficeRepository>(x => x.Add(It.IsAny<DbOffice>()), Times.Never);
            _mocker.Verify<ICreateOfficeRequestValidator>(x => x.Validate(It.IsAny<CreateOfficeRequest>()), Times.Never);
            _mocker.Verify<IDbOfficeMapper, DbOffice>(x => x.Map(It.IsAny<CreateOfficeRequest>()), Times.Never);
        }

        //[Test]
        //public void ShouldThrowValidationException()
        //{
        //    _mocker
        //        .Setup<ICreateOfficeRequestValidator, bool>(x => x.Validate(_request).IsValid)
        //        .Returns(false);

        //    Assert.Throws<ValidationException>(() => _command.Execute(_request));
        //    _mocker.Verify<IOfficeRepository>(x => x.Add(It.IsAny<DbOffice>()), Times.Never);
        //    _mocker.Verify<ICreateOfficeRequestValidator>(x => x.Validate(It.IsAny<CreateOfficeRequest>()), Times.Once);
        //    _mocker.Verify<IDbOfficeMapper, DbOffice>(x => x.Map(It.IsAny<CreateOfficeRequest>()), Times.Never);
        //}

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<ICreateOfficeRequestValidator, bool>(x => x.Validate(_request).IsValid)
                .Returns(true);

            _mocker
                .Setup<IDbOfficeMapper, DbOffice>(x => x.Map(_request))
                .Returns(_office);

            _mocker
                .Setup<IOfficeRepository>(x => x.Add(_office))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _mocker.Verify<IOfficeRepository>(x => x.Add(It.IsAny<DbOffice>()), Times.Once);
            //_mocker.Verify<ICreateOfficeRequestValidator>(x => x.Validate(_request), Times.Once);
            _mocker.Verify<IDbOfficeMapper, DbOffice>(x => x.Map(It.IsAny<CreateOfficeRequest>()), Times.Once);
        }

        [Test]
        public void ShouldAddOfficeSuccessfuly()
        {
            _mocker
                .Setup<ICreateOfficeRequestValidator, bool>(x => x.Validate(_request).IsValid)
                .Returns(true);

            _mocker
                .Setup<IDbOfficeMapper, DbOffice>(x => x.Map(_request))
                .Returns(_office);

            var expected = new OperationResultResponse<Guid>
            {
                Body = _office.Id,
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));
            _mocker.Verify<IOfficeRepository>(x => x.Add(It.IsAny<DbOffice>()), Times.Once);
            //_mocker.Verify<ICreateOfficeRequestValidator>(x => x.Validate(It.IsAny<CreateOfficeRequest>()), Times.Once);
            _mocker.Verify<IDbOfficeMapper, DbOffice>(x => x.Map(It.IsAny<CreateOfficeRequest>()), Times.Once);
        }
    }
}
