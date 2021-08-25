using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Commands.Office;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Enums;
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
        private Mock<ICompanyRepository> _companyRepositoryMock;
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
                City = "City"
            };

            _office = new DbOffice
            {
                Id = Guid.NewGuid(),
                Address = _request.Address,
                CreatedAtUtc = DateTime.UtcNow,
                IsActive = true,
                City = _request.City,
                CompanyId = Guid.NewGuid(),
                Name = _request.Name
            };
        }

        [SetUp]
        public void SetUp()
        {
            _accessValidatorMock = new();
            _mapperMock = new();
            _repositoryMock = new();
            _companyRepositoryMock = new();
            _validatorMock = new();

            _command = new CreateOfficeCommand(
                _accessValidatorMock.Object,
                _repositoryMock.Object,
                _companyRepositoryMock.Object,
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
            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Never);
            _mapperMock.Verify(x => x.Map(It.IsAny<CreateOfficeRequest>(), It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void ShouldReturnResponseWithTypeBadRequestWhenIncorrectRequest()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            var response = _command.Execute(_request);
            Assert.AreEqual(OperationResultStatusType.BadRequest, response.Status);
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(It.IsAny<DbOffice>()), Times.Never);
            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(It.IsAny<CreateOfficeRequest>(), It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mapperMock
                .Setup(x => x.Map(_request, _office.CompanyId))
                .Returns(_office);

            _companyRepositoryMock
                .Setup(x => x.Get(null))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(_office), Times.Never);
            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(_request, _office.CompanyId), Times.Never);
        }

        [Test]
        public void ShouldAddOfficeSuccessfuly()
        {
            _mapperMock
                .Setup(x => x.Map(_request, _office.CompanyId))
                .Returns(_office);

            var expected = new OperationResultResponse<Guid>
            {
                Body = _office.Id,
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess
            };

            _companyRepositoryMock
                .Setup(x => x.Get(null))
                .Returns(new DbCompany { Id = _office.CompanyId });

            SerializerAssert.AreEqual(expected, _command.Execute(_request));
            _accessValidatorMock.Verify(x => x.IsAdmin(null), Times.Once);
            _repositoryMock.Verify(x => x.Add(_office), Times.Once);
            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(_request, _office.CompanyId), Times.Once);
        }
    }
}
