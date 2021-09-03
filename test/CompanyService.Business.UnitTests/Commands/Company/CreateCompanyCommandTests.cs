using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Commands.Company;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Microsoft.AspNetCore.Http;
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
            _autoMock = new();

            _autoMock
                .Setup<IHttpContextAccessor, int>(a => a.HttpContext.Response.StatusCode)
                .Returns(200);

            _command = _autoMock.CreateInstance<CreateCompanyCommand>();

            _request = new()
            {
                PortalName = "PortalName",
                CompanyName = "Name",
                SiteUrl = "siteurl",
                IsDepartmentModuleEnabled = true,
                AdminInfo = new()
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

            _company = new()
            {
                Id = Guid.NewGuid(),
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
            _autoMock.GetMock<ICreateCompanyRequestValidator>().Reset();
            _autoMock.GetMock<ICompanyRepository>().Reset();
            _autoMock.GetMock<IRequestClient<ICreateAdminRequest>>().Reset();
            _autoMock.GetMock<IRequestClient<IUpdateSmtpCredentialsRequest>>().Reset();

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

            var response = _command.Execute(_request);

            Assert.AreEqual(OperationResultStatusType.Failed, response.Status);

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(It.IsAny<DbCompany>()),
                Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Never());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Never());
        }

        [Test]
        public void ShouldReturnFaliledResponseWhenCompanyAlreadyExists()
        {
            _autoMock
                .Setup<ICompanyRepository, DbCompany>(x => x.Get(null))
                .Returns(_company);

            var response = _command.Execute(_request);

            Assert.AreEqual(OperationResultStatusType.Failed, response.Status);
            SerializerAssert.AreEqual(response.Errors, new List<string> { "Company already exists" });

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(It.IsAny<DbCompany>()),
                Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Never());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Never());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Never());
        }

        [Test]
        public void ShouldReturnFailedResponseWhenUpdateSMTPResponseThrowException()
        {
            _autoMock
                .Setup<IOperationResult<bool>, List<string>>(x => x.Errors)
                .Returns(new List<string> { "Can not update smtp credentials." });

            _autoMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(_autoMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Throws(new Exception());

            OperationResultResponse<Guid> expected = new()
            {
                Status = OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not update smtp credentials." }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(It.IsAny<DbCompany>()),
                Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Never());
        }

        [Test]
        public void ShouldReturnFailedResponseWhenUpdateSMTPResponseIsNotSuccessfuly()
        {
            _autoMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(false);

            _autoMock
                .Setup<IOperationResult<bool>, List<string>>(x => x.Errors)
                .Returns(new List<string> { "Can not update smtp credentials." });

            _autoMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(_autoMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(_autoMock.GetMock<Response<IOperationResult<bool>>>().Object));

            OperationResultResponse<Guid> expected = new()
            {
                Status = OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not update smtp credentials." }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(It.IsAny<DbCompany>()),
                Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Never());
        }

        [Test]
        public void ShouldReturnFailedResponseWhenAdminResponseThrowException()
        {
            AutoMocker secondMock = new();
            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            secondMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(secondMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker thirdMock = new();

            thirdMock
                .Setup<IOperationResult<bool>, List<string>>(x => x.Errors)
                .Returns(new List<string> { "Can not create admin." });

            thirdMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(thirdMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Throws(new Exception());

            OperationResultResponse<Guid> expected = new()
            {
                Status = OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not create admin." }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(_company), Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());
        }

        [Test]
        public void ShouldReturnFailedResponseWhenCreateAdminResponseIsNotSuccessfuly()
        {
            AutoMocker secondMock = new();
            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            secondMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(secondMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker thirdMock = new();
            thirdMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(false);

            thirdMock
                .Setup<IOperationResult<bool>, List<string>>(x => x.Errors)
                .Returns(new List<string> { "Can not create admin." });

            thirdMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(thirdMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(thirdMock.GetMock<Response<IOperationResult<bool>>>().Object));

            OperationResultResponse<Guid> expected = new()
            {
                Status = OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not create admin." }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(_company), Times.Never());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Never());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            AutoMocker secondMock = new();
            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            secondMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(secondMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker thirdMock = new();
            thirdMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            thirdMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            thirdMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(thirdMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(thirdMock.GetMock<Response<IOperationResult<bool>>>().Object));

            _autoMock.Setup<IDbCompanyMapper, DbCompany>(x => x.Map(_request))
                .Returns(_company);

            _autoMock.Setup<ICompanyRepository>(x => x.Add(_company))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(_company),
                Times.Once());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Once());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());
        }

        [Test]
        public void ShouldCreateCompanySuccessfuly()
        {
            AutoMocker secondMock = new();
            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            secondMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            secondMock
                .Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(secondMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<IUpdateSmtpCredentialsRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(secondMock.GetMock<Response<IOperationResult<bool>>>().Object));

            AutoMocker thirdMock = new();
            thirdMock
                .Setup<IOperationResult<bool>, bool>(x => x.IsSuccess)
                .Returns(true);

            thirdMock
                .Setup<IOperationResult<bool>, bool>(x => x.Body)
                .Returns(true);

            thirdMock.Setup<Response<IOperationResult<bool>>, IOperationResult<bool>>(x => x.Message)
                .Returns(thirdMock.GetMock<IOperationResult<bool>>().Object);

            _autoMock
                .Setup<IRequestClient<ICreateAdminRequest>, Task<Response<IOperationResult<bool>>>>(
                    x => x.GetResponse<IOperationResult<bool>>(
                        It.IsAny<object>(), default, default))
                .Returns(Task.FromResult(thirdMock.GetMock<Response<IOperationResult<bool>>>().Object));

            _autoMock
                .Setup<IDbCompanyMapper, DbCompany>(x => x.Map(_request))
                .Returns(_company);

            OperationResultResponse<Guid> expected = new()
            {
                Body = _company.Id,
                Status = OperationResultStatusType.FullSuccess
            };

            SerializerAssert.AreEqual(expected, _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository>(
                x => x.Get(null), Times.Once());

            _autoMock.Verify<ICompanyRepository>(
                x => x.Add(_company), Times.Once());

            _autoMock.Verify<ICreateCompanyRequestValidator>(
                x => x.Validate(It.IsAny<IValidationContext>()),
                Times.Once());

            _autoMock.Verify<IDbCompanyMapper>(
                x => x.Map(_request),
                Times.Once());

            _autoMock.Verify<IRequestClient<IUpdateSmtpCredentialsRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());

            _autoMock.Verify<IRequestClient<ICreateAdminRequest>>(
                x => x.GetResponse<IOperationResult<bool>>(
                    It.IsAny<object>(), default, default), Times.Once());
        }
    }
}
