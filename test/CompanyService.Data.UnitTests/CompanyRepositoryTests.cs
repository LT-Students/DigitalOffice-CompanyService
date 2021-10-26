//using LT.DigitalOffice.CompanyService.Data.Interfaces;
//using LT.DigitalOffice.CompanyService.Data.Provider;
//using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
//using LT.DigitalOffice.CompanyService.Models.Db;
//using LT.DigitalOffice.Kernel.Exceptions.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace LT.DigitalOffice.CompanyService.Data.UnitTests
//{
//    public class CompanyRepositoryTests
//    {
//        private IDataProvider _provider;
//        private ICompanyRepository _repository;
//        private DbContextOptions<CompanyServiceDbContext> _dbOptions;
//        private Mock<IHttpContextAccessor> _accessorMock;
//        private Guid _userId;

//        [OneTimeSetUp]
//        public void OneTimeSetUp()
//        {
//            CreateInMemoryDb();

//            _userId = Guid.NewGuid();

//            _accessorMock = new();
//            IDictionary<object, object> _items = new Dictionary<object, object>();
//            _items.Add("UserId", _userId);

//            _accessorMock
//                .Setup(x => x.HttpContext.Items)
//                .Returns(_items);
//        }

//        public void CreateInMemoryDb()
//        {
//            _dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
//                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
//                .Options;
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            _provider = new CompanyServiceDbContext(_dbOptions);
//            _repository = new CompanyRepository(_provider, _accessorMock.Object);
//        }

//        [TearDown]
//        public void CleanInMemoryDb()
//        {
//            if (_provider.IsInMemory())
//            {
//                _provider.EnsureDeleted();
//            }
//        }

//        #region Add tests

//        [Test]
//        public void ShouldThrowArgumentNullExceptionWhenModelsToAddIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() => _repository.CreateAsync(null));
//        }

//        [Test]
//        public void ShouldAddSuccessfuly()
//        {
//            DbCompany company = new()
//            {
//                Id = Guid.NewGuid(),
//                CreatedAtUtc = DateTime.UtcNow,
//                IsActive = true,
//                Description = "Desc",
//                CompanyName = "Name",
//                PortalName = "PortalName",
//                Tagline = "tagline"
//            };

//            _repository.CreateAsync(company);

//            Assert.IsTrue(_provider.Companies.Contains(company));
//        }

//        [Test]
//        public void ShouldThrowBadRequestExceptionWhenCompanyExist()
//        {
//            DbCompany company1 = new();
//            DbCompany company2 = new();

//            _repository.CreateAsync(company1);

//            Assert.Throws<BadRequestException>(() => _repository.CreateAsync(company2));
//        }

//        #endregion

//        #region Get Tests

//        [Test]
//        public void ShouldGetCompanySuccessfuly()
//        {
//            DbCompany company = new DbCompany();

//            _provider.Companies.Add(company);
//            _provider.Save();

//            Assert.AreEqual(company, _repository.GetAsync(null));
//        }

//        [Test]
//        public void ShouldThrowExceptionWhetCompanyDoesNotExist()
//        {
//            Assert.IsNull(_repository.GetAsync(null));
//        }

//        #endregion
//    }
//}
