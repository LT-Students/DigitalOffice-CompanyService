using System;
using System.Collections.Generic;
using LT.DigitalOffice.CompanyService.Business.Commands.Position;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
{
  public class FindPositionsCommandTests
  {
    private IFindPositionsCommand _command;
    private AutoMocker _autoMock;

    private List<PositionInfo> _positionsList;
    private List<DbPosition> _dbPositionsList;
    private FindResultResponse<PositionInfo> _findResultResponse;

    [SetUp]
    public void SetUp()
    {
      _autoMock = new AutoMocker();

      var dbPosition = new DbPosition
      {
        Name = "Position",
        Description = "Description"
      };
      _dbPositionsList = new List<DbPosition> { dbPosition };

      var position = new PositionInfo
      {
        Name = dbPosition.Name,
        Description = dbPosition.Description
      };
      _positionsList = new List<PositionInfo> { position };

      int value = 1;
      _autoMock
          .Setup<IPositionRepository, List<DbPosition>>(x => x.Find(0, 15, true, out value))
          .Returns(_dbPositionsList);

      _autoMock
          .Setup<IPositionInfoMapper, PositionInfo>(x => x.Map(dbPosition))
          .Returns(position);

      _autoMock
        .Setup<IHttpContextAccessor, int>(a => a.HttpContext.Response.StatusCode)
        .Returns(200);

      _command = _autoMock.CreateInstance<FindPositionsCommand>();

      _findResultResponse = new()
      {
        Status = OperationResultStatusType.FullSuccess,
        Body = _positionsList,
        TotalCount = value
      };
    }

    [Test]
    public void ShouldReturnPositionsListSuccessfully()
    {
      var result = _command.Execute(0, 15, true);

      Assert.DoesNotThrow(() => _command.Execute(0, 15, true));
      SerializerAssert.AreEqual(_findResultResponse, result);
    }

    [Test]
    public void ShouldThrowExceptionWhenRepositoryThrowsException()
    {
      int value;
      _autoMock
          .Setup<IPositionRepository, List<DbPosition>>(x => x.Find(0, 15, true, out value))
          .Throws(new Exception());
      Assert.Throws<Exception>(() => _command.Execute(0, 15, true));

      _autoMock.Verify<IPositionRepository, List<DbPosition>>(
          x => x.Find(0, 15, true, out value),
          Times.Once());

      _autoMock.Verify<IPositionInfoMapper, PositionInfo>(
          x => x.Map(It.IsAny<DbPosition>()),
          Times.Never());
    }

    [Test]
    public void ShouldThrowExceptionWhenMapperThrowsException()
    {
      _autoMock
          .Setup<IPositionInfoMapper, PositionInfo>(x => x.Map(It.IsAny<DbPosition>()))
          .Throws(new Exception())
          .Verifiable();

      int value;
      _autoMock
          .Setup<IPositionRepository, List<DbPosition>>(x => x.Find(0, 15, true, out value))
          .Returns(_dbPositionsList)
          .Verifiable();

      Assert.Throws<Exception>(() => _command.Execute(0, 15, true));

      _autoMock.Verify<IPositionInfoMapper, PositionInfo>(
          x => x.Map(It.IsAny<DbPosition>()),
          Times.Once());

      _autoMock.Verify<IPositionRepository, List<DbPosition>>(
          x => x.Find(0, 15, true, out value),
          Times.Once());
    }
  }
}
