using FluentValidation;
using Moq;
using NUnit.Framework;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using FluentValidation.Results;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class CreatePositionCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IDbPositionMapper> mapperMock;
        private Mock<IValidator<Position>> validatorMock;

        private ICreatePositionCommand command;
        private Position request;
        private DbPosition createdPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new Position
            {
                Name = "Position",
                Description = "Description"
            };

            createdPosition = new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };
        }

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IValidator<Position>>();
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IDbPositionMapper>();

            command = new CreatePositionCommand(validatorMock.Object, repositoryMock.Object, mapperMock.Object);
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
            repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToRepository()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<Position>()))
                .Returns(createdPosition);

            repositoryMock
                .Setup(x => x.CreatePosition(It.IsAny<DbPosition>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Once);
        }

        [Test]
        public void ShouldCreateNewPositionSuccessfully()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<Position>()))
                .Returns(createdPosition);

            repositoryMock
                .Setup(x => x.CreatePosition(It.IsAny<DbPosition>()))
                .Returns(createdPosition.Id);

            Assert.AreEqual(createdPosition.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}