using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    internal class UserInfoMapperTests
    {
        private IUserInfoMapper _mapper;

        private UserData _userData;
        private UserInfo _expectedUser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new UserInfoMapper();

            _userData = new UserData(Guid.NewGuid(), "Ivan", "Ivanovich", "Ivanov", true, null, null);

            _expectedUser = new UserInfo
            {
                Id = _userData.Id,
                FirstName = _userData.FirstName,
                LastName = _userData.LastName,
                MiddleName = _userData.MiddleName
            };
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenUserDataIsNull()
        {
            UserData userData = null;

            Assert.IsNull(_mapper.Map(userData));
        }

        [Test]
        public void ShouldMapUserSuccessfully()
        {
            var user = _mapper.Map(_userData);

            SerializerAssert.AreEqual(_expectedUser, user);
        }
    }
}
