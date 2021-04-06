using LT.DigitalOffice.CompanyService.Business.Interfaces;
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
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class GetPositionByIdCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IPositionMapper> mapperMock;
        private IGetPositionByIdCommand command;

        private DbDepartmentUser dbUsersIds;
        private DbPosition position;

        private Guid positionId;
        private Guid companyId;
        private Guid userId;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IPositionMapper>();
            command = new GetPositionByIdCommand(repositoryMock.Object, mapperMock.Object);

            companyId = Guid.NewGuid();
            userId = Guid.NewGuid();
            positionId = Guid.NewGuid();
            dbUsersIds = new DbDepartmentUser
            {
                UserId = userId,
                DepartmentId = companyId,
                PositionId = positionId,
                IsActive = true,
                StartTime = new DateTime()
            };
            position = new DbPosition
            {
                Id = positionId,
                Name = "Position",
                Description = "Description",
                Users = new List<DbDepartmentUser> { dbUsersIds }
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
            var expected = new PositionResponse
            {
                Info = new Position
                {
                    Name = position.Name,
                    Description = position.Description
                },
                UserIds = position.Users?.Select(x => x.UserId).ToList()
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