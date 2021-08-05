using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Commands.Company;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Requests.User;
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
        private AutoMocker _autoMock;
        private ICreateCompanyCommand _command;

        private CreateCompanyRequest _request;
        private DbCompany _company;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoMock = new AutoMocker();

            _command = new CreateCompanyCommand(
                _autoMock.GetMock<IDbCompanyMapper>().Object,
                _autoMock.GetMock<ILogger<ICreateCompanyCommand>>().Object,
                _autoMock.GetMock<ICreateCompanyRequestValidator>().Object,
                _autoMock.GetMock<ICompanyRepository>().Object,
                _autoMock.GetMock<IRequestClient<ICreateAdminRequest>>().Object,
                _autoMock.GetMock<IRequestClient<IUpdateSmtpCredentialsRequest>>().Object,
                _autoMock.GetMock<ICompanyChangesRepository>().Object,
                _autoMock.GetMock<IHttpContextAccessor>().Object);

            _request = new()
            {
                PortalName = "PortalName",
                CompanyName = "Name",
                SiteUrl = "siteurl",
                IsDepartmentModuleEnabled = true,
                AdminInfo = new AdminInfo
                {
                    FirstName = "Name",
                    MiddleName = null,
                    LastName = "LastName",
                    Email = "email@email.com",
                    Login = "MyLogin",
                    Password = "MyPassword"
                },
                SmtpInfo = new()
                {
                    Host = "host",
                    Port = 123,
                    Email = "email@email.ru",
                    EnableSsl = true,
                    Password = "password"
                }
            };

            _company = new DbCompany
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDepartmentModuleEnabled = _request.IsDepartmentModuleEnabled,
                PortalName = _request.PortalName,
                CompanyName = _request.CompanyName,
                Description = null,
                LogoId = null,
                Tagline = null,
                SiteUrl = _request.SiteUrl
            };
        }

        [SetUp]
        public void SetUp()
        {
            _autoMock.GetMock<IDbCompanyMapper>().Reset();
            _autoMock.GetMock<ILogger<ICreateCompanyCommand>>().Reset();
            _autoMock.GetMock<ICreateCompanyRequestValidator>().Reset();
            _autoMock.GetMock<ICompanyRepository>().Reset();
            _autoMock.GetMock<IRequestClient<ICreateAdminRequest>>().Reset();
            _autoMock.GetMock<IRequestClient<IUpdateSmtpCredentialsRequest>>().Reset();
            _autoMock.GetMock<ICompanyChangesRepository>().Reset();
            _autoMock.GetMock<IHttpContextAccessor>().Reset();

            _autoMock
                .Setup<ICreateCompanyRequestValidator, bool>(
                x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _autoMock
               .Setup<ICreateCompanyRequestValidator, bool>(
               x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
               .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
               x => x.Add(It.IsAny<DbCompany>()),
               Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
               x => x.Validate(It.IsAny<IValidationContext>()),
               Times.Once);

            _autoMock.Verify<IDbCompanyMapper>(
               x => x.Map(It.IsAny<CreateCompanyRequest>()),
               Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
            It.IsAny<object>(), default, default).Result.Message, Times.Never);

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Never);
        }

        [Test]
        public void ShouldThrowBadRequestException()
        {
            _autoMock
               .Setup<ICompanyRepository, DbCompany>(x => x.Get(null))
               .Returns(_company);

            Assert.Throws<BadRequestException>(() => _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
               x => x.Add(It.IsAny<DbCompany>()),
               Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
               x => x.Validate(It.IsAny<IValidationContext>()),
               Times.Never);

            _autoMock.Verify<IDbCompanyMapper>(
               x => x.Map(It.IsAny<CreateCompanyRequest>()),
               Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
            It.IsAny<object>(), default, default).Result.Message, Times.Never);

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Never);
        }

        [Test]
        public void ShouldReturnFailedResponseWhenUpdateSMTPResponseIsNotSuccessfuly()
        {
            _autoMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(false);

            _autoMock
                .Setup<IOperationResult<bool>, List<string>>(x => x.Errors)
                .Returns(new List<string> { "some error" });

            _autoMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(_autoMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
               .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(_autoMock.GetMock<Response<IOperationResult<bool>>>().Object));

            var expected = new OperationResultResponse<Guid>
            {
                Status = Kernel.Enums.OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not update smtp credentials." }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
               x => x.Add(It.IsAny<DbCompany>()),
               Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
               x => x.Validate(It.IsAny<IValidationContext>()),
               Times.Once);

            _autoMock.Verify<IDbCompanyMapper>(
               x => x.Map(It.IsAny<CreateCompanyRequest>()),
               Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
            It.IsAny<object>(), default, default).Result.Message, Times.Once);

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Never);
        }

        [Test]
        public void ShouldReturnFailedResponseWhenCreateAdminResponseIsNotSuccessfuly()
        {
            AutoMocker firstMock = new();
            firstMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            firstMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            firstMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(firstMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
               .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(firstMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker secondAutoMock = new();
            secondAutoMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(false);

            secondAutoMock
                .Setup<IOperationResult<bool>, List<string>>(x => x.Errors)
                .Returns(new List<string> { "some error" });

            secondAutoMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondAutoMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(secondAutoMock.GetMock<Response<IOperationResult<bool>>>().Object));

            var expected = new OperationResultResponse<Guid>
            {
                Status = Kernel.Enums.OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not create admin." }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));
            _autoMock.Verify<ICompanyRepository>(
               x => x.Add(_company), Times.Never);

            _autoMock.Verify<ICreateCompanyRequestValidator>(
               x => x.Validate(It.IsAny<IValidationContext>()),
               Times.Once);

            _autoMock.Verify<IDbCompanyMapper>(
                 x => x.Map(_request),
                 Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
            It.IsAny<object>(), default, default).Result.Message, Times.Once);

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            AutoMocker firstMock = new();
            firstMock
               .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
               .Returns(true);

            firstMock
               .Setup<IOperationResult<bool>, bool>(x => x.Body)
               .Returns(true);

            firstMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(firstMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
               .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(firstMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker secondAutoMock = new();
            secondAutoMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            secondAutoMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            secondAutoMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondAutoMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(secondAutoMock.GetMock<Response<IOperationResult<bool>>>().Object));

            _autoMock.Setup<IDbCompanyMapper, DbCompany>(x => x.Map(_request))
                .Returns(_company);

            _autoMock.Setup<ICompanyRepository>(x => x.Add(_company))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
               x => x.Add(_company),
               Times.Once);

            _autoMock.Verify<ICreateCompanyRequestValidator>(
               x => x.Validate(It.IsAny<IValidationContext>()),
               Times.Once);

            _autoMock.Verify<IDbCompanyMapper>(
                 x => x.Map(_request),
                 Times.Once());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
            It.IsAny<object>(), default, default).Result.Message, Times.Once);

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Once);
        }

        [Test]
        public void ShouldCreateCompanySuccessfuly()
        {
            AutoMocker firstMock = new();
            firstMock
               .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
               .Returns(true);

            firstMock
               .Setup<IOperationResult<bool>, bool>(x => x.Body)
               .Returns(true);

            firstMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(firstMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
               .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(firstMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker secondAutoMock = new();
            secondAutoMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            secondAutoMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            secondAutoMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondAutoMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                x => x.GetResponse<IOperationResult<bool>>(
                       It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(secondAutoMock.GetMock<Response<IOperationResult<bool>>>().Object));

            _autoMock
               .Setup<IDbCompanyMapper, DbCompany>(x => x.Map(_request))
               .Returns(_company);

            var expected = new OperationResultResponse<Guid>
            {
                Body = _company.Id,
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
               x => x.Add(_company), Times.Once);

            _autoMock.Verify<ICreateCompanyRequestValidator>(
               x => x.Validate(It.IsAny<IValidationContext>()),
               Times.Once);

            _autoMock.Verify<IDbCompanyMapper>(
                 x => x.Map(_request),
                 Times.Once());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
            It.IsAny<object>(), default, default).Result.Message, Times.Once);

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
               x => x.GetResponse<IOperationResult<bool>>(
               It.IsAny<object>(), default, default).Result.Message, Times.Once);
        }
    }
}