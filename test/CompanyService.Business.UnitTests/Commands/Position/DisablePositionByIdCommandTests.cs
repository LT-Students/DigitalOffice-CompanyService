using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    class DisablePositionByIdCommandTests
    {
        private IDisablePositionByIdCommand _command;
        private Mock<IPositionRepository> _repositoryMock;

        private Guid _positionId;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPositionRepository>();
            _command = new DisablePositionByIdCommand(_repositoryMock.Object);

            _positionId = Guid.NewGuid();
        }

        [Test]
        public void ShouldThrowExceptionIfRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.DisablePosition(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_positionId));
            _repositoryMock.Verify(repository => repository.DisablePosition(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldDisablePositionSuccessfully()
        {
            _repositoryMock
                .Setup(x => x.DisablePosition(It.IsAny<Guid>()));

            _command.Execute(_positionId);
            _repositoryMock.Verify(repository => repository.DisablePosition(It.IsAny<Guid>()), Times.Once);
        }
    }
}