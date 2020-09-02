using FluentValidation;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    public class AddCompanyCommandTests
    {
        private Mock<IValidator<AddCompanyRequest>> validatorMock;
        private Mock<IMapper<AddCompanyRequest, DbCompany>> mapperMock;
        private Mock<ICompanyRepository> repositoryMock;

        private IAddCompanyCommand command;
        private AddCompanyRequest request;
        private DbCompany createdCompany;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new AddCompanyRequest
            {
                Name = "Lanit-Tercom"
            };

            createdCompany = new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true
            };
        }

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IValidator<AddCompanyRequest>>();
            mapperMock = new Mock<IMapper<AddCompanyRequest, DbCompany>>();
            repositoryMock = new Mock<ICompanyRepository>();

            command = new AddCompanyCommand(validatorMock.Object, mapperMock.Object, repositoryMock.Object);
        }


        [Test]
        public void ShouldThrowValidationExceptionWhenValidatorReturnsFalse()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.AddCompany(It.IsAny<DbCompany>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowingException()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<AddCompanyRequest>()))
                .Returns(createdCompany);

            repositoryMock
                .Setup(x => x.AddCompany(It.IsAny<DbCompany>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
        }

        [Test]
        public void ShouldReturnIdFromRepositoryWhenCompanyAdded()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<AddCompanyRequest>()))
                .Returns(createdCompany);

            repositoryMock
                .Setup(x => x.AddCompany(It.IsAny<DbCompany>()))
                .Returns(createdCompany.Id);

            Assert.AreEqual(createdCompany.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.AddCompany(It.IsAny<DbCompany>()), Times.Once);
        }
    }
}
