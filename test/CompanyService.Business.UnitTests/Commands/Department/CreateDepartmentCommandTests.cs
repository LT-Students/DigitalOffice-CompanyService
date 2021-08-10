using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Commands.Department;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Responses;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Department
{
    internal class CreateDepartmentCommandTests
    {
        private ICreateDepartmentCommand _command;
        private AutoMocker _autoMock;

        private CreateDepartmentRequest _request;
        private DbDepartment _dbDepartment;

        private readonly Guid _directorId = new Guid("528E413B-7A3D-4CA9-851A-B9DA5840B216");
        private readonly Guid _userId1 = new Guid("10C2C56D-28E7-4134-B77A-63CEA1AD0307");
        private readonly Guid _userId2 = new Guid("8C63446D-6D06-4A18-9E7E-E032AB869E1E");

        private Guid _companyId;

        private CreateDepartmentRequest GenerateRequest(
            Guid? directorUserId,
            params Guid[] userIds) 
        {
            return new()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = directorUserId,
                Users = userIds
            };
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _autoMock = new AutoMocker();
            _command = _autoMock.CreateInstance<CreateDepartmentCommand>();

            _companyId = Guid.NewGuid();

            _request = GenerateRequest(_directorId, _userId1, _userId2);

            _dbDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                CompanyId = _companyId,
                Name = _request.Name,
                Description = _request.Description,
                IsActive = true,
                Users = new List<DbDepartmentUser>()
            };
        }

        [SetUp]
        public void SetUp()
        {
            _autoMock.GetMock<IDepartmentRepository>().Reset();
            _autoMock.GetMock<ICompanyRepository>().Reset();
            _autoMock.GetMock<IDepartmentUserRepository>().Reset();
            _autoMock.GetMock<ICreateDepartmentRequestValidator>().Reset();
            _autoMock.GetMock<IDbDepartmentMapper>().Reset();
            _autoMock.GetMock<IAccessValidator>().Reset();

            _autoMock
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(true);

            _autoMock
                .Setup<ICreateDepartmentRequestValidator, bool>(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(true);

            _autoMock
                .Setup<ICompanyRepository, DbCompany>(x => x.Get(null))
                .Returns(new DbCompany { Id = _companyId });

            _autoMock
                .Setup<IDbDepartmentMapper, DbDepartment>(x => x.Map(_request, _companyId))
                .Returns(_dbDepartment);

            _autoMock
                .Setup<IDepartmentRepository, Guid>(x => x.CreateDepartment(_dbDepartment))
                .Returns(_dbDepartment.Id);

            _autoMock
                .Setup<IDepartmentRepository, bool>(x => x.DoesNameExist(_request.Name))
                .Returns(false);
        }

        [Test]
        public void ShouldThrowForbiddenExceptionWhenUserIsNotAdminAndNotEnoughRights()
        {
            _autoMock
                .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
                .Returns(false);

            _autoMock
                .Setup<IAccessValidator, bool>(x => x.HasRights(Rights.AddEditRemoveDepartments))
                .Returns(false);

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository, DbCompany>(
                x => x.Get(It.IsAny<GetCompanyFilter>()), 
                Times.Never);

            _autoMock.Verify<IDepartmentRepository, Guid>(
                x => x.CreateDepartment(It.IsAny<DbDepartment>()), 
                Times.Never);
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _autoMock
                .Setup<ICreateDepartmentRequestValidator, bool>
                (x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                .Returns(false);

            Assert.Throws<ValidationException>(() => _command.Execute(_request));

            _autoMock.Verify<ICompanyRepository, DbCompany>(
                x => x.Get(It.IsAny<GetCompanyFilter>()), 
                Times.Never);

            _autoMock.Verify<IDepartmentRepository, Guid>(
                x => x.CreateDepartment(It.IsAny<DbDepartment>()), 
                Times.Never);
        }

        [Test]
        public void ShouldThrowExcWhenCompanyDoesntExist()
        {
            var expected = new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.Failed,
                Errors = { "Company does not exist, please create company." }
            };

            _autoMock
                .Setup<ICompanyRepository, DbCompany>(x => x.Get(null))
                .Returns((DbCompany)null);

            Assert.AreEqual(expected.Errors, _command.Execute(_request).Errors);
            _autoMock.Verify<ICompanyRepository, DbCompany>(
                x => x.Get(It.IsAny<GetCompanyFilter>()), 
                Times.Once);

            _autoMock.Verify<IDepartmentRepository, Guid>(
                x => x.CreateDepartment(It.IsAny<DbDepartment>()), 
                Times.Never);
        }

        [Test]
        public void ShouldThrowExcWhenDepartmentNameAlreadyExists()
        {
            var expected = new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.Conflict,
                Errors = { "The department name already exists" }
            };

            _autoMock
                .Setup<IDepartmentRepository, bool>(x => x.DoesNameExist(_request.Name))
                .Returns(true);

            Assert.AreEqual(expected.Errors, _command.Execute(_request).Errors);

            _autoMock.Verify<ICompanyRepository, DbCompany>(
                x => x.Get(It.IsAny<GetCompanyFilter>()), 
                Times.Once);

            _autoMock.Verify<IDepartmentRepository, Guid>(
                x => x.CreateDepartment(It.IsAny<DbDepartment>()), 
                Times.Never);
        }

        [Test]
        public void RemoveUserTest()
        {
            _request = GenerateRequest(null, _userId1, _userId2);

            Assert.DoesNotThrow(() => _command.Execute(_request));

            _autoMock.Verify<IDepartmentUserRepository>(
                x => x.Remove(_userId1), 
                Times.Once);

            _autoMock.Verify<IDepartmentUserRepository>(
                x => x.Remove(_userId2), 
                Times.Once);

            _autoMock.Verify<IDepartmentUserRepository>(
                x => x.Remove(It.IsAny<Guid>()),
                Times.Exactly(2));
        }

        [Test]
        public void RemoveWDirectorUserTest()
        {
            _request = GenerateRequest(_directorId);

            Assert.DoesNotThrow(() => _command.Execute(_request));

            _autoMock.Verify<IDepartmentUserRepository>(
                x => x.Remove(_directorId), 
                Times.Once);

            _autoMock.Verify<IDepartmentUserRepository>(
            x => x.Remove(It.IsAny<Guid>()),
            Times.Exactly(1));
        }

        [Test]
        public void ShouldAddNewDepartmentSuccessfully()
        {
            var expected = new OperationResultResponse<Guid>
            {
                Body = _dbDepartment.Id,
                Status = OperationResultStatusType.FullSuccess,
            };

            Assert.AreEqual(expected.Body, _command.Execute(_request).Body);

            _autoMock.Verify<ICompanyRepository, DbCompany>(
                x => x.Get(It.IsAny<GetCompanyFilter>()), 
                Times.Once);

            _autoMock.Verify<IDepartmentRepository, Guid>(
                x => x.CreateDepartment(It.IsAny<DbDepartment>()), 
                Times.Once);
        }
    }
}