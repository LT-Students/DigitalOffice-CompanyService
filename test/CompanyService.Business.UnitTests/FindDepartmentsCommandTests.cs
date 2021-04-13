using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.UnitTestKernel;
using LT.DigitalOffice.UserService.Models.Broker.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class FindDepartmentsCommandTest
    {
        private IFindDepartmentsCommand _command;
        private Mock<IDepartmentRepository> _repositoryMock;
        private Mock<IDepartmentResponseMapper> _departmentMapperMock;
        private Mock<IUserMapper> _userMapperMock;
        private Mock<IRequestClient<IGetUsersDataRequest>> _requestClient;

        private Guid _directorGuid;
        private Guid _workerGuid;
        private UserData _directorData;
        private UserData _workerData;
        private User _director;
        private User _worker;
        private List<DbDepartment> _dbDepartments;
        private List<DepartmentResponse> _expectedDepartments;

        private Guid _dbDepartmentId;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _departmentMapperMock = new Mock<IDepartmentResponseMapper>();
            _userMapperMock = new Mock<IUserMapper>();
            _requestClient = new Mock<IRequestClient<IGetUsersDataRequest>>();
            _command = new FindDepartmentsCommand(_repositoryMock.Object, _departmentMapperMock.Object, _userMapperMock.Object, _requestClient.Object, null);

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
        }

        // TODO
    }
}
