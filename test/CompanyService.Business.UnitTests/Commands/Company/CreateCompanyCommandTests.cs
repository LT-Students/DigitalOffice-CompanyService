//using FluentValidation;
//using LT.DigitalOffice.CompanyService.Business.Commands.Company;
//using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
//using LT.DigitalOffice.CompanyService.Data.Interfaces;
//using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
//using LT.DigitalOffice.CompanyService.Models.Db;
//using LT.DigitalOffice.CompanyService.Models.Dto.Models;
//using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
//using LT.DigitalOffice.CompanyService.Validation.Interfaces;
//using LT.DigitalOffice.Kernel.Broker;
//using LT.DigitalOffice.Kernel.Exceptions.Models;
//using LT.DigitalOffice.Kernel.Responses;
//using LT.DigitalOffice.Models.Broker.Requests.Message;
//using LT.DigitalOffice.Models.Broker.Requests.User;
//using LT.DigitalOffice.UnitTestKernel;
//using MassTransit;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Company
//{
//    public class CreateCompanyCommandTests
//    {
//        private Mock<IDbCompanyMapper> _mapperMock;
//        private Mock<IRequestClient<ICreateSMTPRequest>> _rcCreateSMTPMock;
//        private Mock<IRequestClient<ICreateAdminRequest>> _rcCreateAdminMock;
//        private Mock<ILogger<ICreateCompanyCommand>> _loggerMock;
//        private Mock<ICreateCompanyRequestValidator> _validatorMock;
//        private Mock<ICompanyRepository> _repositoryMock;
//        private ICreateCompanyCommand _command;

//        private CreateCompanyRequest _request;
//        private DbCompany _company;
//        private Guid _authorId;

//        [OneTimeSetUp]
//        public void OneTimeSetUp()
//        {
//            _mapperMock = new();
//            _rcCreateSMTPMock = new();
//            _rcCreateAdminMock = new();
//            _loggerMock = new();
//            _validatorMock = new();
//            _repositoryMock = new();
//            _command = new CreateCompanyCommand(
//                _mapperMock.Object,
//                _loggerMock.Object,
//                _validatorMock.Object,
//                _repositoryMock.Object,
//                _rcCreateSMTPMock.Object,
//                _rcCreateAdminMock.Object);

//            _request = new()
//            {
//                PortalName = "PortalName",
//                CompanyName = "Name",
//                SiteUrl = "siteurl",
//                AdminInfo = new AdminInfo
//                {
//                    FirstName = "Name",
//                    MiddleName = null,
//                    LastName = "LastName",
//                    Email = "email@email.com",
//                    Login = "MyLogin",
//                    Password = "MyPassword"
//                },
//                Smtp = new()
//                {
//                    Host = "host",
//                    Port = 123,
//                    Email = "email@email.ru",
//                    EnableSsl = true,
//                    Password = "password"
//                }
//            };

//            _company = new DbCompany
//            {
//                Id = Guid.NewGuid(),
//                CreatedAt = DateTime.UtcNow,
//                IsActive = true,
//                PortalName = _request.PortalName,
//                CompanyName = _request.CompanyName,
//                Description = null,
//                LogoId = null,
//                Tagline = null,
//                SiteUrl = _request.SiteUrl
//            };
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            _mapperMock.Reset();
//            _rcCreateAdminMock.Reset();
//            _rcCreateSMTPMock.Reset();
//            _validatorMock.Reset();
//            _repositoryMock.Reset();
//            _loggerMock.Reset();

//            _authorId = Guid.NewGuid();

//            _validatorMock
//                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
//                .Returns(true);
//        }

//        [Test]
//        public void ShouldThrowValidationException()
//        {
//            _validatorMock
//                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
//                .Returns(false);

//            Assert.Throws<ValidationException>(() => _command.Execute(_request));
//            _repositoryMock.Verify(x => x.Add(It.IsAny<DbCompany>()), Times.Never);
//            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
//            _mapperMock.Verify(x => x.Map(It.IsAny<CreateCompanyRequest>()), Times.Never);
//            _rcCreateSMTPMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Never);
//            _rcCreateAdminMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Never);
//        }

