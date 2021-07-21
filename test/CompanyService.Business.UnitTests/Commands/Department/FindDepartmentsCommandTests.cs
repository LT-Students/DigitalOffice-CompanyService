//using LT.DigitalOffice.CompanyService.Business.Commands.Department;
//using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
//using LT.DigitalOffice.CompanyService.Data.Interfaces;
//using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
//using LT.DigitalOffice.CompanyService.Models.Db;
//using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
//using LT.DigitalOffice.CompanyService.Models.Dto.Models;
//using LT.DigitalOffice.Kernel.Broker;
//using LT.DigitalOffice.UnitTestKernel;
//using MassTransit;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using LT.DigitalOffice.Models.Broker.Requests.User;
//using LT.DigitalOffice.Models.Broker.Responses.User;
//using LT.DigitalOffice.Models.Broker.Models;

//namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Department
//{
//    class FindDepartmentsCommandTest
//    {
//        private IFindDepartmentsCommand _command;
//        private Mock<IDepartmentRepository> _repositoryMock;
//        private Mock<IDepartmentInfoMapper> _departmentMapperMock;
//        private Mock<IUserInfoMapper> _userMapperMock;
//        private Mock<IRequestClient<IGetUsersDataRequest>> _requestClientMock;

//        private Mock<IGetUsersDataResponse> _responseMock;
//        private Mock<IOperationResult<IGetUsersDataResponse>> _operationResultMock;
//        private Mock<Response<IOperationResult<IGetUsersDataResponse>>> _brokerResponseMock;

//        private int _skipCount = 0;
//        private int _takeCount = 10;
//        private bool _includeDeactivated = true;
//        private int _totalCount = 4;
//        private Guid _directorGuid;
//        private Guid _workerGuid;
//        private UserData _directorData;
//        private UserData _workerData;
//        private DepartmentUserInfo _director;
//        private DepartmentUserInfo _worker;
//        private List<DbDepartment> _dbDepartments;
//        private List<DepartmentInfo> _expectedDepartments;
//        private FindDepartmentsResponse _expectedResponse;

//        [SetUp]
//        public void SetUp()
//        {
//            _repositoryMock = new Mock<IDepartmentRepository>();
//            _departmentMapperMock = new Mock<IDepartmentInfoMapper>();
//            _userMapperMock = new Mock<IUserInfoMapper>();

//            _directorGuid = Guid.NewGuid();
//            _workerGuid = Guid.NewGuid();

//            var directorDepartmentUser = new DbDepartmentUser { UserId = _directorGuid };
//            var workerDepartmentUser = new DbDepartmentUser { UserId = _workerGuid };

//            _dbDepartments = new List<DbDepartment>
//            {
//                new DbDepartment
//                {
//                    Id = Guid.NewGuid(),
//                    Name = "Name",
//                    Description = "TestDescription",
//                    IsActive = true,
//                    Users = new List<DbDepartmentUser> { directorDepartmentUser, workerDepartmentUser }
//                }
//            };

//            _repositoryMock
//                .Setup(x => x.FindDepartments(_skipCount, _takeCount, _includeDeactivated, out _totalCount))
//                .Returns(_dbDepartments)
//                .Verifiable();

//            _directorData = new UserData(_directorGuid, "Ivan", "Ivanovich", "Ivanov", true, null, null);

//            _workerData = new UserData(_workerGuid, "Arsen", "Arsenovich", "Ivanov", true, null, null);

//            BrokerSetUp();

//            _director = new DepartmentUserInfo
//            {
//                FirstName = _directorData.FirstName,
//                LastName = _directorData.LastName,
//                MiddleName = _directorData.MiddleName
//            };

//            _worker = new DepartmentUserInfo
//            {
//                FirstName = _workerData.FirstName,
//                LastName = _workerData.LastName,
//                MiddleName = _workerData.MiddleName
//            };

//            _userMapperMock
//                .Setup(x => x.Map(_directorData))
//                .Returns(_director)
//                .Verifiable();

//            _userMapperMock
//                .Setup(x => x.Map(_workerData))
//                .Returns(_worker)
//                .Verifiable();

//            _expectedDepartments = new List<DepartmentInfo>
//            {
//                new DepartmentInfo
//                {
//                    Id = _dbDepartments.First().Id,
//                    Name = _dbDepartments.First().Name,
//                    Description = _dbDepartments.First().Description,
//                    Director = _director,
//                    CountUsers = 2
//                }
//            };

//            _departmentMapperMock
//                .Setup(x => x.Map(_dbDepartments.First(), _director))
//                .Returns(new List<DepartmentUserInfo> { _director, _worker })
//                .Verifiable();

//            _command = new FindDepartmentsCommand(_repositoryMock.Object, _departmentMapperMock.Object, _userMapperMock.Object, _requestClientMock.Object, null);

