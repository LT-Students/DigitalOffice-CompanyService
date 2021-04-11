using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    public class EditPositionCommandTests
    {
        private Mock<IPositionValidator> validatorMock;
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IDbPositionMapper> mapperMock;

        private IEditPositionCommand command;
        private Position request;
        private DbPosition newPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new Position
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description",
                IsActive = true
            };

            newPosition = new DbPosition
            {
                Id = (Guid)request.Id,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };
        }

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IPositionValidator>();
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IDbPositionMapper>();

            command = new EditPositionCommand(validatorMock.Object, repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToValidator()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToRepository()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<Position>()))
                .Returns(newPosition);

            repositoryMock
                .Setup(x => x.EditPosition(It.IsAny<DbPosition>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Once);
        }

        [Test]
        public void ShouldEditPositionSuccessfully()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<Position>()))
                .Returns(newPosition);

            repositoryMock
                .Setup(x => x.EditPosition(It.IsAny<DbPosition>()))
                .Returns(true);

            Assert.IsTrue(command.Execute(request));
            repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}