//        [Test]
//        public void ShouldReturnFailedResponseWhenCreateSMTPResponseIsNotSuccessfuly()
//        {
//            var response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(false);

//            response
//                .Setup(x => x.Errors)
//                .Returns(new List<string> { "some error" });

//            var brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateSMTPMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            var expected = new OperationResultResponse<Guid>
//            {
//                Status = Kernel.Enums.OperationResultStatusType.Failed,
//                Errors = new List<string> { "Can not create smtp." }
//            };

//            SerializerAssert.AreEqual(expected, _command.Execute(_request));
//            _repositoryMock.Verify(x => x.Add(_company), Times.Never);
//            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
//            _mapperMock.Verify(x => x.Map(_request), Times.Never);
//            _rcCreateSMTPMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//            _rcCreateAdminMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Never);
//        }

//        [Test]
//        public void ShouldReturnFailedResponseWhenCreateAdminResponseIsNotSuccessfuly()
//        {
//            var response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(true);

//            response
//                .Setup(x => x.Body)
//                .Returns(true);

//            var brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateSMTPMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(false);

//            response
//                .Setup(x => x.Errors)
//                .Returns(new List<string> { "some error" });

//            brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateAdminMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            var expected = new OperationResultResponse<Guid>
//            {
//                Status = Kernel.Enums.OperationResultStatusType.Failed,
//                Errors = new List<string> { "Can not create admin." }
//            };

//            SerializerAssert.AreEqual(expected, _command.Execute(_request));
//            _repositoryMock.Verify(x => x.Add(_company), Times.Never);
//            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
//            _mapperMock.Verify(x => x.Map(_request), Times.Never);
//            _rcCreateSMTPMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//            _rcCreateAdminMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//        }

//        [Test]
//        public void ShouldThrowExceptionWhenRepositoryThrow()
//        {
//            var response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(true);

//            response
//                .Setup(x => x.Body)
//                .Returns(true);

//            var brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateSMTPMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(true);

//            response
//                .Setup(x => x.Body)
//                .Returns(true);

//            brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateAdminMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            _mapperMock
//                .Setup(x => x.Map(_request))
//                .Returns(_company);

//            _repositoryMock
//                .Setup(x => x.Add(_company))
//                .Throws(new Exception());

//            Assert.Throws<Exception>(() => _command.Execute(_request));
//            _repositoryMock.Verify(x => x.Add(_company), Times.Once);
//            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
//            _mapperMock.Verify(x => x.Map(_request), Times.Once);
//            _rcCreateSMTPMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//            _rcCreateAdminMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//        }

//        [Test]
//        public void ShouldCreateCompanySuccessfuly()
//        {
//            var response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(true);

//            response
//                .Setup(x => x.Body)
//                .Returns(true);

//            var brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateSMTPMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            response = new Mock<IOperationResult<bool>>();
//            response
//                .Setup(x => x.IsSuccess)
//                .Returns(true);

//            response
//                .Setup(x => x.Body)
//                .Returns(true);

//            brokerResponseMock = new Mock<Response<IOperationResult<bool>>>();
//            brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(response.Object);

//            _rcCreateAdminMock
//               .Setup(x => x.GetResponse<IOperationResult<bool>>(
//                       It.IsAny<object>(), default, default))
//               .Returns(Task.FromResult(brokerResponseMock.Object));

//            _mapperMock
//                .Setup(x => x.Map(_request))
//                .Returns(_company);

//            var expected = new OperationResultResponse<Guid>
//            {
//                Body = _company.Id,
//                Status = Kernel.Enums.OperationResultStatusType.FullSuccess
//            };

//            SerializerAssert.AreEqual(expected, _command.Execute(_request));
//            _repositoryMock.Verify(x => x.Add(_company), Times.Once);
//            _validatorMock.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
//            _mapperMock.Verify(x => x.Map(_request), Times.Once);
//            _rcCreateSMTPMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//            _rcCreateAdminMock.Verify(x => x.GetResponse<IOperationResult<bool>>(
//               It.IsAny<object>(), default, default).Result.Message, Times.Once);
//        }
//    }
//}
