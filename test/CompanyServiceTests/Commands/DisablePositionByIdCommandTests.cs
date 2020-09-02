using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    class DisablePositionByIdCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private IDisablePositionByIdCommand command;

        private Guid positionId;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IPositionRepository>();
            command = new DisablePositionByIdCommand(repositoryMock.Object);

            positionId = Guid.NewGuid();
        }

        [Test]
        public void ShouldThrowExceptionIfRepositoryThrowsIt()
        {
            repositoryMock.Setup(x => x.DisablePositionById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(positionId));
            repositoryMock.Verify(repository => repository.DisablePositionById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldDisablePositionSuccessfully()
        {
            repositoryMock
                .Setup(x => x.DisablePositionById(It.IsAny<Guid>()));

            command.Execute(positionId);
            repositoryMock.Verify(repository => repository.DisablePositionById(It.IsAny<Guid>()), Times.Once);
        }
    }
}