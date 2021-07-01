using LT.DigitalOffice.CompanyService.Business.Commands.Company;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Company
{
    public class GetCompanyCommandTests
    {
        private Mock<ICompanyRepository> _repositoryMock;
        private Mock<ICompanyInfoMapper> _mapperMock;
        private IGetCompanyCommand _command;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new();
            _mapperMock = new();
            _command = new GetCompanyCommand(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _repositoryMock
                .Setup(x => x.Get(true))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
            _repositoryMock.Verify(x => x.Get(true), Times.Once);
            _mapperMock.Verify(x => x.Map(It.IsAny<DbCompany>(), It.IsAny<ImageInfo>()), Times.Never);
        }

        [Test]
        public void ShouldGetCompanySuccessfuly()
        {
            var dbCompany = new DbCompany()
            {
                Id = Guid.NewGuid(),
                LogoId = Guid.NewGuid(),
                PortalName = "portalname",
                CompanyName = "name",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Description = "desc",
                Tagline = "tag"
            };

            var companyInfo = new CompanyInfo
            {
                Id = dbCompany.Id,
                Logo = new ImageInfo(),
                PortalName = dbCompany.PortalName,
                CompanyName = dbCompany.CompanyName,
                Description = dbCompany.Description,
                Tagline = dbCompany.Tagline
            };

            _repositoryMock
                .Setup(x => x.Get(true))
                .Returns(dbCompany);

            _mapperMock
                .Setup(x => x.Map(dbCompany, It.IsAny<ImageInfo>()))
                .Returns(companyInfo);

            SerializerAssert.AreEqual(companyInfo, _command.Execute());
            _repositoryMock.Verify(x => x.Get(true), Times.Once);
            _mapperMock.Verify(x => x.Map(dbCompany, It.IsAny<ImageInfo>()), Times.Once);
        }
    }
}
