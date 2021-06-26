using FluentValidation;
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
using NUnit.Framework;
using System;
namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Office
{
    public class CreateOfficeCommandTests
    {
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<IOfficeRepository> _repositoryMock;
        private Mock<IDbOfficeMapper> _mapperMock;
        private Mock<ICreateOfficeRequestValidator> _validatorMock;
        private ICreateOfficeCommand _command;

        private CreateOfficeRequest _request;
        private DbOffice _office;

        [OneTimeSetUp]
        public void OneTimeSetUp()
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
        }

        [SetUp]
        public void SetUp()
        {
            _accessValidatorMock = new();
            _mapperMock = new();
            _repositoryMock = new();
            _validatorMock = new();

            _command = new CreateOfficeCommand(
                _accessValidatorMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object,
                _validatorMock.Object);

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(_request).IsValid)
                .Returns(true);
        }

        [Test]
        public void ShouldThrowForbiddenException()
        {
            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(It.IsAny<DbOffice>()), Times.Never);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Never);
            _mapperMock.Verify(x => x.Map(It.IsAny<CreateOfficeRequest>()), Times.Never);
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_request));
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(It.IsAny<DbOffice>()), Times.Never);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(It.IsAny<CreateOfficeRequest>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mapperMock
                .Setup(x => x.Map(_request))
                .Returns(_office);

            _repositoryMock
                .Setup(x => x.Add(_office))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(_office), Times.Once);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(_request), Times.Once);
        }

        [Test]
        public void ShouldAddOfficeSuccessfuly()
        {
            _mapperMock
                .Setup(x => x.Map(_request))
                .Returns(_office);

            var expected = new OperationResultResponse<Guid>
            {
                Body = _office.Id,
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(_office), Times.Once);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(_request), Times.Once);
        }
    }
}
