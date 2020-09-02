using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    public class GetCompanyByIdCommandTests
    {
        private Mock<ICompanyRepository> repositoryMock;
        private Mock<IMapper<DbCompany, Company>> mapperMock;

        private IGetCompanyByIdCommand command;

        private DbCompany dbCompany;
        private Company company;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            dbCompany = new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = "Lanit-Tercom",
                IsActive = true
            };

            company = new Company
            {
                Id = dbCompany.Id,
                Name = dbCompany.Name,
                IsActive = dbCompany.IsActive
            };
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ICompanyRepository>();
            mapperMock = new Mock<IMapper<DbCompany, Company>>();

            command = new GetCompanyByIdCommand(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryReturnsException()
        {
            repositoryMock
                .Setup(x => x.GetCompanyById(It.IsAny<Guid>()))
                .Throws(new Exception());

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbCompany>()))
                .Returns(company);

            Assert.Throws<Exception>(() => command.Execute(dbCompany.Id));
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbCompany>()), Times.Never);
        }

        [Test]
        public void ShouldReturnCompanyModelWhenCompanyFounded()
        {
            repositoryMock
                .Setup(x => x.GetCompanyById(It.IsAny<Guid>()))
                .Returns(dbCompany);

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbCompany>()))
                .Returns(company);

            Assert.AreEqual(company, command.Execute(dbCompany.Id));
        }
    }
}
