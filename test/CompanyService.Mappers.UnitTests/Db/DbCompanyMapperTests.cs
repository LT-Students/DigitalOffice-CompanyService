using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class DbCompanyMapperTests
    {
        private IDbCompanyMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new DbCompanyMapper();
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            CreateCompanyRequest request = new CreateCompanyRequest
            {
                Name = "Name",
                Description = "Description",
                Logo = new AddImageRequest(),
                Tagline = "Tagline",
                SiteUrl = "siteurl"
            };

            Guid imageId = Guid.NewGuid();

            DbCompany expected = new()
            {
                Name = request.Name,
                Description = request.Description,
                LogoId = imageId,
                Tagline = request.Tagline,
                CreatedAt = DateTime.UtcNow,
                SiteUrl = request.SiteUrl
            };

            var response = _mapper.Map(request, imageId);

            Assert.AreNotEqual(Guid.Empty, response.Id);
            Assert.AreEqual(expected.Name, response.Name);
            Assert.AreEqual(expected.Description, response.Description);
            Assert.AreEqual(expected.LogoId, response.LogoId);
            Assert.AreEqual(expected.Tagline, response.Tagline);
            Assert.AreEqual(expected.SiteUrl, response.SiteUrl);
            Assert.IsTrue(response.IsActive);
            Assert.LessOrEqual(expected.CreatedAt, response.CreatedAt);
        }
    }
}
