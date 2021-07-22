using LT.DigitalOffice.CompanyService.Business.Commands.Office;
using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Office
{
    class FindOfficeCommandTests
    {
        private AutoMocker _mocker;
        private IFindOfficesCommand _command;

        private int _totalCount;

        [SetUp]
        public void SetUp()
        {
            _totalCount = 5;

            _mocker = new();
            _command = _mocker.CreateInstance<FindOfficesCommand>();
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<IOfficeRepository, List<DbOffice>>(x => x.Find(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool?>(), out _totalCount))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(0, 10, true));
        }

        [Test]
        public void ShouldFindOfficesSuccessfuly()
        {
            int skipCount = 0;
            int takeCount = 10;

            var dbOffice1 = new DbOffice
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                City = "City",
                CompanyId = Guid.NewGuid(),
                Name = "Name"
            };

            var dbOffice2 = new DbOffice
            {
                Id = Guid.NewGuid(),
                Address = "Address1",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                City = "City1",
                CompanyId = Guid.NewGuid(),
                Name = "Name1"
            };

            var dbOffices = new List<DbOffice>()
            {
                dbOffice1,
                dbOffice2
            };

            var officeInfo1 = new OfficeInfo
            {
                Id = dbOffice1.Id,
                Address = dbOffice1.Address,
                City = dbOffice1.City,
                Name = dbOffice1.Name
            };

            var officeInfo2 = new OfficeInfo
            {
                Id = dbOffice2.Id,
                Address = dbOffice2.Address,
                City = dbOffice2.City,
                Name = dbOffice2.Name
            };

            _mocker
                .Setup<IOfficeRepository, List<DbOffice>>(x => x.Find(skipCount, takeCount, true, out _totalCount))
                .Returns(dbOffices);

            _mocker
                .Setup<IOfficeInfoMapper, OfficeInfo>(x => x.Map(dbOffice1))
                .Returns(officeInfo1);

            _mocker
                .Setup<IOfficeInfoMapper, OfficeInfo>(x => x.Map(dbOffice2))
                .Returns(officeInfo2);

            var expected = new FindResultResponse<OfficeInfo>
            {
                Status = OperationResultStatusType.FullSuccess,
                TotalCount = _totalCount,
                Body = new List<OfficeInfo>
                {
                    officeInfo1,
                    officeInfo2
                }
            };

            SerializerAssert.AreEqual(expected, _command.Execute(skipCount, takeCount, true));
        }
    }
}
