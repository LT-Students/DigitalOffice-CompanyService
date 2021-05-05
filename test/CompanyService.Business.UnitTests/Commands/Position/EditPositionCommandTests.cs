using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    public class EditPositionCommandTests
    {
        private IEditPositionCommand _command;
        private Mock<IPositionInfoValidator> _validatorMock;
        private Mock<IPositionRepository> _repositoryMock;
        private Mock<IDbPositionMapper> _mapperMock;
        private Mock<IAccessValidator> _accessValidatorMock;

        private PositionInfo _request;
        private DbPosition _newPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _request = new PositionInfo
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description",
                IsActive = true
            };

            _newPosition = new DbPosition
            {
                Id = (Guid)_request.Id,
                Name = _request.Name,
                Description = _request.Description,
                IsActive = _request.IsActive
            };
        }

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IPositionInfoValidator>();
            _repositoryMock = new Mock<IPositionRepository>();
            _mapperMock = new Mock<IDbPositionMapper>();
            _accessValidatorMock = new Mock<IAccessValidator>();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
                .Returns(true);

            _command = new EditPositionCommand(
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
            _repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Never);
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
            _repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToRepository()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _mapperMock
                .Setup(x => x.Map(It.IsAny<PositionInfo>()))
                .Returns(_newPosition);

            _repositoryMock
                .Setup(x => x.EditPosition(It.IsAny<DbPosition>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Once);
        }

        [Test]
        public void ShouldEditPositionSuccessfully()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _mapperMock
                .Setup(x => x.Map(It.IsAny<PositionInfo>()))
                .Returns(_newPosition);

            _repositoryMock
                .Setup(x => x.EditPosition(It.IsAny<DbPosition>()))
                .Returns(true);

            Assert.IsTrue(_command.Execute(_request));
            _repositoryMock.Verify(repository => repository.EditPosition(It.IsAny<DbPosition>()), Times.Once);
        }
    }
}