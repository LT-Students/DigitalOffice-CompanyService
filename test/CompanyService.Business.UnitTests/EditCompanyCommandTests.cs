﻿using FluentValidation;
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
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

            mapperMock
                .Setup(x => x.Map(It.IsAny<EditCompanyRequest>()))
                .Returns(dbCompany);

            Assert.Throws<BadRequestException>(() => command.Execute(request));
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
