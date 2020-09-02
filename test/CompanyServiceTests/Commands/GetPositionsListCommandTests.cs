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

namespace LT.DigitalOffice.CompanyServiceUnitTests.Commands
{
    public class GetRightsListCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IMapper<DbPosition, Position>> mapperMock;
        private IGetPositionsListCommand command;
        private List<Position> positionsList;
        private List<DbPosition> dbPositionsList;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IMapper<DbPosition, Position>>();
            command = new GetPositionsListCommand(repositoryMock.Object, mapperMock.Object);

            var dbPosition = new DbPosition
            {
                Name = "Position",
                Description = "Description"
            };
            dbPositionsList = new List<DbPosition> { dbPosition };
            var position = new Position
            {
                Name = dbPosition.Name,
                Description = dbPosition.Description
            };
            positionsList = new List<Position> { position };

            repositoryMock
                .Setup(repository => repository.GetPositionsList())
                .Returns(dbPositionsList);

            mapperMock
                .Setup(mapper => mapper.Map(dbPosition))
                .Returns(position);
        }

        [Test]
        public void ShouldReturnPositionsListSuccessfully()
        {
            var result = command.Execute();

            Assert.DoesNotThrow(() => command.Execute());
            SerializerAssert.AreEqual(positionsList, result);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            repositoryMock.Setup(repository => repository.GetPositionsList())
                .Throws(new Exception());

            Assert.That(() => command.Execute(), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsException()
        {
            repositoryMock.Setup(repository => repository.GetPositionsList())
                .Returns(dbPositionsList)
                .Verifiable();
            mapperMock.Setup(mapper => mapper.Map(It.IsAny<DbPosition>()))
                .Throws(new Exception())
                .Verifiable();

            Assert.That(() => command.Execute(), Throws.TypeOf<Exception>());
            repositoryMock.Verify();
            mapperMock.Verify();
        }
    }
}
