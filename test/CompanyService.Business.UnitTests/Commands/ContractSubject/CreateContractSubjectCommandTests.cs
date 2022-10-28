using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.CompanyService.Validation.ContractSubject.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.ContractSubject;

public class CreateContractSubjectCommandTests
{
  private AutoMocker _mocker;
  private ICreateContractSubjectCommand _command;
  private CreateContractSubjectRequest _request;
  private OperationResultResponse<Guid?> _successResponse;
  private OperationResultResponse<Guid?> _failureResponse;
  private DbContractSubject _dbContractSubject;

  private void Verifiable(
    Times accessValidatorCalls,
    Times responseCreatorCalls,
    Times validatorCalls,
    Times mapperCalls,
    Times contractSubjectRepositoryCalls)
  {
    _mocker.Verify<IAccessValidator, Task<bool>>(
      x => x.HasRightsAsync(It.IsAny<int[]>()),
      accessValidatorCalls);

    _mocker.Verify<IResponseCreator, OperationResultResponse<Guid?>>(
      x => x.CreateFailureResponse<Guid?>(It.IsAny<HttpStatusCode>(), It.IsAny<List<string>>()),
      responseCreatorCalls);

    _mocker.Verify<ICreateContractSubjectRequestValidator>(
      x => x.ValidateAsync(It.IsAny<CreateContractSubjectRequest>(), default),
      validatorCalls);

    _mocker.Verify<IDbContractSubjectMapper, DbContractSubject>(
      x => x.Map(It.IsAny<CreateContractSubjectRequest>()),
      mapperCalls);

    _mocker.Verify<IContractSubjectRepository>(
      x => x.CreateAsync(It.IsAny<DbContractSubject>()),
      contractSubjectRepositoryCalls);

    _mocker.Resolvers.Clear();
  }

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    _request = new()
    {
      Name = "Name",
      Description = "Description"
    };

    _dbContractSubject = new()
    {
      Id = Guid.NewGuid(),
      Name = "Name",
      Description = "Description",
      IsActive = true,
      CreatedBy = Guid.NewGuid(),
      CreatedAtUtc = DateTime.UtcNow,
      ModifiedBy = Guid.NewGuid(),
      ModifiedAtUtc = DateTime.UtcNow
    };

    _failureResponse = new()
    {
      Body = Guid.Empty,
      Errors = new() { "Error" }
    };

    _successResponse = new(body: _dbContractSubject.Id);
  }

  [SetUp]
  public void SetUp()
  {
    _mocker = new AutoMocker();

    _mocker
      .Setup<IAccessValidator, Task<bool>>(x => x.HasRightsAsync(It.IsAny<int[]>()))
      .ReturnsAsync(true);

    _mocker
      .Setup<IResponseCreator, OperationResultResponse<Guid?>>(
        x => x.CreateFailureResponse<Guid?>(It.IsAny<HttpStatusCode>(), It.IsAny<List<string>>()))
      .Returns(_failureResponse);

    _mocker
      .Setup<ICreateContractSubjectRequestValidator, Task<ValidationResult>>(
        x => x.ValidateAsync(It.IsAny<CreateContractSubjectRequest>(), default))
      .ReturnsAsync(new ValidationResult() { });

    _mocker
      .Setup<IDbContractSubjectMapper, DbContractSubject>(x => x.Map(It.IsAny<CreateContractSubjectRequest>()))
      .Returns(_dbContractSubject);

    _mocker
      .Setup<IContractSubjectRepository>(x => x.CreateAsync(It.IsAny<DbContractSubject>()));

    _mocker
        .Setup<IHttpContextAccessor, int>(x => x.HttpContext.Response.StatusCode)
        .Returns(201);

    _command = _mocker.CreateInstance<CreateContractSubjectCommand>();
  }

  [Test]
  public async Task CreateContractSubjectSuccessTestAsync()
  {
    SerializerAssert.AreEqual(_successResponse, await _command.ExecuteAsync(_request));

    Verifiable(
      accessValidatorCalls: Times.Exactly(2),
      responseCreatorCalls: Times.Never(),
      validatorCalls: Times.Once(),
      mapperCalls: Times.Once(),
      contractSubjectRepositoryCalls: Times.Once());
  }

  [Test]
  public async Task NotEnoughRightTestAsync()
  {
    _mocker
      .Setup<IAccessValidator, Task<bool>>(x => x.HasRightsAsync(It.IsAny<int[]>()))
      .Returns(Task.FromResult(false));

    SerializerAssert.AreEqual(_failureResponse, await _command.ExecuteAsync(_request));

    Verifiable(
      accessValidatorCalls: Times.Once(),
      responseCreatorCalls: Times.Once(),
      validatorCalls: Times.Never(),
      mapperCalls: Times.Never(),
      contractSubjectRepositoryCalls: Times.Never());
  }

  [Test]
  public async Task ValidationFailureTestAsync()
  {
    _mocker
      .Setup<ICreateContractSubjectRequestValidator, Task<ValidationResult>>(
      x => x.ValidateAsync(It.IsAny<CreateContractSubjectRequest>(), default))
      .Returns(Task.FromResult(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("_", "Error") })));

    SerializerAssert.AreEqual(_failureResponse, await _command.ExecuteAsync(_request));

    Verifiable(
      accessValidatorCalls: Times.Exactly(2),
      responseCreatorCalls: Times.Once(),
      validatorCalls: Times.Once(),
      mapperCalls: Times.Never(),
      contractSubjectRepositoryCalls: Times.Never());
  }
}
