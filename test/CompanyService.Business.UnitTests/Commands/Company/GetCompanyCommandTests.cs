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
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Company
{
    public class GetCompanyCommandTests
    {
        private Mock<ICompanyRepository> _repositoryMock;
        private Mock<ILogger<GetCompanyCommand>> _loggerMock;
        private Mock<ICompanyInfoMapper> _mapperMock;
        private Mock<IRequestClient<IGetFileRequest>> _rcMock;
        private IGetCompanyCommand _command;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new();
            _loggerMock = new();
            _mapperMock = new();
            _rcMock = new();
            _command = new GetCompanyCommand(
                _repositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object,
                _rcMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _repositoryMock
                .Setup(x => x.Get(true))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
            _repositoryMock.Verify(x => x.Get(true), Times.Once);
            _rcMock.Verify(x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, default).Result.Message, Times.Never);
            _mapperMock.Verify(x => x.Map(It.IsAny<DbCompany>(), It.IsAny<ImageInfo>()), Times.Never);
        }

        [Test]
        public void ShouldGetCompanyWithoutImage()
        {
            var response = new Mock<IOperationResult<IGetFileResponse>>();
            response
                .Setup(x => x.IsSuccess)
                .Returns(false);

            response
                .Setup(x => x.Errors)
                .Returns(new List<string> { "some error" });

            var brokerResponseMock = new Mock<Response<IOperationResult<IGetFileResponse>>>();
            brokerResponseMock
                .Setup(x => x.Message)
                .Returns(response.Object);

            _rcMock
               .Setup(x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(brokerResponseMock.Object));

            var dbCompany = new DbCompany()
            {
                Id = Guid.NewGuid(),
                LogoId = Guid.NewGuid(),
                Name = "name",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Description = "desc",
                Tagline = "tag"
            };

            var companyInfo = new CompanyInfo
            {
                Id = dbCompany.Id,
                Logo = null,
                Name = dbCompany.Name,
                Description = dbCompany.Description,
                Tagline = dbCompany.Tagline
            };

            var expected = new CompanyResponse
            {
                Company = companyInfo,
                Errors = new List<string> { $"Cannot get image with id: {dbCompany.LogoId}. Please try later." }
            };

            _repositoryMock
                .Setup(x => x.Get(true))
                .Returns(dbCompany);

            _mapperMock
                .Setup(x => x.Map(dbCompany, null))
                .Returns(companyInfo);

            SerializerAssert.AreEqual(expected, _command.Execute());
            _repositoryMock.Verify(x => x.Get(true), Times.Once);
            _rcMock.Verify(x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, default).Result.Message, Times.Once);
            _mapperMock.Verify(x => x.Map(dbCompany, null), Times.Once);
        }

        [Test]
        public void ShouldGetCompanySuccessfuly()
        {

            var response = new Mock<IOperationResult<IGetFileResponse>>();
            response
                .Setup(x => x.IsSuccess)
                .Returns(true);

            string content = "content";
            string extension = "extension";
            Guid fileId = Guid.NewGuid();

            response
                .Setup(x => x.Body.Content)
                .Returns(content);
            response
                .Setup(x => x.Body.Extension)
                .Returns(extension);
            response
                .Setup(x => x.Body.FileId)
                .Returns(fileId);

            var brokerResponseMock = new Mock<Response<IOperationResult<IGetFileResponse>>>();
            brokerResponseMock
                .Setup(x => x.Message)
                .Returns(response.Object);

            _rcMock
               .Setup(x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, default))
               .Returns(Task.FromResult(brokerResponseMock.Object));

            var dbCompany = new DbCompany()
            {
                Id = Guid.NewGuid(),
                LogoId = Guid.NewGuid(),
                Name = "name",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Description = "desc",
                Tagline = "tag"
            };

            var companyInfo = new CompanyInfo
            {
                Id = dbCompany.Id,
                Logo = new ImageInfo
                {
                    Id = fileId,
                    Content = content,
                    Extension = extension,
                    ParentId = null
                },
                Name = dbCompany.Name,
                Description = dbCompany.Description,
                Tagline = dbCompany.Tagline
            };

            var expected = new CompanyResponse
            {
                Company = companyInfo,
                Errors = new()
            };

            _repositoryMock
                .Setup(x => x.Get(true))
                .Returns(dbCompany);

            _mapperMock
                .Setup(x => x.Map(dbCompany, It.IsAny<ImageInfo>()))
                .Returns(companyInfo);

            SerializerAssert.AreEqual(expected, _command.Execute());
            _repositoryMock.Verify(x => x.Get(true), Times.Once);
            _rcMock.Verify(x => x.GetResponse<IOperationResult<IGetFileResponse>>(
                       It.IsAny<object>(), default, default).Result.Message, Times.Once);
            _mapperMock.Verify(x => x.Map(dbCompany, It.IsAny<ImageInfo>()), Times.Once);
        }
    }
}
