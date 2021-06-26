using FluentValidation;
using Moq;
using NUnit.Framework;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    class CreatePositionCommandTests
    {
        private ICreatePositionCommand _command;
        private Mock<IPositionRepository> _repositoryMock;
        private Mock<ICompanyRepository> _companyRepositoryMock;
        private Mock<IDbPositionMapper> _mapperMock;
        private Mock<ICreatePositionRequestValidator> _validatorMock;
        private Mock<IAccessValidator> _accessValidatorMock;
        private CreatePositionRequest _request;

        private Guid _companyId;
        private DbPosition _createdPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _companyId = Guid.NewGuid();

            _request = new CreatePositionRequest
            {
                Name = "Position",
                Description = "Description"
            };

            _createdPosition = new DbPosition
            {
                Id = Guid.NewGuid(),
                CompanyId = _companyId,
                Name = _request.Name,
                Description = _request.Description
            };
        }

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<ICreatePositionRequestValidator>();
            _repositoryMock = new Mock<IPositionRepository>();
            _mapperMock = new Mock<IDbPositionMapper>();
            _accessValidatorMock = new Mock<IAccessValidator>();
            _companyRepositoryMock = new();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
                .Returns(true);

            _command = new CreatePositionCommand(
                _validatorMock.Object,
                _repositoryMock.Object,
                _companyRepositoryMock.Object,
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
            _companyRepositoryMock.Verify(repository => repository.Get(false), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToValidator()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Never);
            _companyRepositoryMock.Verify(repository => repository.Get(false), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionAccordingToRepository()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _mapperMock
                .Setup(x => x.Map(It.IsAny<PositionInfo>(), _companyId))
                .Returns(_createdPosition);

            _companyRepositoryMock
                .Setup(x => x.Get(false))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Never);
            _companyRepositoryMock.Verify(repository => repository.Get(false), Times.Once);
        }

        [Test]
        public void ShouldCreateNewPositionSuccessfully()
        {
            _validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            _companyRepositoryMock
                .Setup(x => x.Get(false))
                .Returns(new DbCompany { Id = _companyId });

            _mapperMock
                .Setup(x => x.Map(It.IsAny<PositionInfo>(), _companyId))
                .Returns(_createdPosition);

            _repositoryMock
                .Setup(x => x.CreatePosition(It.IsAny<DbPosition>()))
                .Returns(_createdPosition.Id);

            Assert.AreEqual(_createdPosition.Id, _command.Execute(_request));
            _repositoryMock.Verify(repository => repository.CreatePosition(It.IsAny<DbPosition>()), Times.Once);
            _companyRepositoryMock.Verify(repository => repository.Get(false), Times.Once);
        }
    }
}