//            _expectedResponse = new FindDepartmentsResponse
//            {
//                TotalCount = _totalCount,
//                Departments = _expectedDepartments,
//                Errors = new()
//            };
//        }

//        private void BrokerSetUp()
//        {
//            _responseMock = new Mock<IGetUsersDataResponse>();

//            _responseMock
//                .Setup(x => x.UsersData)
//                .Returns(new List<UserData> { _directorData, _workerData });

//            _operationResultMock = new Mock<IOperationResult<IGetUsersDataResponse>>();
//            _operationResultMock
//                .Setup(x => x.Body)
//                .Returns(_responseMock.Object);
//            _operationResultMock
//                .Setup(x => x.IsSuccess)
//                .Returns(true);
//            _operationResultMock
//                .Setup(x => x.Errors)
//                .Returns(new List<string>());

//            _brokerResponseMock = new Mock<Response<IOperationResult<IGetUsersDataResponse>>>();
//            _brokerResponseMock
//                .Setup(x => x.Message)
//                .Returns(_operationResultMock.Object);

//            _requestClientMock = new Mock<IRequestClient<IGetUsersDataRequest>>();
//            _requestClientMock
//                .Setup(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default).Result)
//                .Returns(_brokerResponseMock.Object)
//                .Verifiable();
//        }

//        [Test]
//        public void ShouldReturnDepartmentListSuccessfully()
//        {
//            SerializerAssert.AreEqual(_expectedResponse, _command.Execute(_skipCount, _takeCount, _includeDeactivated));

//            _repositoryMock.Verify(x => x.FindDepartments(_skipCount, _takeCount, _includeDeactivated, out _totalCount), Times.Once);
//            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Once);
//            _departmentMapperMock.Verify(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<UserInfo>(), It.IsAny<List<UserInfo>>()), Times.Once);
//            _userMapperMock.Verify(x => x.Map(It.IsAny<UserData>()), Times.Exactly(3));
//        }

//        [Test]
//        public void ShouldThrowExceptionWhenRepositoryThrowsException()
//        {
//            _repositoryMock
//                .Setup(repository => repository.FindDepartments(_skipCount, _takeCount, _includeDeactivated, out _totalCount))
//                .Throws(new Exception());

//            Assert.Throws<Exception>(() => _command.Execute(_skipCount, _takeCount, _includeDeactivated));
//            _repositoryMock.Verify(x => x.FindDepartments(_skipCount, _takeCount, _includeDeactivated, out _totalCount), Times.Once);
//            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Never);
//            _departmentMapperMock.Verify(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<UserInfo>(), It.IsAny<List<UserInfo>>()), Times.Never);
//            _userMapperMock.Verify(x => x.Map(It.IsAny<UserData>()), Times.Never);
//        }

//        [Test]
//        public void ShouldReturnDataWhenRequestClientThrowsException()
//        {
//            _requestClientMock
//                .Setup(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(
//                    It.IsAny<object>(), default, default))
//                .Throws(new Exception());

//            _expectedDepartments.First().Director = null;
//            _expectedDepartments.First().Users = null;

//            _departmentMapperMock
//                .Setup(x => x.Map(_dbDepartments.First(), null, new List<UserInfo>()))
//                .Returns(_expectedDepartments.First());

//            var result = _command.Execute(_skipCount, _takeCount, _includeDeactivated);
//            _expectedResponse.Errors = result.Errors;

//            SerializerAssert.AreEqual(_expectedResponse, result);
//            _requestClientMock.Verify(x => x.GetResponse<IOperationResult<IGetUsersDataResponse>>(It.IsAny<object>(), default, default), Times.Once);
//            _departmentMapperMock.Verify(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<UserInfo>(), It.IsAny<List<UserInfo>>()), Times.Once);
//            _userMapperMock.Verify(x => x.Map(It.IsAny<UserData>()), Times.Never);
//        }

//        [Test]
//        public void ShouldThrowExceptionWhenUserMapperThrowsException()
//        {
//            _userMapperMock
//                .Setup(x => x.Map(_directorData))
//                .Throws(new Exception());

//            Assert.Throws<Exception>(() => _command.Execute(_skipCount, _takeCount, _includeDeactivated));
//        }

//        [Test]
//        public void ShouldThrowExceptionWhenDepartmentMapperThrowsException()
//        {
//            _departmentMapperMock
//                .Setup(x => x.Map(It.IsAny<DbDepartment>(), It.IsAny<UserInfo>(), It.IsAny<List<UserInfo>>()))
//                .Throws(new Exception());

//            Assert.Throws<Exception>(() => _command.Execute(_skipCount, _takeCount, _includeDeactivated));
//        }
//    }
//}
