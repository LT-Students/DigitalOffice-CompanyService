//using LT.DigitalOffice.CompanyService.Mappers.Db;
//using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
//using LT.DigitalOffice.CompanyService.Models.Db;
//using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
//using NUnit.Framework;
//using System;

//namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
//{
//    public class DbOfficeMapperTests
//    {
//        private IDbOfficeMapper _mapper;

//        [SetUp]
//        public void SetUp()
//        {
//            _mapper = new DbOfficeMapper();
//        }

//        [Test]
//        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, Guid.NewGuid()));
//        }

//        [Test]
//        public void ShouldMapSuccessfuly()
//        {
//            CreateOfficeRequest request = new()
//            {
//                Name = "Name",
//                City = "City",
//                Address = "Address"
//            };

//            DbOffice expected = new()
//            {
//                Name = request.Name,
//                City = "City",
//                Address = "Address",
//                CompanyId = Guid.NewGuid(),
//                CreatedAt = DateTime.UtcNow
//            };

//            var response = _mapper.Map(request, expected.CompanyId);

//            Assert.AreNotEqual(Guid.Empty, response.Id);
//            Assert.AreEqual(expected.Name, response.Name);
//            Assert.AreEqual(expected.City, response.City);
//            Assert.AreEqual(expected.Address, response.Address);
//            Assert.AreEqual(expected.CompanyId, response.CompanyId);
//            Assert.IsTrue(response.IsActive);
//            Assert.LessOrEqual(expected.CreatedAt, response.CreatedAt);
//        }
//    }
//}
