using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
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
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

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
