using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    internal class PositionRepositoryTests
    {
        private IDataProvider _provider;
        private IPositionRepository _repository;

        private DbPosition _dbPosition;
        private Guid _positionId;
        private DbPosition _newPosition;
        private DbPosition _dbPositionToAdd;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                .UseInMemoryDatabase("InMemoryDatabase")
                .Options;

            _provider = new CompanyServiceDbContext(dbOptions);

            _repository = new PositionRepository(_provider);
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
                StartTime = DateTime.Now.AddDays(-1)
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
        public void ShouldThrowExceptionIfPositionDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => _repository.GetPosition(Guid.NewGuid(), null));
        }

        [Test]
        public void ShouldReturnSimplePositionInfoSuccessfully()
        {
            var result = _repository.GetPosition(_dbPosition.Id, null);

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
            Assert.IsNotEmpty(_repository.FindPositions());
        }
        #endregion

        #region GetUserPosition

        [Test]
        public void ShouldReturnUserPosition()
        {
            var userId = _dbPosition.Users.First().UserId;

            var result = _repository.GetPosition(null, userId);

            Assert.AreEqual(_dbPosition.Id, result.Id);
            Assert.AreEqual(_dbPosition.Description, result.Description);
            Assert.AreEqual(_dbPosition.Name, result.Name);
            Assert.That(_provider.Positions, Is.EquivalentTo(new[] { _dbPosition }));
        }
        #endregion

        #region AddPosition
        [Test]
        public void ShouldAddNewPositionSuccessfully()
        {
            var expected = _dbPositionToAdd.Id;

            var result = _repository.CreatePosition(_dbPositionToAdd);

            Assert.AreEqual(expected, result);
            Assert.NotNull(_provider.Positions.Find(_dbPositionToAdd.Id));
        }
        #endregion

        #region EditPosition
        [Test]
        public void ShouldEditPositionSuccessfully()
        {
            _newPosition = new DbPosition
            {
                Id = _positionId,
                Name = "abracadabra",
                Description = "bluhbluh"
            };

            var result = _repository.EditPosition(_newPosition);

            DbPosition updatedPosition = _provider.Positions.FirstOrDefault(p => p.Id == _positionId);

            Assert.IsTrue(result);
            Assert.AreEqual(_newPosition.Id, updatedPosition.Id);
            Assert.AreEqual(_newPosition.Name, updatedPosition.Name);
            Assert.AreEqual(_newPosition.Description, updatedPosition.Description);
        }
        #endregion

        #region DisablePosition
        [Test]
        public void ShouldThrowExceptionIfPositionDoesNotExistWhileDisablingPosition()
        {
            Assert.Throws<NotFoundException>(() => _repository.DisablePosition(Guid.NewGuid()));
        }

        [Test]
        public void ShouldDisablePositionSuccessfully()
        {
            _repository.DisablePosition(_positionId);

            Assert.IsFalse(_provider.Positions.Find(_positionId).IsActive);
        }

        [Test]
        public void ShouldThrowExceptionIfPositionIdNullWhileDisablingPosition()
        {
            Assert.Throws<NotFoundException>(() => _repository.DisablePosition(Guid.Empty));
        }
        #endregion
    }
}