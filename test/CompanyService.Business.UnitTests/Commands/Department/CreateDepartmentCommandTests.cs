﻿using LT.DigitalOffice.CompanyService.Business.Commands.Department;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
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
        private Mock<ICompanyRepository> _companyRepositoryMock;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<ICreateDepartmentRequestValidator> _validatorMock;
        private Mock<IDbDepartmentMapper> _mapperMock;

        private CreateDepartmentRequest _request;
        private DbDepartment _dbDepartment;

        private Guid _companyId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _accessValidatorMock = new Mock<IAccessValidator>();
            _validatorMock = new Mock<ICreateDepartmentRequestValidator>();
            _mapperMock = new Mock<IDbDepartmentMapper>();
            _companyRepositoryMock = new();

            _command = new CreateDepartmentCommand(
                _repositoryMock.Object,
                _companyRepositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _accessValidatorMock.Object);

            _companyId = Guid.NewGuid();

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
                CompanyId = _companyId,
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
            _companyRepositoryMock.Reset();

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
            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
            _repositoryMock.Verify(x => x.CreateDepartment(It.IsAny<DbDepartment>()), Times.Never);
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDepartmentRequestIsNull()
        {

            _companyRepositoryMock
                .Setup(x => x.Get(null))
                .Returns(new DbCompany { Id = _companyId });

            _mapperMock
                .Setup(x => x.Map(null, _companyId))
                .Throws(new ArgumentNullException());

            Assert.Throws<ArgumentNullException>(() => _command.Execute(null));
            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
            _repositoryMock.Verify(x => x.CreateDepartment(_dbDepartment), Times.Never);
        }

        [Test]
        public void ShouldAddNewDepartmentSuccessfully()
        {
            var expected = new OperationResultResponse<Guid>
            {
                Body = _dbDepartment.Id,
                Status = OperationResultStatusType.FullSuccess,
            };

            _validatorMock
                .Setup(x => x.Validate(It.IsAny<CreateDepartmentRequest>()).IsValid)
                .Returns(true);

            _companyRepositoryMock
                .Setup(x => x.Get(null))
                .Returns(new DbCompany { Id = _companyId });

            _mapperMock
                .Setup(x => x.Map(_request, _companyId))
                .Returns(_dbDepartment);

            _repositoryMock
                .Setup(x => x.CreateDepartment(_dbDepartment))
                .Returns(_dbDepartment.Id);

            Assert.AreEqual(expected.Body, _command.Execute(_request).Body);
            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
            _repositoryMock.Verify(x => x.CreateDepartment(_dbDepartment), Times.Once);
        }
    }
}
