using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
    class GetPositionCommandTests
    {
        private IGetPositionCommand _command;
        private Mock<IPositionRepository> _repositoryMock;
        private Mock<IPositionInfoMapper> _mapperMock;

        private DbPositionUser _dbPositionUser;
        private DbPosition _position;

        private Guid _positionId;
        private Guid _companyId;
        private Guid _userId;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPositionRepository>();
            _mapperMock = new Mock<IPositionInfoMapper>();
            _command = new GetPositionCommand(_repositoryMock.Object, _mapperMock.Object);

            _companyId = Guid.NewGuid();
            _userId = Guid.NewGuid();
            _positionId = Guid.NewGuid();
            _dbPositionUser = new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                PositionId = _positionId,
                IsActive = true,
                StartTime = new DateTime()
            };
            _position = new DbPosition
            {
                Id = _positionId,
                Name = "Position",
                Description = "Description",
                Users = new List<DbPositionUser> { _dbPositionUser }
            };
        }

        [Test]
        public void ShouldThrowExceptionIfRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.Get(It.IsAny<Guid?>(), It.IsAny<Guid?>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_positionId));
            _repositoryMock.Verify(repository => repository.Get(It.IsAny<Guid?>(), It.IsAny<Guid?>()), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionIfMapperThrowsIt()
        {
            _mapperMock
                .Setup(x => x.Map(It.IsAny<DbPosition>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_positionId));
            _repositoryMock.Verify(repository => repository.Get(It.IsAny<Guid?>(), It.IsAny<Guid?>()), Times.Once);
        }

        [Test]
        public void ShouldReturnPositionInfoSuccessfully()
        {
            var expected = new OperationResultResponse<PositionInfo>
            {
                Status = Kernel.Enums.OperationResultStatusType.FullSuccess,
                Body = new PositionInfo
                {
                    Name = _position.Name,
                    Description = _position.Description
                }
            };

            _repositoryMock
                .Setup(x => x.Get(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .Returns(_position);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<DbPosition>()))
                .Returns(expected.Body);

            var result = _command.Execute(_positionId);

            SerializerAssert.AreEqual(expected, result);
        }
    }
}