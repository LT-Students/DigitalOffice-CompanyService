using FluentValidation;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using FluentValidation.Results;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class AddPositionCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IMapper<Position, DbPosition>> mapperMock;
        private Mock<IValidator<Position>> validatorMock;

        private IAddPositionCommand command;
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
            mapperMock = new Mock<IMapper<Position, DbPosition>>();

            command = new AddPositionCommand(validatorMock.Object, repositoryMock.Object, mapperMock.Object);
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
            repositoryMock.Verify(repository => repository.AddPosition(It.IsAny<DbPosition>()), Times.Never);
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
                .Setup(x => x.AddPosition(It.IsAny<DbPosition>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.AddPosition(It.IsAny<DbPosition>()), Times.Once);
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
                .Setup(x => x.AddPosition(It.IsAny<DbPosition>()))
                .Returns(createdPosition.Id);

            Assert.AreEqual(createdPosition.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.AddPosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}