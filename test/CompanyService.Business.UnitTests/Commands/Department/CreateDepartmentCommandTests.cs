using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
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
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Department
{
  internal class CreateDepartmentCommandTests
  {
    private ICreateDepartmentCommand _command;
    private AutoMocker _autoMock;

    private CreateDepartmentRequest _request;
    private DbDepartment _dbDepartment;

    private readonly Guid _directorId = Guid.NewGuid();
    private readonly Guid _userId1 = Guid.NewGuid();
    private readonly Guid _userId2 = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

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
      _autoMock = new AutoMocker();

      IDictionary<object, object> _items = new Dictionary<object, object>();
      _items.Add("UserId", _userId);

      _command = _autoMock.CreateInstance<CreateDepartmentCommand>();

      _autoMock
        .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
        .Returns(_items);

      _autoMock
        .Setup<IHttpContextAccessor, int>(x => x.HttpContext.Response.StatusCode)
        .Returns(200);

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
        .Setup<IDepartmentRepository, Guid>(x => x.Create(_dbDepartment))
        .Returns(_dbDepartment.Id);

      _autoMock
        .Setup<IDepartmentRepository, bool>(x => x.DoesNameExist(_request.Name))
        .Returns(false);
    }

    [Test]
    public void ShouldReturnFailedResponseWhenUserIsNotAdminAndNotEnoughRights()
    {
      _autoMock
        .Setup<IAccessValidator, bool>(x => x.IsAdmin(null))
        .Returns(false);

      _autoMock
        .Setup<IAccessValidator, bool>(x => x.HasRights(Rights.AddEditRemoveDepartments))
        .Returns(false);

      Assert.AreEqual(OperationResultStatusType.Failed, _command.Execute(_request).Status);

      _autoMock.Verify<ICompanyRepository, DbCompany>(
        x => x.Get(It.IsAny<GetCompanyFilter>()),
        Times.Never);

      _autoMock.Verify<IDepartmentRepository, Guid>(
        x => x.Create(It.IsAny<DbDepartment>()),
        Times.Never);

      _autoMock.Verify<ICreateDepartmentRequestValidator, ValidationResult>(
        x => x.Validate(It.IsAny<IValidationContext>()),
        Times.Never);

      _autoMock.Verify<IDepartmentRepository, bool>(
        x => x.DoesNameExist(It.IsAny<string>()),
        Times.Never);

      _autoMock.Verify<IDbDepartmentMapper, DbDepartment>(
        x => x.Map(It.IsAny<CreateDepartmentRequest>(), It.IsAny<Guid>()),
        Times.Never);
    }

    [Test]
    public void ShouldThrowValidationException()
    {
      _autoMock
        .Setup<ICreateDepartmentRequestValidator, bool>(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
        .Returns(false);

      Assert.AreEqual(OperationResultStatusType.Failed, _command.Execute(_request).Status);

      _autoMock.Verify<ICompanyRepository, DbCompany>(
        x => x.Get(It.IsAny<GetCompanyFilter>()),
        Times.Never);

      _autoMock.Verify<IDepartmentRepository, Guid>(
        x => x.Create(It.IsAny<DbDepartment>()),
        Times.Never);

      _autoMock.Verify<ICreateDepartmentRequestValidator, ValidationResult>(
        x => x.Validate(It.IsAny<IValidationContext>()),
        Times.Once);

      _autoMock.Verify<IDepartmentRepository, bool>(
        x => x.DoesNameExist(It.IsAny<string>()),
        Times.Never);

      _autoMock.Verify<IDbDepartmentMapper, DbDepartment>(
        x => x.Map(It.IsAny<CreateDepartmentRequest>(), It.IsAny<Guid>()),
        Times.Never);
    }

    [Test]
    public void ShouldThrowExceptionWhenCompanyDoesNotExist()
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
        x => x.Create(It.IsAny<DbDepartment>()),
        Times.Never);

      _autoMock.Verify<ICreateDepartmentRequestValidator, ValidationResult>(
        x => x.Validate(It.IsAny<IValidationContext>()),
        Times.Once);

      _autoMock.Verify<IDepartmentRepository, bool>(
        x => x.DoesNameExist(It.IsAny<string>()),
        Times.Never);

      _autoMock.Verify<IDbDepartmentMapper, DbDepartment>(
        x => x.Map(It.IsAny<CreateDepartmentRequest>(), It.IsAny<Guid>()),
        Times.Never);
    }

    [Test]
    public void ShouldThrowExceptionWhenDepartmentNameAlreadyExists()
    {
      var expected = new OperationResultResponse<Guid>
      {
        Status = OperationResultStatusType.Failed,
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
        x => x.Create(It.IsAny<DbDepartment>()),
        Times.Never);

      _autoMock.Verify<ICreateDepartmentRequestValidator, ValidationResult>(
        x => x.Validate(It.IsAny<IValidationContext>()),
        Times.Once);

      _autoMock.Verify<IDepartmentRepository, bool>(
        x => x.DoesNameExist(It.IsAny<string>()),
        Times.Once);

      _autoMock.Verify<IDbDepartmentMapper, DbDepartment>(
        x => x.Map(It.IsAny<CreateDepartmentRequest>(), It.IsAny<Guid>()),
        Times.Never);
    }

    [Test]
    public void ShouldRemoveUserWhenUserAlreadyExists()
    {
      _request = GenerateRequest(null, _userId1, _userId2);

      Assert.DoesNotThrow(() => _command.Execute(_request));

      _autoMock.Verify<IDepartmentUserRepository>(
        x => x.Remove(_userId1, _userId),
        Times.Once);

      _autoMock.Verify<IDepartmentUserRepository>(
        x => x.Remove(_userId2, _userId),
        Times.Once);

      _autoMock.Verify<IDepartmentUserRepository>(
        x => x.Remove(It.IsAny<Guid>(), _userId),
        Times.Exactly(2));
    }

    [Test]
    public void ShouldRemoveDerictorrWhenDirectorAlreadyExists()
    {
      _request = GenerateRequest(_directorId);

      Assert.DoesNotThrow(() => _command.Execute(_request));

      _autoMock.Verify<IDepartmentUserRepository>(
        x => x.Remove(_directorId, _userId),
        Times.Once);

      _autoMock.Verify<IDepartmentUserRepository>(
        x => x.Remove(It.IsAny<Guid>(), _userId),
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
        x => x.Create(It.IsAny<DbDepartment>()),
        Times.Once);

      _autoMock.Verify<ICreateDepartmentRequestValidator, ValidationResult>(
        x => x.Validate(It.IsAny<IValidationContext>()),
        Times.Once);

      _autoMock.Verify<IDepartmentRepository, bool>(
        x => x.DoesNameExist(It.IsAny<string>()),
        Times.Once);

      _autoMock.Verify<IDbDepartmentMapper, DbDepartment>(
        x => x.Map(It.IsAny<CreateDepartmentRequest>(), It.IsAny<Guid>()),
        Times.Once);
    }
  }
}
