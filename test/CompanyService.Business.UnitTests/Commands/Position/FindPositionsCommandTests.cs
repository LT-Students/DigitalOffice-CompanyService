using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    public class FindPositionsCommandTests
    {
        private IFindPositionsCommand _command;
        private AutoMocker _autoMock;

        private List<PositionInfo> _positionsList;
        private List<DbPosition> _dbPositionsList;
        private FindResultResponse<PositionInfo> _findResultResponse;

        [SetUp]
        public void SetUp()
        {
            _autoMock = new AutoMocker();

            var dbPosition = new DbPosition
            {
                Name = "Position",
                Description = "Description"
            };
            _dbPositionsList = new List<DbPosition> { dbPosition };

            var position = new PositionInfo
            {
                Name = dbPosition.Name,
                Description = dbPosition.Description
            };
            _positionsList = new List<PositionInfo> { position };

            int value = 1;
            _autoMock
                .Setup<IPositionRepository, List<DbPosition>>(x => x.Find(0, 15, true, out value))
                .Returns(_dbPositionsList);

            _autoMock
                .Setup<IPositionInfoMapper, PositionInfo>(x => x.Map(dbPosition))
                .Returns(position);

            _command = new FindPositionsCommand(_autoMock.GetMock<IPositionRepository>().Object,
                _autoMock.GetMock<IPositionInfoMapper>().Object);


            _findResultResponse = new()
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _positionsList,
                TotalCount = 1
            };

            /*            _mapperMock
                            .Setup(mapper => mapper.Map(dbPosition))
                            .Returns(position);*/
        }

        [Test]
        public void ShouldReturnPositionsListSuccessfully()
        {
            var result = _command.Execute(0, 15, true);

            Assert.DoesNotThrow(() => _command.Execute(0, 15, true));
            SerializerAssert.AreEqual(_findResultResponse, result);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            int value;
            _autoMock
                .Setup<IPositionRepository, List<DbPosition>>(x => x.Find(0, 15, true, out value))
                .Throws(new Exception());
            Assert.That(() => _command.Execute(0, 15, true), Throws.TypeOf<Exception>());
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowsException()
        {
            _autoMock
                .Setup<IPositionInfoMapper, PositionInfo>(x => x.Map(It.IsAny<DbPosition>()))
                .Throws(new Exception())
                .Verifiable();

            int value;
            _autoMock
                .Setup<IPositionRepository, List<DbPosition>>(x => x.Find(0, 15, true, out value))
                .Returns(_dbPositionsList)
                .Verifiable();

            Assert.That(() => _command.Execute(0, 15, true), Throws.TypeOf<Exception>());

            _autoMock.Verify<IPositionInfoMapper, PositionInfo>(
                x => x.Map(It.IsAny<DbPosition>()),
                Times.Once());

            _autoMock.Verify<IPositionRepository, List<DbPosition>>(
                x => x.Find(0, 15, true, out value),
                Times.Once());
        }
    }
}
