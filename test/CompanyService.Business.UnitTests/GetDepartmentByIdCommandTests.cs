using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    class GetDepartmentByIdCommandTests
    {
        private Mock<IDepartmentRepository> _repositoryMock;
        private Mock<IDepartmentMapper> _mapperMock;
        private IGetDepartmentByIdCommand _command;
        private Mock<ILogger<IGetDepartmentByIdCommand>> _loggerMock;

        private DbDepartment _dbDepartment;
        private Department _expectedDepartment;

        private Guid _dbDepartmentId;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _mapperMock = new Mock<IDepartmentMapper>();
            _loggerMock = new Mock<ILogger<IGetDepartmentByIdCommand>>();
            _command = new GetDepartmentByIdCommand(_loggerMock.Object, _repositoryMock.Object, _mapperMock.Object);

            _dbDepartmentId = Guid.NewGuid();
            _dbDepartment = new DbDepartment()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "TestDescription",
                DirectorUserId = Guid.NewGuid(),
                IsActive = true,
                Users = null
            };

            _expectedDepartment = new Department
            {
                Id = _dbDepartment.Id,
                Name = _dbDepartment.Name,
                Description = _dbDepartment.Description,
                DirectorUserId = _dbDepartment.DirectorUserId
            };
        }

        [Test]
        public void ShouldThrowExceptionIfRepositoryThrowsIt()
        {
            _repositoryMock.Setup(x => x.GetDepartment(It.IsAny<Guid>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_dbDepartmentId));
            _repositoryMock.Verify(repository => repository.GetDepartment(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionIfMapperThrowsIt()
        {
            _mapperMock.Setup(x => x.Map(It.IsAny<DbDepartment>())).Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_dbDepartmentId));
            _repositoryMock.Verify(repository => repository.GetDepartment(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void ShouldReturnDepartmentSuccessfully()
        {
            _repositoryMock
                .Setup(x => x.GetDepartment(It.IsAny<Guid>()))
                .Returns(_dbDepartment);
            _mapperMock
                .Setup(x => x.Map(It.IsAny<DbDepartment>()))
                .Returns(_expectedDepartment);

            var result = _command.Execute(_dbDepartmentId);

            SerializerAssert.AreEqual(_expectedDepartment, result);
        }
    }
}
