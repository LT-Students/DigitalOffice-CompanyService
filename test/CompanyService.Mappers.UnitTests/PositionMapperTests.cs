﻿using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests
{
    public class PositionMappersTests
    {
        private IMapper<Position, DbPosition> mapperAddPositionRequest;
        private IMapper<DbPosition, PositionResponse> mapperDbPositionToPosition;
        private IMapper<Position, DbPosition> mapperEditPositionRequestToDbPosition;

        private DbDepartmentUser dbUserIds;
        private DbPosition dbPosition;

        private Position editPositionRequest;
        private Position addPositionRequest;
        private DbPosition expectedDbPosition;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            mapperDbPositionToPosition = new ResponsesMappers.DbPositionMapper();
            mapperEditPositionRequestToDbPosition = new RequestMappers.DbPositionMapper();
            mapperAddPositionRequest = new RequestMappers.DbPositionMapper();
        }

        [SetUp]
        public void SetUp()
        {
            editPositionRequest = new Position
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description",
                IsActive = true
            };

            addPositionRequest = new Position
            {
                Name = "Name",
                Description = "Description",
                IsActive = true
            };
            expectedDbPosition = new DbPosition
            {
                Name = addPositionRequest.Name,
                Description = addPositionRequest.Description,
                IsActive = true
            };

            dbUserIds = new DbDepartmentUser
            {
                UserId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                PositionId = Guid.NewGuid(),
                IsActive = true,
                StartTime = new DateTime()
            };
            dbPosition = new DbPosition
            {
                Id = dbUserIds.PositionId,
                Name = "Position",
                Description = "Description",
                IsActive = true,
                Users = new List<DbDepartmentUser> { dbUserIds }
            };
        }

        #region AddPositionRequest to DbPosition
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullAddPositionRequestToDbPosition()
        {
            Assert.Throws<ArgumentNullException>(() => mapperAddPositionRequest.Map(null));
        }

        public void ShouldMapAddPositionRequestToDbPositionSuccessfully()
        {
            var resultDbPosition = mapperAddPositionRequest.Map(addPositionRequest);

            expectedDbPosition.Id = resultDbPosition.Id;

            Assert.IsInstanceOf<Guid>(resultDbPosition.Id);
            SerializerAssert.AreEqual(expectedDbPosition, resultDbPosition);
        }
        #endregion

        #region DbPosition to Position
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullDbPositionToPosition()
        {
            Assert.Throws<ArgumentNullException>(() => mapperDbPositionToPosition.Map(null));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfully()
        {
            var result = mapperDbPositionToPosition.Map(dbPosition);

            var expected = new PositionResponse
            {
                Info = new Position
                {
                    Name = dbPosition.Name,
                    Description = dbPosition.Description,
                    IsActive = dbPosition.IsActive
                },
                UserIds = dbPosition.Users?.Select(x => x.UserId).ToList()
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion

        #region EditPositionRequest to DbPosition
        [Test]
        public void ShouldThrowExceptionIfArgumentIsNullEditPositionRequestToDbPosition()
        {
            Assert.Throws<ArgumentNullException>(() => mapperEditPositionRequestToDbPosition.Map(null));
        }

        [Test]
        public void ShouldReturnPositionModelSuccessfullyEditPositionRequestToDbPosition()
        {
            var result = mapperEditPositionRequestToDbPosition.Map(editPositionRequest);

            var expected = new DbPosition
            {
                Id = result.Id,
                Name = editPositionRequest.Name,
                Description = editPositionRequest.Description,
                IsActive = true
            };

            SerializerAssert.AreEqual(expected, result);
        }
        #endregion
    }
}