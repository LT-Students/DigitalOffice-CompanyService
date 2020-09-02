using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    public class GetCompaniesListCommandTests
    {
        private Mock<ICompanyRepository> repositoryMock;
        private Mock<IMapper<DbCompany, Company>> mapperMock;
        private IGetCompaniesListCommand command;

        private List<DbCompany> dbCompanies;
        private Company company;
        private List<Company> companies;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbCompany = new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = "Lanit-Tercom",
                IsActive = true
            };

            dbCompanies = new List<DbCompany>
            {
                dbCompany
            };

            company = new Company
            {
                Id = dbCompany.Id,
                Name = dbCompany.Name,
                IsActive = dbCompany.IsActive
            };

            companies = new List<Company>
            {
                company
            };
        }

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ICompanyRepository>();
            mapperMock = new Mock<IMapper<DbCompany, Company>>();

            command = new GetCompaniesListCommand(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryReturnsException()
        {
            repositoryMock
                .Setup(x => x.GetCompaniesList())
                .Throws(new Exception());

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbCompany>()))
                .Returns(company);

            Assert.Throws<Exception>(() => command.Execute());
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<DbCompany>()), Times.Never);
        }

        [Test]
        public void ShouldReturnCompanyListWhenCompaniesFounded()
        {
            repositoryMock
                .Setup(x => x.GetCompaniesList())
                .Returns(dbCompanies);

            mapperMock
                .Setup(x => x.Map(It.IsAny<DbCompany>()))
                .Returns(company);

            Assert.AreEqual(companies, command.Execute());
        }
    }
}
