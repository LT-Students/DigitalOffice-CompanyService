using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    internal class CreateDepartmentCommandTests
    {
        private Mock<IDepartmentRepository> _repositoryMock;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<IValidator<NewDepartmentRequest>> _validatorMock;
        private Mock<IDbDepartmentMapper> _mapperMock;

        private ICreateDepartmentCommand _command;
        private NewDepartmentRequest _request;
        private DbDepartment _dbDepartment;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _accessValidatorMock = new Mock<IAccessValidator>();
            _validatorMock = new Mock<IValidator<NewDepartmentRequest>>();
            _mapperMock = new Mock<IDbDepartmentMapper>();

            _command = new CreateDepartmentCommand(
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _accessValidatorMock.Object);

            var newDepartment = new Department()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid()
            };

            var newUsers = new List<DepartmentUser>
            {
                new DepartmentUser
                {
                    UserId = Guid.NewGuid(),
                    PositionId = Guid.NewGuid()
                },
                new DepartmentUser
                {
                    UserId = Guid.NewGuid(),
                    PositionId = Guid.NewGuid()
                }
            };

            _request = new NewDepartmentRequest
            {
                Info = newDepartment,
                Users = newUsers
            };

            _dbDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = newDepartment.Name,
                Description = newDepartment.Description,
                IsActive = true,
                Users = new List<DbDepartmentUser>()
            };

            foreach (var newUser in newUsers)
            {
                _dbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        Id = Guid.NewGuid(),
                        UserId = newUser.UserId,
                        PositionId = newUser.PositionId,
                        DepartmentId = _dbDepartment.Id,
                        StartTime = DateTime.UtcNow,
                        IsActive = true
                    });
            }
        }

        [SetUp]
        public void SetUp()
        {
            _repositoryMock.Reset();
            _accessValidatorMock.Reset();
            _validatorMock.Reset();
            _mapperMock.Reset();
        }

        [Test]
        public void ShouldThrowForbiddenExceptionWhenUserIsNotAdminAndNotEnoughRights()
        {
            int accessRightId = 4;

            _accessValidatorMock
                .Setup(x => x.IsAdmin())
                .Returns(false);

            _accessValidatorMock
                .Setup(x => x.HasRights(accessRightId))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _mapperMock.Verify(x => x.Map(_request), Times.Never);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDepartmentRequestIsNull()
        {
            _accessValidatorMock
                .Setup(x => x.IsAdmin())
                .Returns(true);

            _mapperMock
                .Setup(x => x.Map(_request))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_request));
            _repositoryMock.Verify(x => x.CreateDepartment(_dbDepartment), Times.Never);
        }

        [Test]
        public void ShouldAddNewDepartmentSuccessfully()
        {
            _accessValidatorMock
                .Setup(x => x.IsAdmin())
                .Returns(true);

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<NewDepartmentRequest>()).IsValid)
                .Returns(true);

            _mapperMock
                .Setup(x => x.Map(_request))
                .Returns(_dbDepartment);

            _repositoryMock
                .Setup(x => x.CreateDepartment(_dbDepartment))
                .Returns(_dbDepartment.Id);


            Assert.AreEqual(_dbDepartment.Id, _command.Execute(_request));
        }
    }
}
