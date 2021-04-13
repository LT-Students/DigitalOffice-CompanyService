using CompanyService.Mappers.RequestMappers;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.UserService.Models.Broker.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.BrokerMappers
{
    internal class UserMapperTests
    {
        private IUserMapper _mapper;

        private UserData _userData;
        private User _expectedUser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new UserMapper();

            _userData = new UserData()
            {
                Id = Guid.NewGuid(),
                FirstName = "Spartak",
                LastName = "Ryabtsev",
                MiddleName = "Alexandrovich",
                IsActive = true
            };

            _expectedUser = new User
            {
                FirstName = _userData.FirstName,
                LastName = _userData.LastName,
                MiddleName = _userData.MiddleName
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenUserDataIsNull()
        {
            UserData userData = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(userData));
        }

        [Test]
        public void ShouldMapUserSuccessfully()
        {
            var user = _mapper.Map(_userData);

            SerializerAssert.AreEqual(_expectedUser, user);
        }
    }
}
