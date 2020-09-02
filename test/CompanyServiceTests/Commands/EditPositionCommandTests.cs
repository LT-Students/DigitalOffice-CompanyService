using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using Moq;
using NUnit.Framework;
using System;
using FluentValidation;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    public class EditPositionCommandTests
    {
        private Mock<IValidator<EditPositionRequest>> validatorMock;
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IMapper<EditPositionRequest, DbPosition>> mapperMock;

        private IEditPositionCommand command;
        private EditPositionRequest request;
        private DbPosition newPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new EditPositionRequest
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description"
            };

            newPosition = new DbPosition
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };
        }

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IValidator<EditPositionRequest>>();
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IMapper<EditPositionRequest, DbPosition>>();

            command = new EditPositionCommand(validatorMock.Object, repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToValidator()
        {
            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => command.Execute(request), "Position field validation error");
            repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToRepository()
        {
            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<EditPositionRequest>()))
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
                .Setup(x => x.Map(It.IsAny<EditPositionRequest>()))
                .Returns(newPosition);

            repositoryMock
                .Setup(x => x.EditPosition(It.IsAny<DbPosition>()))
                .Returns(true);

            Assert.IsTrue(command.Execute(request));
            repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}