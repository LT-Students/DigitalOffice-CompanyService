﻿using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UnitTestKernel;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    public class CompanyInfoMapperTests
    {
        private ICompanyInfoMapper _mapper;
        private AutoMocker _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            _mapper = _mocker.CreateInstance<CompanyInfoMapper>();
        }

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenModelIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            var departament = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = "Dep",
                Description = "Desc",
                IsActive = true
            };

            var position = new DbPosition
            {
                Id = Guid.NewGuid(),
                Name = "Pos",
                Description = "Desc",
                IsActive = true
            };

            var office = new DbOffice
            {
                Id = Guid.NewGuid(),
                Name = "Pos",
                Address = "address",
                City = "city",
                IsActive = true
            };

            DbCompany company = new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Tagline = "Tagline",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                LogoId = Guid.NewGuid(),
                Departments = new List<DbDepartment>
                {
                    departament
                },
                Positions = new List<DbPosition>
                {
                    position
                },
                Offices = new List<DbOffice>
                {
                    office
                }
            };

            var expectedDepartament = new ShortDepartmentInfo
            {
                Id = departament.Id,
                IsActive = departament.IsActive,
                Description = departament.Description,
                Name = departament.Name
            };

            var expectedPosition = new PositionInfo
            {
                Id = position.Id,
                IsActive = position.IsActive,
                Description = position.Description,
                Name = position.Name
            };

            var expectedOffice = new OfficeInfo
            {
                Id = office.Id,
                City = office.City,
                Address = office.Address,
                Name = office.Name,
                CompanyId = office.CompanyId
            };

            _mocker
                .Setup<IShortDepartmentInfoMapper, ShortDepartmentInfo>(x => x.Map(departament))
                .Returns(expectedDepartament);

            _mocker
                .Setup<IPositionInfoMapper, PositionInfo>(x => x.Map(position))
                .Returns(expectedPosition);

            _mocker
                .Setup<IOfficeInfoMapper, OfficeInfo>(x => x.Map(office))
                .Returns(expectedOffice);

            ImageInfo imageInfo = new()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Content = "Content",
                Extension = "extension"
            };

            CompanyInfo expected = new()
            {
                Id = company.Id,
                Name = "Name",
                Tagline = "Tagline",
                Description = "Description",
                Logo = imageInfo,
                Departments = new List<ShortDepartmentInfo> { expectedDepartament },
                Positions = new List<PositionInfo> { expectedPosition },
                Offices = new List<OfficeInfo> { expectedOffice },
            };

            SerializerAssert.AreEqual(expected, _mapper.Map(company, imageInfo));
        }
    }
}
