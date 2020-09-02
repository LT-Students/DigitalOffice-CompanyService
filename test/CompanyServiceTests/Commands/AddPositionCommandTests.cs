using FluentValidation;
using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    class AddPositionCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IMapper<AddPositionRequest, DbPosition>> mapperMock;
        private Mock<IValidator<AddPositionRequest>> validatorMock;

        private IAddPositionCommand command;
        private AddPositionRequest request;
        private DbPosition createdPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new AddPositionRequest
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
            validatorMock = new Mock<IValidator<AddPositionRequest>>();
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IMapper<AddPositionRequest, DbPosition>>();

            command = new AddPositionCommand(validatorMock.Object, repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToValidator()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

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
                .Setup(x => x.Map(It.IsAny<AddPositionRequest>()))
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
                .Setup(x => x.Map(It.IsAny<AddPositionRequest>()))
                .Returns(createdPosition);

            repositoryMock
                .Setup(x => x.AddPosition(It.IsAny<DbPosition>()))
                .Returns(createdPosition.Id);

            Assert.AreEqual(createdPosition.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.AddPosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}