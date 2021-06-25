using LT.DigitalOffice.CompanyService.Business.Commands.Company;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Responses.File;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Company
{
    public class GetCompanyCommandTests
    {
        private AutoMocker _mocker;
        private IGetCompanyCommand _command;

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _command = _mocker.CreateInstance<GetCompanyCommand>();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            var response = new Mock<IOperationResult<IGetFileResponse>>();
            response
                .Setup(x => x.IsSuccess)
                .Returns(false);

            response
                .Setup(x => x.Errors)
                .Returns(new List<string> { "some error" });

            _mocker
               .Setup<IRequestClient<IGetFileRequest>, IOperationResult<IGetFileResponse>>(
                   x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, RequestTimeout.Default).Result.Message)
               .Returns(response.Object);

            _mocker
                .Setup<ICompanyRepository, DbCompany>(x => x.Get())
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
            _mocker.Verify<ICompanyRepository, DbCompany>(x => x.Get(), Times.Once);
            _mocker.Verify<IRequestClient<IGetFileRequest>, IOperationResult<IGetFileResponse>>(
                   x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, RequestTimeout.Default).Result.Message, Times.Never);
            _mocker.Verify<ICompanyInfoMapper, CompanyInfo>(x => x.Map(It.IsAny<DbCompany>(), It.IsAny<ImageInfo>()), Times.Never);
        }

        //[Test]
        //public void ShouldGetCompanyWithoutImage()
        //{
        //    var response = new Mock<IOperationResult<IGetFileResponse>>();
        //    response
        //        .Setup(x => x.IsSuccess)
        //        .Returns(false);

        //    response
        //        .Setup(x => x.Errors)
        //        .Returns(new List<string> { "some error" });

        //    _mocker
        //       .Setup<IRequestClient<IGetFileRequest>, IOperationResult<IGetFileResponse>>(
        //           x => x.GetResponse<IOperationResult<IGetFileResponse>>(
        //               It.IsAny<object>(), default, RequestTimeout.Default).Result.Message)
        //       .Returns(response.Object);

        //    var dbCompany = new DbCompany()
        //    {
        //        Id = Guid.NewGuid(),
        //        LogoId = Guid.NewGuid(),
        //        Name = "name",
        //        CreatedAt = DateTime.UtcNow,
        //        IsActive = true,
        //        Description = "desc",
        //        Tagline = "tag"
        //    };

        //    var companyInfo = new CompanyInfo
        //    {
        //        Id = dbCompany.Id,
        //        Logo = null,
        //        Name = dbCompany.Name,
        //        Description = dbCompany.Description,
        //        Tagline = dbCompany.Tagline
        //    };

        //    var expected = new CompanyResponse
        //    {
        //        Company = companyInfo,
        //        Errors = new List<string> { $"Cannot get image with id: {dbCompany.LogoId}. Please try later." }
        //    };

        //    _mocker
        //        .Setup<ICompanyRepository, DbCompany>(x => x.Get())
        //        .Returns(dbCompany);

        //    _mocker
        //        .Setup<ICompanyInfoMapper, CompanyInfo>(x => x.Map(dbCompany, null))
        //        .Returns(companyInfo);

        //    SerializerAssert.AreEqual(expected, _command.Execute());
        //    _mocker.Verify<ICompanyRepository, DbCompany>(x => x.Get(), Times.Once);
        //    _mocker.Verify<IRequestClient<IGetFileRequest>, IOperationResult<IGetFileResponse>>(
        //           x => x.GetResponse<IOperationResult<IGetFileResponse>>(
        //               It.IsAny<object>(), default, RequestTimeout.Default).Result.Message, Times.Once);
        //    _mocker.Verify<ICompanyInfoMapper, CompanyInfo>(x => x.Map(It.IsAny<DbCompany>(), It.IsAny<ImageInfo>()), Times.Once);
        //}

        //[Test]
        //public void ShouldGetCompanySuccessfuly()
        //{
        //    var response = new Mock<IOperationResult<IGetFileResponse>>();
        //    response
        //        .Setup(x => x.IsSuccess)
        //        .Returns(true);

        //    string content = "content";
        //    string extension = "extension";
        //    Guid fileId = Guid.NewGuid();

        //    response
        //        .Setup(x => x.Body.Content)
        //        .Returns(content);
        //    response
        //        .Setup(x => x.Body.Extension)
        //        .Returns(extension);
        //    response
        //        .Setup(x => x.Body.FileId)
        //        .Returns(fileId);

        //    _mocker
        //       .Setup<IRequestClient<IGetFileRequest>, IOperationResult<IGetFileResponse>>(
        //           x => x.GetResponse<IOperationResult<IGetFileResponse>>(
        //               It.IsAny<object>(), default, RequestTimeout.Default).Result.Message)
        //       .Returns(response.Object);

        //    var dbCompany = new DbCompany()
        //    {
        //        Id = Guid.NewGuid(),
        //        LogoId = Guid.NewGuid(),
        //        Name = "name",
        //        CreatedAt = DateTime.UtcNow,
        //        IsActive = true,
        //        Description = "desc",
        //        Tagline = "tag"
        //    };

        //    var companyInfo = new CompanyInfo
        //    {
        //        Id = dbCompany.Id,
        //        Logo = new ImageInfo
        //        {
        //            Id = fileId,
        //            Content = content,
        //            Extension = extension,
        //            ParentId = null
        //        },
        //        Name = dbCompany.Name,
        //        Description = dbCompany.Description,
        //        Tagline = dbCompany.Tagline
        //    };

        //    var expected = new CompanyResponse
        //    {
        //        Company = companyInfo,
        //        Errors = new List<string> { $"Cannot get image with id: {dbCompany.LogoId}. Please try later." }
        //    };

        //    _mocker
        //        .Setup<ICompanyRepository, DbCompany>(x => x.Get())
        //        .Returns(dbCompany);

        //    _mocker
        //        .Setup<ICompanyInfoMapper, CompanyInfo>(x => x.Map(dbCompany, It.IsAny<ImageInfo>()))
        //        .Returns(companyInfo);

        //    SerializerAssert.AreEqual(expected, _command.Execute());
        //    _mocker.Verify<ICompanyRepository, DbCompany>(x => x.Get(), Times.Once);
        //    _mocker.Verify<IRequestClient<IGetFileRequest>, IOperationResult<IGetFileResponse>>(
        //           x => x.GetResponse<IOperationResult<IGetFileResponse>>(
        //               It.IsAny<object>(), default, RequestTimeout.Default).Result.Message, Times.Once);
        //    _mocker.Verify<ICompanyInfoMapper, CompanyInfo>(x => x.Map(It.IsAny<DbCompany>(), It.IsAny<ImageInfo>()), Times.Once);
        //}
    }
}
