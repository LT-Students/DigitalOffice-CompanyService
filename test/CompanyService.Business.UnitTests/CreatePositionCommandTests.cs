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
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class CreatePositionCommandTests
    {
        private Mock<IPositionRepository> _repositoryMock;
        private Mock<IDbPositionMapper> _mapperMock;
        private Mock<IPositionValidator> _validatorMock;
        private Mock<IAccessValidator> _accessValidatorMock;

        private ICreatePositionCommand _command;
        private Position _request;
        private DbPosition _createdPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _request = new Position
            {
                Name = "Position",
                Description = "Description"
            };

            _createdPosition = new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = _request.Name,
                Description = _request.Description
            };
        }

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IPositionValidator>();
            _repositoryMock = new Mock<IPositionRepository>();
            _mapperMock = new Mock<IDbPositionMapper>();
            _accessValidatorMock = new Mock<IAccessValidator>();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
                .Returns(true);

            _command = new CreatePositionCommand(
                _validatorMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object,
                _accessValidatorMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenNotEnoughRights()
        {
            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
                .Returns(false);

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToValidator()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToRepository()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _mapperMock
                .Setup(x => x.Map(It.IsAny<Position>()))
                .Returns(_createdPosition);

            _repositoryMock
                .Setup(x => x.CreatePosition(It.IsAny<DbPosition>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Once);
        }

        [Test]
        public void ShouldCreateNewPositionSuccessfully()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _mapperMock
                .Setup(x => x.Map(It.IsAny<Position>()))
                .Returns(_createdPosition);

            _repositoryMock
                .Setup(x => x.CreatePosition(It.IsAny<DbPosition>()))
                .Returns(_createdPosition.Id);

            Assert.AreEqual(_createdPosition.Id, _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}