using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject.Filters;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.ContractSubject;

public class FindContractSubjectCommandTests
{
  private AutoMocker _mocker;
  private IFindContractSubjectsCommand _command;
  private List<DbContractSubject> _dbContractSubjects;
  private DbContractSubject _dbContractSubject;
  private ContractSubjectInfo _contractSubjectInfo;
  private FindResultResponse<ContractSubjectInfo> _response;
  private List<ContractSubjectInfo> _contractSubjectInfos;
  private FindContractSubjectFilter _filter;

  private void Verifiable(
    Times contractSubjectRepositoryCalls,
    Times mapperCalls)
  {
    _mocker.Verify<IContractSubjectRepository, Task<(List<DbContractSubject>, int)>>(
      x => x.FindAsync(It.IsAny<FindContractSubjectFilter>()),
      contractSubjectRepositoryCalls);

    _mocker.Verify<IContractSubjectInfoMapper, ContractSubjectInfo>(
      x => x.Map(It.IsAny<DbContractSubject>()),
      mapperCalls);

    _mocker.Resolvers.Clear();
  }

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
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

    _dbContractSubjects = new() { _dbContractSubject };

    _contractSubjectInfo = new()
    {
      Id = _dbContractSubject.Id,
      Name = _dbContractSubject.Name,
      Description = _dbContractSubject.Description,
      IsActive = _dbContractSubject.IsActive
    };

    _contractSubjectInfos = new() { _contractSubjectInfo };
  }

  [SetUp]
  public void SetUp()
  {
    _response = new(
      body: _contractSubjectInfos,
      totalCount: 1,
      errors: null);

    _filter = new()
    {
      SkipCount = 0,
      TakeCount = 1,
      IsActive = true
    };

    _mocker = new AutoMocker();

    _mocker
      .Setup<IContractSubjectRepository, Task<(List<DbContractSubject>, int)>>(
        x => x.FindAsync(It.IsAny<FindContractSubjectFilter>()))
      .Returns(Task.FromResult((_dbContractSubjects, 1)));

    _mocker
      .Setup<IContractSubjectInfoMapper, ContractSubjectInfo>(
        x => x.Map(It.IsAny<DbContractSubject>()))
      .Returns(_contractSubjectInfo);

    _command = _mocker.CreateInstance<FindContractSubjectsCommand>();
  }

  [Test]
  public void FindContractSubjectsSuccesTest()
  {
    SerializerAssert.AreEqual(_response, _command.ExecuteAsync(_filter).Result);

    Verifiable(
      contractSubjectRepositoryCalls: Times.Once(),
      mapperCalls: Times.Once());
  }

  [Test]
  public void FilterIsNullTest()
  {
    _response = new(
      body: new List<ContractSubjectInfo>(),
      totalCount: 1,
      errors: null);

    _filter = new()
    {
      SkipCount = 0,
      TakeCount = 0,
      IsActive = null
    };

    _mocker
      .Setup<IContractSubjectRepository, Task<(List<DbContractSubject>, int)>>(
        x => x.FindAsync(It.IsAny<FindContractSubjectFilter>()))
      .Returns(Task.FromResult((new List<DbContractSubject>(), 1)));

    SerializerAssert.AreEqual(_response, _command.ExecuteAsync(_filter).Result);

    Verifiable(
      contractSubjectRepositoryCalls: Times.Once(),
      mapperCalls: Times.Never());
  }
}
