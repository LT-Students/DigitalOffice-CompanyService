using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    public class GetRightsListCommandTests
    {
        private IFindPositionsCommand _command;
        private Mock<IPositionRepository> _repositoryMock;
        private Mock<IPositionInfoMapper> _mapperMock;

        private List<PositionInfo> _positionsList;
        private List<DbPosition> _dbPositionsList;

        //[SetUp]
        //public void SetUp()
        //{
        //    _repositoryMock = new Mock<IPositionRepository>();
        //    _mapperMock = new Mock<IPositionInfoMapper>();
        //    _command = new FindPositionsCommand(_repositoryMock.Object, _mapperMock.Object);

        //    var dbPosition = new DbPosition
        //    {
        //        Name = "Position",
        //        Description = "Description"
        //    };
        //    _dbPositionsList = new List<DbPosition> { dbPosition };
        //    var position = new PositionInfo
        //    {
        //        Name = dbPosition.Name,
        //        Description = dbPosition.Description
        //    };
        //    _positionsList = new List<PositionInfo> { position };

        //    _repositoryMock
        //        .Setup(repository => repository.Find(It.IsAny true))
        //        .Returns(_dbPositionsList);

        //    _mapperMock
        //        .Setup(mapper => mapper.Map(dbPosition))
        //        .Returns(position);
        //}

        /*[Test]
        public void ShouldReturnPositionsListSuccessfully()
        {
            var result = _command.Execute(true);

            Assert.DoesNotThrow(() => _command.Execute());
            SerializerAssert.AreEqual(_positionsList, result);
        }*/

        /*[Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            _repositoryMock.Setup(repository => repository.Find(true))
                .Throws(new Exception());

            Assert.That(() => _command.Execute(), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsException()
        {
            _repositoryMock.Setup(repository => repository.Find(true))
                .Returns(_dbPositionsList)
                .Verifiable();
            _mapperMock.Setup(mapper => mapper.Map(It.IsAny<DbPosition>()))
                .Throws(new Exception())
                .Verifiable();

            Assert.That(() => _command.Execute(), Throws.TypeOf<Exception>());
            _repositoryMock.Verify();
            _mapperMock.Verify();
        }*/
    }
}
