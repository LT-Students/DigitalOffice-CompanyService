﻿using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    public class ShortDepartmentsInfoMapperTests
    {
        private IShortDepartmentInfoMapper _mapper;
        private Mock<IUserInfoMapper> _userInfoMapperMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userInfoMapperMock = new Mock<IUserInfoMapper>();
        }

        [SetUp]
        public void SetUp()
        {
            UserInfo userData = null;

            _userInfoMapperMock
                .Setup(x => x.Map(It.IsAny<UserData>()))
                .Returns(userData);

            _mapper = new ShortDepartmentInfoMapper(_userInfoMapperMock.Object);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            DbDepartment department = new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CompanyId = Guid.NewGuid(),
                Description = "Description",
                IsActive = true
            };

            ShortDepartmentInfo expected = new()
            {
                Id = department.Id,
                Name = "Name",
                Description = "Description",
                IsActive = true
            };

            SerializerAssert.AreEqual(expected, _mapper.Map(department, null));
        }
    }
}
