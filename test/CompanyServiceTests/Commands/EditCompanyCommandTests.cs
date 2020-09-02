using FluentValidation;
using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    public class EditCompanyCommandTests
    {
        private Mock<IValidator<EditCompanyRequest>> validatorMock;
        private Mock<IMapper<EditCompanyRequest, DbCompany>> mapperMock;
        private Mock<ICompanyRepository> repositoryMock;

        private IEditCompanyCommand command;
        private EditCompanyRequest request;
        private DbCompany dbCompany;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new EditCompanyRequest
            {
                CompanyId = Guid.NewGuid(),
                Name = "Microsoft",
                IsActive = false
            };

            dbCompany = new DbCompany
            {
                Id = request.CompanyId,
                Name = request.Name,
                IsActive = request.IsActive
            };
        }

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IValidator<EditCompanyRequest>>();
            mapperMock = new Mock<IMapper<EditCompanyRequest, DbCompany>>();
            repositoryMock = new Mock<ICompanyRepository>();

            command = new EditCompanyCommand(validatorMock.Object, mapperMock.Object, repositoryMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenValidatorReturnFalse()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            mapperMock
                .Setup(x => x.Map(It.IsAny<EditCompanyRequest>()))
                .Returns(dbCompany);

            Assert.Throws<ValidationException>(() => command.Execute(request));
            mapperMock.Verify(mapper => mapper.Map(It.IsAny<EditCompanyRequest>()), Times.Never);
            repositoryMock.Verify(repository => repository.UpdateCompany(It.IsAny<DbCompany>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowException()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<EditCompanyRequest>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.UpdateCompany(It.IsAny<DbCompany>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowException()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<EditCompanyRequest>()))
                .Returns(dbCompany);

            repositoryMock
                .Setup(x => x.UpdateCompany(It.IsAny<DbCompany>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
        }

        [Test]
        public void ShouldUpdateCompanySuccessfully()
        {
            const bool response = true;

            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<EditCompanyRequest>()))
                .Returns(dbCompany);

            repositoryMock
                .Setup(x => x.UpdateCompany(It.IsAny<DbCompany>()))
                .Returns(response);

            Assert.AreEqual(response, command.Execute(request));
            repositoryMock.Verify(repository => repository.UpdateCompany(It.IsAny<DbCompany>()), Times.Once);
        }
    }
}
