﻿using LT.DigitalOffice.Broker.Models;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class FindDepartmentsCommandTest
    {
        private IFindDepartmentsCommand _command;
        private Mock<IDepartmentRepository> _repositoryMock;
        private Mock<IDepartmentResponseMapper> _departmentMapperMock;
        private Mock<IUserMapper> _userMapperMock;

        private Mock<IRequestClient<IGetUsersDataRequest>> _requestClientMock;

        private Mock<IGetUsersDataResponse> _responseMock;
        private Mock<IOperationResult<IGetUsersDataResponse>> _operationResultMock;
        private Mock<Response<IOperationResult<IGetUsersDataResponse>>> _brokerResponseMock;

        private Guid _directorGuid;
        private Guid _workerGuid;
        private UserData _directorData;
        private UserData _workerData;
        private User _director;
        private User _worker;
        private List<DbDepartment> _dbDepartments;
        private List<DepartmentInfo> _expectedDepartments;
        private DepartmentsResponse _expectedResponse;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _departmentMapperMock = new Mock<IDepartmentResponseMapper>();
            _userMapperMock = new Mock<IUserMapper>();

            _directorGuid = Guid.NewGuid();
            _workerGuid = Guid.NewGuid();

            var directorDepartmentUser = new DbDepartmentUser { UserId = _directorGuid };
            var workerDepartmentUser = new DbDepartmentUser { UserId = _workerGuid };

            _dbDepartments = new List<DbDepartment>
            {
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    Description = "TestDescription",
                    DirectorUserId = _directorGuid,
                    IsActive = true,
                    Users = new List<DbDepartmentUser> { directorDepartmentUser, workerDepartmentUser }
                }
            };

            _repositoryMock
                .Setup(x => x.FindDepartments())
                .Returns(_dbDepartments)
                .Verifiable();

            _directorData = new UserData
            {
                Id = _directorGuid,
                IsActive = true,
                FirstName = "Spartak",
                LastName = "Ryabtsev",
                MiddleName = "Alexandrovich"
            };

            _workerData = new UserData
            {
                Id = _workerGuid,
                IsActive = true,
                FirstName = "Alexandr",
                LastName = "Ryabtsev",
                MiddleName = "Spartakovich"
            };

            BrokerSetUp();

            _director = new User
            {
                FirstName = _directorData.FirstName,
                LastName = _directorData.LastName,
                MiddleName = _directorData.MiddleName
            };

            _worker = new User
            {
                FirstName = _workerData.FirstName,
                LastName = _workerData.LastName,
                MiddleName = _workerData.MiddleName
            };

            _userMapperMock
                .Setup(x => x.Map(_directorData))
                .Returns(_director)
                .Verifiable();

            _userMapperMock
                .Setup(x => x.Map(_workerData))
                .Returns(_worker)
                .Verifiable();

            _expectedDepartments = new List<DepartmentInfo>
            {
                new DepartmentInfo
                {
                    Id = _dbDepartments.First().Id,
                    Name = _dbDepartments.First().Name,
                    Description = _dbDepartments.First().Description,
                    Director = _director,
                    Users = new List<User> { _director, _worker }
                }
            };

            _departmentMapperMock
                .Setup(x => x.Map(_dbDepartments.First(), _director, _expectedDepartments.First().Users))
                .Returns(_expectedDepartments.First())
                .Verifiable();

            _command = new FindDepartmentsCommand(_repositoryMock.Object, _departmentMapperMock.Object, _userMapperMock.Object, _requestClientMock.Object, null);

            _expectedResponse = new DepartmentsResponse
            {
                TotalCount = 1,
                Departments = _expectedDepartments,
                Errors = new()
            };
        }

        private void BrokerSetUp()
        {
            _responseMock = new Mock<IGetUsersDataResponse>();
            _responseMock
                .Setup(x => x.UsersData)
                .Returns(new List<UserData> { _directorData, _workerData });

            _operationResultMock = new Mock<IOperationResult<IGetUsersDataResponse>>();
            _operationResultMock
                .Setup(x => x.Body)
                .Returns(_responseMock.Object);
            _operationResultMock
                .Setup(x => x.IsSuccess)
                .Returns(true);
            _operationResultMock
                .Setup(x => x.Errors)
                .Returns(new List<string>());

            _brokerResponseMock = new Mock<Response<IOperationResult<IGetUsersDataResponse>>>();
            _brokerResponseMock
                .Setup(x => x.Message)
                .Returns(_operationResultMock.Object);

            _requestClientMock = new Mock<IRequestClient<IGetUsersDataRequest>>();
            _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default).Result)
                .Returns(_brokerResponseMock.Object)
                .Verifiable();
        }

        [Test]
        public void ShouldReturnDepartmentListSuccessfully()
        {
            SerializerAssert.AreEqual(_expectedResponse, _command.Execute());

            _repositoryMock.Verify(x => x.FindDepartments(), Times.Once);
            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Once);
            _departmentMapperMock.Verify(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<User>(), It.IsAny<List<User>>()), Times.Once);
            _userMapperMock.Verify(x => x.Map(It.IsAny<UserData>()), Times.Exactly(3));
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            _repositoryMock
                .Setup(repository => repository.FindDepartments())
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
            _repositoryMock.Verify(x => x.FindDepartments(), Times.Once);
            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Never);
            _departmentMapperMock.Verify(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<User>(), It.IsAny<List<User>>()), Times.Never);
            _userMapperMock.Verify(x => x.Map(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public void ShouldReturnDataWhenRequestClientThrowsException()
        {
            _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(
                    It.IsAny<object>(), default, default))
                .Throws(new Exception());

            _expectedDepartments.First().Director = null;
            _expectedDepartments.First().Users = null;

            _departmentMapperMock
                .Setup(x => x.Map(_dbDepartments.First(), null, new List<User>()))
                .Returns(_expectedDepartments.First());

            var result = _command.Execute();
            _expectedResponse.Errors = result.Errors;

            SerializerAssert.AreEqual(_expectedResponse, result);
            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Once);
            _departmentMapperMock.Verify(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<User>(), It.IsAny<List<User>>()), Times.Once);
            _userMapperMock.Verify(x => x.Map(It.IsAny<UserData>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserMapperThrowsException()
        {
            _userMapperMock
                .Setup(x => x.Map(_directorData))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
        }

        [Test]
        public void ShouldThrowExceptionWhenDepartmentMapperThrowsException()
        {
            _departmentMapperMock
                .Setup(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<User>(), It.IsAny<List<User>>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
        }
    }
}
