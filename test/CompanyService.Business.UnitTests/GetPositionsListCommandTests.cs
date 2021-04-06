﻿using LT.DigitalOffice.CompanyService.Business.Interfaces;
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

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    public class GetRightsListCommandTests
    {
        private Mock<IPositionRepository> repositoryMock;
        private Mock<IPositionMapper> mapperMock;
        private IGetPositionsListCommand command;
        private List<PositionResponse> positionsList;
        private List<DbPosition> dbPositionsList;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IPositionRepository>();
            mapperMock = new Mock<IPositionMapper>();
            command = new GetPositionsListCommand(repositoryMock.Object, mapperMock.Object);

            var dbPosition = new DbPosition
            {
                Name = "Position",
                Description = "Description"
            };
            dbPositionsList = new List<DbPosition> { dbPosition };
            var position = new PositionResponse
            {
                Info = new Position
                {
                    Name = dbPosition.Name,
                    Description = dbPosition.Description
                }
            };
            positionsList = new List<PositionResponse> { position };

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
