using CompanyService.Mappers;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests
{
    public class DepartmentMapperTests
    {
        private IMapper<DepartmentRequest, DbDepartment> departmentRequestMapper;
        private DepartmentRequest departmentRequest;
        private DbDepartment expectedDbDepartment;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            departmentRequestMapper = new DepartmentMapper();
            departmentRequest = new DepartmentRequest
            {
                Name = "Department",
                Description = "Description",
                CompanyId = Guid.NewGuid(),
                UsersIds = new List<Guid>() { Guid.NewGuid()}
            };
            expectedDbDepartment = new DbDepartment
            {
                Name = "Department",
                Description = "Description",
                IsActive = true,
                Users = new List<DbDepartmentUser>()
                {
                    new DbDepartmentUser
                    {
                        UserId = departmentRequest.UsersIds.ElementAt(0),
                        IsActive = true
                    }
                }
            };
        }

        #region DepartmentRequest to DbDepartment
        [Test]
        public void ShouldThrowArgumentNullExceptionWhenDepartmentRequestIsNull()
        {
            Assert.Throws<BadRequestException>(() => departmentRequestMapper.Map(null));
        }

        [Test]
        public void ShouldReturnRightModelWhenDepartmentRequestIsMapped()
        {
            var dbDepartment = departmentRequestMapper.Map(departmentRequest);

            expectedDbDepartment.Id = dbDepartment.Id;
            expectedDbDepartment.Users.ElementAt(0).DepartmentId = dbDepartment.Id;
            expectedDbDepartment.Users.ElementAt(0).StartTime = dbDepartment.Users.ElementAt(0).StartTime;

            SerializerAssert.AreEqual(expectedDbDepartment, dbDepartment);
        }
        #endregion
    }
}
