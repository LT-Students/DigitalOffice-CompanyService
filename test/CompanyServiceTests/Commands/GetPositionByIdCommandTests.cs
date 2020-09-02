using LT.DigitalOffice.CompanyService.Commands;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    class GetPositionByIdCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IMapper<DbPosition, Position>> mapperMock;
        private IGetPositionByIdCommand command;

        private DbCompanyUser dbUsersIds;
        private DbPosition position;

        private Guid positionId;
        private Guid companyId;
        private Guid userId;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IMapper<DbPosition, Position>>();
            command = new GetPositionByIdCommand(repositoryMock.Object, mapperMock.Object);

            companyId = Guid.NewGuid();
            userId = Guid.NewGuid();
            positionId = Guid.NewGuid();
            dbUsersIds = new DbCompanyUser
            {
                UserId = userId,
                CompanyId = companyId,
                PositionId = positionId,
                IsActive = true,
                StartTime = new DateTime()
            };
            position = new DbPosition
            {
                Id = positionId,
                Name = "Position",
                Description = "Description",
                UserIds = new List<DbCompanyUser> { dbUsersIds }
            };
        }

        [Test]
        public void ShouldThrowExceptionIfRepositoryThrowsIt()
        {
            repositoryMock.Setup(x => x.GetPositionById(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(positionId));
            repositoryMock.Verify(repository => repository.GetPositionById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionIfMapperThrowsIt()
        {
            mapperMock.Setup(x => x.Map(It.IsAny<DbPosition>())).Throws(new Exception());

            Assert.Throws<Exception>(() => command.Execute(positionId));
            repositoryMock.Verify(repository => repository.GetPositionById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldReturnPositionInfoSuccessfully()
        {
            var expected = new Position
            {
                Name = position.Name,
                Description = position.Description,
                UserIds = position.UserIds?.Select(x => x.UserId).ToList()
            };

            repositoryMock
                .Setup(x => x.GetPositionById(It.IsAny<Guid>()))
                .Returns(position);
            mapperMock
                .Setup(x => x.Map(It.IsAny<DbPosition>()))
                .Returns(expected);

            var result = command.Execute(positionId);

            SerializerAssert.AreEqual(expected, result);
        }
    }
}