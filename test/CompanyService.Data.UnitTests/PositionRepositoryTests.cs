using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    internal class PositionRepositoryTests
    {
        private IDataProvider _provider;
        private IPositionRepository _repository;
        private Mock<IHttpContextAccessor> _accessorMock;

        private Guid _userId;
        private DbPosition _dbPosition;
        private Guid _positionId;
        private DbPosition _dbPositionToAdd;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                .UseInMemoryDatabase("InMemoryDatabase")
                .Options;

            _userId = Guid.NewGuid();

            _accessorMock = new();
            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _accessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(_items);

            _provider = new CompanyServiceDbContext(dbOptions);

            _repository = new PositionRepository(_provider, _accessorMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _positionId = Guid.NewGuid();

            _dbPosition = new DbPosition
            {
                Id = _positionId,
                Description = "Description",
                Name = "Name"
            };
            _dbPosition.Users.Add(new DbPositionUser
            {
                Id = Guid.NewGuid(),
                PositionId = _positionId,
                UserId = Guid.NewGuid(),
                IsActive = true,
                CreatedAtUtc = DateTime.Now.AddDays(-1)
            });

            _provider.Positions.Add(_dbPosition);
            _provider.Save();

            _dbPositionToAdd = new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description"
            };
        }

        [TearDown]
        public void CleanDb()
        {
            if (_provider.IsInMemory())
            {
                _provider.EnsureDeleted();
            }
        }

        #region GetPosition
        [Test]
        public void ShouldReturnNullWhenPositionDoesNotExist()
        {
            Assert.IsNull(_repository.Get(Guid.NewGuid()));
        }

        [Test]
        public void ShouldReturnSimplePositionInfoSuccessfully()
        {
            var result = _repository.Get(_dbPosition.Id);

            var expected = new DbPosition
            {
                Id = _dbPosition.Id,
                Name = _dbPosition.Name,
                Description = _dbPosition.Description
            };

            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Description, result.Description);
        }
        #endregion

        #region FindPositions
        [Test]
        public void FindPositionsSuccessfully()
        {
            Assert.IsNotEmpty(_repository.Find(0, 2, true, out _));
        }
        #endregion

        #region AddPosition
        [Test]
        public void ShouldAddNewPositionSuccessfully()
        {
            var expected = _dbPositionToAdd.Id;

            var result = _repository.Create(_dbPositionToAdd);

            Assert.AreEqual(expected, result);
            Assert.NotNull(_provider.Positions.Find(_dbPositionToAdd.Id));
        }
        #endregion

        #region EditPosition
        #endregion
    }
}
