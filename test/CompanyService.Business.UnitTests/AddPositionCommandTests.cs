using FluentValidation;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Exceptions;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class AddPositionCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IMapper<PositionInfo, DbPosition>> mapperMock;
        private Mock<IValidator<PositionInfo>> validatorMock;

        private IAddPositionCommand command;
        private PositionInfo request;
        private DbPosition createdPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new PositionInfo
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
            validatorMock = new Mock<IValidator<PositionInfo>>();
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IMapper<PositionInfo, DbPosition>>();

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
                .Setup(x => x.Map(It.IsAny<PositionInfo>()))
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
                .Setup(x => x.Map(It.IsAny<PositionInfo>()))
                .Returns(createdPosition);

            repositoryMock
                .Setup(x => x.AddPosition(It.IsAny<DbPosition>()))
                .Returns(createdPosition.Id);

            Assert.AreEqual(createdPosition.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.AddPosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}