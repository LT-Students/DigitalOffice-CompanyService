using LT.DigitalOffice.CompanyService.Mappers;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Mappers
{
    public class CompanyMapperTests
    {
        private IMapper<DbCompany, Company> dbCompanyMapper;
        private DbCompany dbCompany;
        private Company expectedCompany;

        private IMapper<AddCompanyRequest, DbCompany> addCompanyRequestMapper;
        private AddCompanyRequest addCompanyRequest;
        private DbCompany expectedDbCompanyWithoutId;

        private IMapper<EditCompanyRequest, DbCompany> changeCompanyRequestMapper;
        private EditCompanyRequest changeCompanyRequest;
        private DbCompany expectedDbCompanyAfterChange;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            const string name = "Lanit-Tercom";

            dbCompanyMapper = new CompanyMapper();
            dbCompany = new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = name,
                IsActive = true
            };
            expectedCompany = new Company
            {
                Id = dbCompany.Id,
                Name = name,
                IsActive = dbCompany.IsActive
            };

            addCompanyRequestMapper = new CompanyMapper();
            addCompanyRequest = new AddCompanyRequest
            {
                Name = name
            };
            expectedDbCompanyWithoutId = new DbCompany
            {
                Name = name,
                IsActive = true
            };

            changeCompanyRequestMapper = new CompanyMapper();
            changeCompanyRequest = new EditCompanyRequest
            {
                CompanyId = dbCompany.Id,
                Name = dbCompany.Name + "abracadabra",
                IsActive = !dbCompany.IsActive
            };
            expectedDbCompanyAfterChange = new DbCompany
            {
                Id = changeCompanyRequest.CompanyId,
                Name = changeCompanyRequest.Name,
                IsActive = changeCompanyRequest.IsActive
            };
        }

        #region DbCompany to Company
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDbCompanyIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => dbCompanyMapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenDbCompanyMapped()
        {
            var company = dbCompanyMapper.Map(dbCompany);
            SerializerAssert.AreEqual(expectedCompany, company);
        }
        #endregion

        #region AddCompanyRequest to DbCompany
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenAddCompanyRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => addCompanyRequestMapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenAddCompanyRequestIsMapped()
        {
            var addedDbCompany = addCompanyRequestMapper.Map(addCompanyRequest);
            expectedDbCompanyWithoutId.Id = addedDbCompany.Id;

            Assert.IsInstanceOf<Guid>(addedDbCompany.Id);
            SerializerAssert.AreEqual(expectedDbCompanyWithoutId, addedDbCompany);
        }
        #endregion

        #region EditCompanyRequest to DbCompany
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenEditCompanyRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => changeCompanyRequestMapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenEditCompanyRequestIsMapped()
        {
            var changedDbCompany = changeCompanyRequestMapper.Map(changeCompanyRequest);

            SerializerAssert.AreEqual(expectedDbCompanyAfterChange, changedDbCompany);
        }
        #endregion
    }
}
