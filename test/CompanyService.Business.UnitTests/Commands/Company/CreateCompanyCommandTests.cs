using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Commands.Company;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Company
{
    public class CreateCompanyCommandTests
    {
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<IDbCompanyMapper> _mapperMock;
        private Mock<IRequestClient<IAddImageRequest>> _rcAddImageMock;
        private Mock<ILogger<CreateCompanyCommand>> _loggerMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ICreateCompanyRequestValidator> _validatorMock;
        private Mock<ICompanyRepository> _repositoryMock;
        private ICreateCompanyCommand _command;

        private CreateCompanyRequest _request;
        private DbCompany _company;
        private Guid _authorId;
        private Guid _imageId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _accessValidatorMock = new();
            _mapperMock = new();
            _rcAddImageMock = new();
            _loggerMock = new();
            _httpContextAccessorMock = new();
            _validatorMock = new();
            _repositoryMock = new();
            _command = new CreateCompanyCommand(
                _accessValidatorMock.Object,
                _mapperMock.Object,
                _rcAddImageMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object,
                _validatorMock.Object,
                _repositoryMock.Object);

            _request = new()
            {
                Name = "Name",
                Description = "Description",
                Logo = new AddImageRequest
                {
                    Name = "Name",
                    Content = "Content",
                    Extension = "Extension"
                },
                Tagline = "tagline",
                SiteUrl = "siteurl"
            };

            _company = new DbCompany
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Name = _request.Name,
                Description = _request.Description,
                LogoId = _imageId,
                Tagline = _request.Tagline,
                SiteUrl = _request.SiteUrl
            };
        }

        [SetUp]
        public void SetUp()
        {
            _accessValidatorMock.Reset();
            _mapperMock.Reset();
            _rcAddImageMock.Reset();
            _httpContextAccessorMock.Reset();
            _validatorMock.Reset();
            _repositoryMock.Reset();
            _loggerMock.Reset();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _authorId = Guid.NewGuid();
            _imageId = Guid.NewGuid();

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _authorId);
            _httpContextAccessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(_items);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);
        }

        [Test]
        public void ShouldThrowForbiddenException()
        {
            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _repositoryMock.Verify(x => x.Add(It.IsAny<DbCompany>()), Times.Never);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Never);
            _mapperMock.Verify(x => x.Map(It.IsAny<CreateCompanyRequest>(), It.IsAny<Guid?>()), Times.Never);
            _rcAddImageMock.Verify( x => x.GetResponse<IOperationResult<IAddImageResponse>>(
                It.IsAny<object>(), default, default).Result.Message, Times.Never);
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_request));
            _repositoryMock.Verify(x => x.Add(It.IsAny<DbCompany>()), Times.Never);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(It.IsAny<CreateCompanyRequest>(), It.IsAny<Guid?>()), Times.Never);
            _rcAddImageMock.Verify(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            var response = new Mock<IOperationResult<IAddImageResponse>>();
            response
                .Setup(x => x.IsSuccess)
                .Returns(false);

            response
                .Setup(x => x.Errors)
                .Returns(new List<string> { "some error" });

            var brokerResponseMock = new Mock<Response<IOperationResult<IAddImageResponse>>>();
            brokerResponseMock
                .Setup(x => x.Message)
                .Returns(response.Object);

            _rcAddImageMock
               .Setup(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(brokerResponseMock.Object));

            _mapperMock
                .Setup(x => x.Map(_request, It.IsAny<Guid?>()))
                .Returns(_company);

            _repositoryMock
                .Setup(x => x.Add(_company))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _repositoryMock.Verify(x => x.Add(_company), Times.Once);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(_request, null), Times.Once);
            _rcAddImageMock.Verify(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Once);
        }

        [Test]
        public void ShouldCreateCompanySuccessfuly()
        {
            var response = new Mock<IOperationResult<IAddImageResponse>>();
            response
                .Setup(x => x.IsSuccess)
                .Returns(true);

            response
                .Setup(x => x.Body.Id)
                .Returns(_imageId);

            var brokerResponseMock = new Mock<Response<IOperationResult<IAddImageResponse>>>();
            brokerResponseMock
                .Setup(x => x.Message)
                .Returns(response.Object);

            _rcAddImageMock
               .Setup(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(brokerResponseMock.Object));

            _mapperMock
                .Setup(x => x.Map(_request, _imageId))
                .Returns(_company);

            var expected = new OperationResultResponse<Guid>
            {
                Body = _company.Id,
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));
            _repositoryMock.Verify(x => x.Add(_company), Times.Once);
            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
            _mapperMock.Verify(x => x.Map(_request, _imageId), Times.Once);
            _rcAddImageMock.Verify(x => x.GetResponse<IOperationResult<IAddImageResponse>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Once);
        }
    }
}
