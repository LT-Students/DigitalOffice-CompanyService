using CompanyService.Mappers;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Exceptions;
using LT.DigitalOffice.Kernel.UnitTestLibrary;
using NUnit.Framework;
using System;

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
                CompanyId = Guid.NewGuid()
            };
            expectedDbDepartment = new DbDepartment
            {
                Name = "Department",
                Description = "Description",
                CompanyId = departmentRequest.CompanyId,
                IsActive = true
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

            SerializerAssert.AreEqual(expectedDbDepartment, dbDepartment);
        }
        #endregion
    }
}
