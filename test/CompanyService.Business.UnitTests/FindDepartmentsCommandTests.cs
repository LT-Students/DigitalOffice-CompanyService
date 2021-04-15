using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.UserService.Models.Broker.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private List<DepartmentResponse> _expectedDepartments;

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
                .Returns(_dbDepartments);

            _directorData = new UserData
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                FirstName = "Spartak",
                LastName = "Ryabtsev",
                MiddleName = "Alexandrovich"
            };

            _workerData = new UserData
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                FirstName = "Pavel",
                LastName = "Kostin",
                MiddleName = "Alexandrovich"
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
                .Returns(_director);

            _userMapperMock
                .Setup(x => x.Map(_workerData))
                .Returns(_worker);

            _expectedDepartments = new List<DepartmentResponse>
            {
                new DepartmentResponse
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
                .Returns(_expectedDepartments.First());

            _command = new FindDepartmentsCommand(_repositoryMock.Object, _departmentMapperMock.Object, _userMapperMock.Object, _requestClientMock.Object, null);
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
                .Returns(_brokerResponseMock.Object).Verifiable();
        }

        [Test]
        public void ShouldReturnDepartmentListSuccessfully()
        {
            _command.Execute();
            //SerializerAssert.AreEqual(_expectedDepartments, _command.Execute());

            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Once);
            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default).Result, Times.Once);

        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrowsException()
        {
            _repositoryMock
                .Setup(repository => repository.FindDepartments())
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
        }

        [Test]
        public void ShouldReturnDataWhenRequestClientThrowsException()
        {
            _requestClientMock
                .Setup(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(
                    It.IsAny<object>(), default, default))
                .Throws(new Exception());

            Assert.True(_command.Execute().Count != 0);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserMapperThrowsExceptionOnDirector()
        {
            _userMapperMock
                .Setup(x => x.Map(_directorData))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute());
        }

        [Test]
        public void ShouldThrowExceptionWhenUserMapperThrowsExceptionOnWorker()
        {
            _userMapperMock
                .Setup(x => x.Map(_workerData))
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
