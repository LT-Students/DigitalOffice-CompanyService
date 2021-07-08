using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    public class GetRightsListCommandTests
    {
        private IFindPositionsCommand _command;
        private Mock<IPositionRepository> _repositoryMock;
        private Mock<IPositionResponseMapper> _mapperMock;

        private List<PositionResponse> _positionsList;
        private List<DbPosition> _dbPositionsList;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPositionRepository>();
            _mapperMock = new Mock<IPositionResponseMapper>();
            _command = new FindPositionsCommand(_repositoryMock.Object, _mapperMock.Object);

            var dbPosition = new DbPosition
            {
                Name = "Position",
                Description = "Description"
            };
            _dbPositionsList = new List<DbPosition> { dbPosition };
            var position = new PositionResponse
            {
                Info = new PositionInfo
                {
                    Name = dbPosition.Name,
                    Description = dbPosition.Description
                }
            };
            _positionsList = new List<PositionResponse> { position };

            _repositoryMock
                .Setup(repository => repository.Find())
                .Returns(_dbPositionsList);

            _mapperMock
                .Setup(mapper => mapper.Map(dbPosition))
                .Returns(position);
        }

        [Test]
        public void ShouldReturnPositionsListSuccessfully()
        {
            var result = _command.Execute();

            Assert.DoesNotThrow(() => _command.Execute());
            SerializerAssert.AreEqual(_positionsList, result);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            _repositoryMock.Setup(repository => repository.Find())
                .Throws(new Exception());

            Assert.That(() => _command.Execute(), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsException()
        {
            _repositoryMock.Setup(repository => repository.Find())
                .Returns(_dbPositionsList)
                .Verifiable();
            _mapperMock.Setup(mapper => mapper.Map(It.IsAny<DbPosition>()))
                .Throws(new Exception())
                .Verifiable();

            Assert.That(() => _command.Execute(), Throws.TypeOf<Exception>());
            _repositoryMock.Verify();
            _mapperMock.Verify();
        }
    }
}
