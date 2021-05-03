using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class DisablePositionByIdCommandTests
    {
        private Mock<IPositionRepository> _repositoryMock;
        private IDisablePositionByIdCommand _command;
        private Mock<IAccessValidator> _accessValidatorMock;

        private Guid _positionId;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPositionRepository>();
            _accessValidatorMock = new Mock<IAccessValidator>();

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
                .Returns(true);

            _command = new DisablePositionByIdCommand(
                _repositoryMock.Object,
                _accessValidatorMock.Object);

            _positionId = Guid.NewGuid();
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

            Assert.Throws<ForbiddenException>(() => _command.Execute(_positionId));
            _repositoryMock.Verify(repository => repository.DisablePositionById(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionIfRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.DisablePositionById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_positionId));
            _repositoryMock.Verify(repository => repository.DisablePositionById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldDisablePositionSuccessfully()
        {
            _repositoryMock
                .Setup(x => x.DisablePositionById(It.IsAny<Guid>()));

            _command.Execute(_positionId);
            _repositoryMock.Verify(repository => repository.DisablePositionById(It.IsAny<Guid>()), Times.Once);
        }
    }
}