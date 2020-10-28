using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data.UnitTests
{
    internal class PositionRepositoryTests
    {
        private IDataProvider provider;
        private IPositionRepository repository;

        private DbPosition dbPosition;
        private Guid positionId;
        private DbPosition newPosition;
        private DbPosition dbPositionToAdd;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<CompanyServiceDbContext>()
                .UseInMemoryDatabase("InMemoryDatabase")
                .Options;

            provider = new CompanyServiceDbContext(dbOptions);

            repository = new PositionRepository(provider);
        }

        [SetUp]
        public void SetUp()
        {
            positionId = Guid.NewGuid();

            dbPosition = new DbPosition
            {
                Id = positionId,
                Description = "Description",
                Name = "Name"
            };
            dbPosition.Users.Add(new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                PositionId = positionId,
                UserId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                IsActive = true,
                StartTime = DateTime.Now.AddDays(-1)
            });

            provider.Positions.Add(dbPosition);
            provider.Save();

            dbPositionToAdd = new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description"
            };
        }

        [TearDown]
        public void CleanDb()
        {
            if (provider.IsInMemory())
            {
                provider.EnsureDeleted();
            }
        }

        #region GetPositionById
        [Test]
        public void ShouldThrowExceptionIfPositionDoesNotExist()
        {
            Assert.Throws<NotFoundException>(() => repository.GetPositionById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldReturnSimplePositionInfoSuccessfully()
        {
            var result = repository.GetPositionById(dbPosition.Id);

            var expected = new DbPosition
            {
                Id = dbPosition.Id,
                Name = dbPosition.Name,
                Description = dbPosition.Description
            };

            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Description, result.Description);
        }
        #endregion

        #region GetPositionsList
        [Test]
        public void GetPositionsListSuccessfully()
        {
            Assert.IsNotEmpty(repository.GetPositionsList());
        }
        #endregion

        #region GetUserPosition
        [Test]
        public void ShouldThrowExceptionWhenUserIdEmpty()
        {
            Assert.Throws<NotFoundException>(() => repository.GetUserPosition(Guid.Empty));
        }

        [Test]
        public void ShouldReturnUserPosition()
        {
            var userId = dbPosition.Users.First().UserId;

            var result = repository.GetUserPosition(userId);

            Assert.AreEqual(dbPosition.Id, result.Id);
            Assert.AreEqual(dbPosition.Description, result.Description);
            Assert.AreEqual(dbPosition.Name, result.Name);
            Assert.That(provider.Positions, Is.EquivalentTo(new[] { dbPosition }));
        }
        #endregion

        #region AddPosition
        [Test]
        public void ShouldAddNewPositionSuccessfully()
        {
            var expected = dbPositionToAdd.Id;

            var result = repository.AddPosition(dbPositionToAdd);

            Assert.AreEqual(expected, result);
            Assert.NotNull(provider.Positions.Find(dbPositionToAdd.Id));
        }
        #endregion

        #region EditPosition
        [Test]
        public void ShouldEditPositionSuccessfully()
        {
            newPosition = new DbPosition
            {
                Id = positionId,
                Name = "abracadabra",
                Description = "bluhbluh"
            };

            var result = repository.EditPosition(newPosition);

            DbPosition updatedPosition = provider.Positions.FirstOrDefault(p => p.Id == positionId);

            Assert.IsTrue(result);
            Assert.AreEqual(newPosition.Id, updatedPosition.Id);
            Assert.AreEqual(newPosition.Name, updatedPosition.Name);
            Assert.AreEqual(newPosition.Description, updatedPosition.Description);
        }
        #endregion

        #region DisablePositionById
        [Test]
        public void ShouldThrowExceptionIfPositionDoesNotExistWhileDisablingPosition()
        {
            Assert.Throws<NotFoundException>(() => repository.DisablePositionById(Guid.NewGuid()));
        }

        [Test]
        public void ShouldDisablePositionSuccessfully()
        {
            repository.DisablePositionById(positionId);

            Assert.IsFalse(provider.Positions.Find(positionId).IsActive);
        }

        [Test]
        public void ShouldThrowExceptionIfPositionIdNullWhileDisablingPosition()
        {
            Assert.Throws<NotFoundException>(() => repository.DisablePositionById(Guid.Empty));
        }
        #endregion
    }
}