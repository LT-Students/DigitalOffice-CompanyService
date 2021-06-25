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
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Company
{
    public class CreateCompanyCommandTests
    {
        private AutoMocker _mocker;
        private ICreateCompanyCommand _command;

        private CreateCompanyRequest _request;
        private DbCompany _company;
        private Guid _authorId;
        private Guid _imageId;

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _command = _mocker.CreateInstance<CreateCompanyCommand>();

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
                Tagline = "tagline"
            };

            _company = new DbCompany
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Name = _request.Name,
                Description = _request.Description,
                LogoId = _imageId,
                Tagline = _request.Tagline
            };

            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(true);

            _authorId = Guid.NewGuid();
            _imageId = Guid.NewGuid();

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _authorId);
            _mocker
                .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
                .Returns(_items);
        }

        [Test]
        public void ShouldThrowForbiddenException()
        {
            _mocker
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _mocker.Verify<ICompanyRepository>(x => x.Add(It.IsAny<DbCompany>()), Times.Never);
            _mocker.Verify<ICreateCompanyRequestValidator>(x => x.Validate(It.IsAny<CreateCompanyRequest>()), Times.Never);
            _mocker.Verify<IDbCompanyMapper, DbCompany>(x => x.Map(It.IsAny<CreateCompanyRequest>(), It.IsAny<Guid?>()), Times.Never);
            _mocker.Verify<IRequestClient<IAddImageRequest>, IOperationResult<IAddImageResponse>>(
                x => x.GetResponse<IOperationResult<IAddImageResponse>>(It.IsAny<IAddImageRequest>(), default, default).Result.Message, Times.Never);
        }

        //[Test]
        //public void ShouldThrowValidationException()
        //{
        //    _mocker
        //        .Setup<ICreateCompanyRequestValidator, bool>(x => x.Validate(_request).IsValid)
        //        .Returns(false);

        //    Assert.Throws<ValidationException>(() => _command.Execute(_request));
        //    _mocker.Verify<ICompanyRepository>(x => x.Add(It.IsAny<DbCompany>()), Times.Never);
        //    _mocker.Verify<ICreateCompanyRequestValidator>(x => x.Validate(It.IsAny<CreateCompanyRequest>()), Times.Once);
        //    _mocker.Verify<IDbCompanyMapper, DbCompany>(x => x.Map(It.IsAny<CreateCompanyRequest>(), It.IsAny<Guid?>()), Times.Never);
        //    _mocker.Verify<IRequestClient<IAddImageRequest>, IOperationResult<IAddImageResponse>>(
        //        x => x.GetResponse<IOperationResult<IAddImageResponse>>(It.IsAny<IAddImageRequest>(), default, default).Result.Message, Times.Never);
        //}

        //[Test]
        //public void ShouldThrowExceptionWhenRepositoryThrow()
        //{
        //    var response = new Mock<IOperationResult<IAddImageResponse>>();
        //    response
        //        .Setup(x => x.IsSuccess)
        //        .Returns(false);

        //    response
        //        .Setup(x => x.Errors)
        //        .Returns(new List<string> { "some error" });

        //    _mocker
        //       .Setup<IRequestClient<IAddImageRequest>, IOperationResult<IAddImageResponse>>(
        //           x => x.GetResponse<IOperationResult<IAddImageResponse>>(
        //               It.IsAny<object>(), default, RequestTimeout.Default).Result.Message)
        //       .Returns(response.Object);

        //    _mocker
        //        .Setup<ICreateCompanyRequestValidator, bool>(x => x.Validate(_request).IsValid)
        //        .Returns(true);

        //    _mocker
        //        .Setup<IDbCompanyMapper, DbCompany>(x => x.Map(_request, It.IsAny<Guid?>()))
        //        .Returns(_company);

        //    _mocker
        //        .Setup<ICompanyRepository>(x => x.Add(_company))
        //        .Throws(new Exception());

        //    Assert.Throws<Exception>(() => _command.Execute(_request));
        //    _mocker.Verify<ICompanyRepository>(x => x.Add(It.IsAny<DbCompany>()), Times.Once);
        //    //_mocker.Verify<ICreateOfficeRequestValidator>(x => x.Validate(_request), Times.Once);
        //    _mocker.Verify<IDbCompanyMapper, DbCompany>(x => x.Map(It.IsAny<CreateCompanyRequest>(), null), Times.Once);
        //    //_mocker.Verify<IRequestClient<IAddImageRequest>, IOperationResult<IAddImageResponse>>(
        //    //    x => x.GetResponse<IOperationResult<IAddImageResponse>>(It.IsAny<IAddImageRequest>(), default, default).Result.Message, Times.Once);
        //}

        //[Test]
        //public void ShouldCreateCompanySuccessfuly()
        //{
        //    var response = new Mock<IOperationResult<IAddImageResponse>>();
        //    response
        //        .Setup(x => x.IsSuccess)
        //        .Returns(true);

        //    response
        //        .Setup(x => x.Body.Id)
        //        .Returns(_imageId);

        //    _mocker
        //       .Setup<IRequestClient<IAddImageRequest>, IOperationResult<IAddImageResponse>>(
        //           x => x.GetResponse<IOperationResult<IAddImageResponse>>(
        //               It.IsAny<object>(), default, RequestTimeout.Default).Result.Message)
        //       .Returns(response.Object);

        //    _mocker
        //        .Setup<ICreateCompanyRequestValidator, bool>(x => x.Validate(_request).IsValid)
        //        .Returns(true);

        //    _mocker
        //        .Setup<IDbCompanyMapper, DbCompany>(x => x.Map(_request, _imageId))
        //        .Returns(_company);

        //    var expected = new OperationResultResponse<Guid>
        //    {
        //        Body = _company.Id,
        //        Status = Kernel.Enums.OperationResultStatusType.FullSuccess
        //    };

        //    SerializerAssert.AreEqual(expected, _command.Execute(_request));
        //    _mocker.Verify<ICompanyRepository>(x => x.Add(It.IsAny<DbCompany>()), Times.Once);
        //    //_mocker.Verify<ICreateOfficeRequestValidator>(x => x.Validate(It.IsAny<CreateOfficeRequest>()), Times.Once);
        //    _mocker.Verify<IDbCompanyMapper, DbCompany>(x => x.Map(It.IsAny<CreateCompanyRequest>(), _imageId), Times.Once);
        //}
    }
}
