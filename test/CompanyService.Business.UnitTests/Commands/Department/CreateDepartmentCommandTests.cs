using LT.DigitalOffice.CompanyService.Business.Commands.Department;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
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
        private Mock<IDepartmentUserRepository> _userRepositoryMock;
        private Mock<ICompanyRepository> _companyRepositoryMock;
        private Mock<IAccessValidator> _accessValidatorMock;
        private Mock<ICreateDepartmentRequestValidator> _validatorMock;
        private Mock<IDbDepartmentMapper> _mapperMock;
        private Mock<IHttpContextAccessor> _accessorMock;

        private CreateDepartmentRequest _request;
        private DbDepartment _dbDepartment;

        private Guid _companyId;
        private Guid _userId = Guid.NewGuid();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _repositoryMock = new();
            _userRepositoryMock = new();
            _accessValidatorMock = new();
            _validatorMock = new();
            _mapperMock = new();
            _companyRepositoryMock = new();
            _accessorMock = new();

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _userId);

            _accessorMock
                .Setup(x => x.HttpContext.Items)
                .Returns(_items);

            _command = new CreateDepartmentCommand(
                _repositoryMock.Object,
                _companyRepositoryMock.Object,
                _userRepositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _accessValidatorMock.Object,
                _accessorMock.Object);

            _companyId = Guid.NewGuid();

            var newUsers = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _request = new CreateDepartmentRequest
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid(),
                Users = newUsers
            };

            _dbDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                CompanyId = _companyId,
                Name = _request.Name,
                Description = _request.Description,
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
                        CreatedAtUtc = DateTime.UtcNow,
                        Role = (int)DepartmentUserRole.Employee,
                        IsActive = true
                    });
            }

            _dbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        Id = Guid.NewGuid(),
                        UserId = _request.DirectorUserId.Value,
                        DepartmentId = _dbDepartment.Id,
                        CreatedAtUtc = DateTime.UtcNow,
                        Role = (int)DepartmentUserRole.Director,
                        IsActive = true
                    });
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

        /*[Test]
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
        }*/

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
