using LT.DigitalOffice.CompanyService.Database;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Repositories;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Repositories
{
    internal class PositionRepositoryTests
    {
        private CompanyServiceDbContext dbContext;
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
            dbContext = new CompanyServiceDbContext(dbOptions);

            repository = new PositionRepository(dbContext);
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

            dbContext.Positions.Add(dbPosition);
            dbContext.SaveChanges();

            dbPositionToAdd = new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description"
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (dbContext.Database.IsInMemory())
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        #region GetPositionById
        [Test]
        public void ShouldThrowExceptionIfPositionDoesNotExist()
        {
            Assert.Throws<Exception>(() => repository.GetPositionById(Guid.NewGuid()));
            Assert.AreEqual(dbContext.Positions, new List<DbPosition> { dbPosition });
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

            SerializerAssert.AreEqual(expected, result);
            Assert.AreEqual(dbContext.Positions, new List<DbPosition> { dbPosition });
        }
        #endregion

        #region GetPositionsList
        [Test]
        public void GetPositionsListSuccessfully()
        {
            var result = repository.GetPositionsList();

            var expected = new List<DbPosition> { dbPosition };

            Assert.DoesNotThrow(() => repository.GetPositionsList());
            SerializerAssert.AreEqual(expected, result);
            Assert.That(dbContext.Positions, Is.EqualTo(new List<DbPosition> { dbPosition }));
        }
        #endregion

        #region GetUserPosition
        [Test]
        public void ShouldThrowExceptionWhenUserIdEmpty()
        {
            Assert.Throws<Exception>(() => repository.GetUserPosition(Guid.Empty));
        }

        [Test]
        public void ShouldReturnUserPosition()
        {
            var dbCompanyUser = new DbCompanyUser
            {
                PositionId = positionId,
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                IsActive = true,
                StartTime = DateTime.Now.AddDays(-1)
            };
            var position = dbContext.Positions
                .Include(p => p.UserIds)
                .FirstOrDefault(p => p.Id == positionId);
            position.UserIds = new List<DbCompanyUser> { dbCompanyUser };
            dbContext.SaveChanges();

            var userId = dbPosition.UserIds.First().UserId;
            var result = repository.GetUserPosition(userId);
            Assert.AreEqual(dbPosition.Id, result.Id);
            Assert.AreEqual(dbPosition.Description, result.Description);
            Assert.AreEqual(dbPosition.Name, result.Name);
            Assert.That(dbContext.Positions, Is.EquivalentTo(new[] { dbPosition }));
        }
        #endregion

        #region AddPosition
        [Test]
        public void ShouldAddNewPositionSuccessfully()
        {
            var expected = dbPositionToAdd.Id;

            var result = repository.AddPosition(dbPositionToAdd);

            Assert.AreEqual(expected, result);
            Assert.NotNull(dbContext.Positions.Find(dbPositionToAdd.Id));
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
            DbPosition updatedPosition = dbContext.Positions.FirstOrDefault(p => p.Id == positionId);

            Assert.IsTrue(result);
            SerializerAssert.AreEqual(newPosition, updatedPosition);
            Assert.That(dbContext.Positions, Is.EquivalentTo(new List<DbPosition> { updatedPosition }));
        }
        #endregion

        #region DisablePositionById
        [Test]
        public void ShouldThrowExceptionIfPositionDoesNotExistWhileDisablingPosition()
        {
            Assert.Throws<Exception>(() => repository.DisablePositionById(Guid.NewGuid()));
            Assert.AreEqual(dbContext.Positions, new List<DbPosition> { dbPosition });
        }

        [Test]
        public void ShouldDisablePositionSuccessfully()
        {
            repository.DisablePositionById(positionId);

            Assert.IsTrue(dbContext.Positions.Find(positionId).IsActive == false);
            Assert.AreEqual(dbContext.Positions, new List<DbPosition> { dbPosition });
        }

        [Test]
        public void ShouldThrowExceptionIfPositionIdNullWhileDisablingPosition()
        {
            Assert.Throws<Exception>(() => repository.DisablePositionById(Guid.Empty));
            Assert.AreEqual(dbContext.Positions, new List<DbPosition> { dbPosition });
        }
        #endregion
    }
}