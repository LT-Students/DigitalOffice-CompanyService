using LT.DigitalOffice.CompanyService.Business.Commands.Department;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Department
{
    internal class CreateDepartmentCommandTests
    {
        private ICreateDepartmentCommand _command;
        private Mock<IDepartmentRepository> _repositoryMock;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<INewDepartmentRequestValidator> _validatorMock;
        private Mock<IDbDepartmentMapper> _mapperMock;

        private CreateDepartmentRequest _request;
        private DbDepartment _dbDepartment;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _accessValidatorMock = new Mock<IAccessValidator>();
            _validatorMock = new Mock<INewDepartmentRequestValidator>();
            _mapperMock = new Mock<IDbDepartmentMapper>();

            _command = new CreateDepartmentCommand(
                _repositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _accessValidatorMock.Object);

            var newDepartment = new BaseDepartmentInfo()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid()
            };

            var newUsers = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _request = new CreateDepartmentRequest
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

            foreach (var userId in newUsers)
            {
                _dbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
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

            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(true);
        }

        [Test]
        public void ShouldThrowForbiddenExceptionWhenUserIsNotAdminAndNotEnoughRights()
        {
            _accessValidatorMock
                .Setup(x => x.IsAdmin(null))
                .Returns(false);

            _accessValidatorMock
                .Setup(x => x.HasRights(Rights.AddEditRemoveDepartments))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _mapperMock.Verify(x => x.Map(_request), Times.Never);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDepartmentRequestIsNull()
        {
            _mapperMock
                .Setup(x => x.Map(_request))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(_request));
            _repositoryMock.Verify(x => x.CreateDepartment(_dbDepartment), Times.Never);
        }

        [Test]
        public void ShouldAddNewDepartmentSuccessfully()
        {
            _validatorMock
                .Setup(x => x.Validate(It.IsAny<CreateDepartmentRequest>()).IsValid)
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
