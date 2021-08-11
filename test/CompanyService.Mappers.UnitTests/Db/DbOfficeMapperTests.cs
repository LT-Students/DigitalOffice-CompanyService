using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class DbOfficeMapperTests
    {
        private IDbOfficeMapper _mapper;
        private AutoMocker _mocker;

        private Guid _userId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _mapper = _mocker.CreateInstance<DbOfficeMapper>();

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _mocker
                .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
                .Returns(_items);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, Guid.NewGuid()));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            CreateOfficeRequest request = new()
            {
                Name = "Name",
                City = "City",
                Address = "Address"
            };

            DbOffice expected = new()
            {
                Name = request.Name,
                City = "City",
                Address = "Address",
                CompanyId = Guid.NewGuid(),
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = _userId
            };

            var response = _mapper.Map(request, expected.CompanyId);

            Assert.AreNotEqual(Guid.Empty, response.Id);
            Assert.AreEqual(expected.Name, response.Name);
            Assert.AreEqual(expected.City, response.City);
            Assert.AreEqual(expected.Address, response.Address);
            Assert.AreEqual(expected.CompanyId, response.CompanyId);
            Assert.IsTrue(response.IsActive);
            Assert.LessOrEqual(expected.CreatedAtUtc, response.CreatedAtUtc);
            Assert.LessOrEqual(expected.CreatedBy, response.CreatedBy);
        }
    }
}
