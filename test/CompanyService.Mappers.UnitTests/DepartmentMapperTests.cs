﻿using CompanyService.Mappers;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests
{
    internal class DepartmentMapperTests
    {
        private IMapper<NewDepartmentRequest, DbDepartment> _mapper;

        private NewDepartmentRequest _request;
        private DbDepartment _expectedDbDepartment;

        [OneTimeSetUp]
        public void OneTimeSeyUp()
        {
            _mapper = new DepartmentMapper();

            var newDepartment = new Department()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid()
            };

            var newUsers = new List<DepartmentUser>
            {
                new DepartmentUser
                {
                    UserId = Guid.NewGuid(),
                    PositionId = Guid.NewGuid()
                },
                new DepartmentUser
                {
                    UserId = Guid.NewGuid(),
                    PositionId = Guid.NewGuid()
                }
            };

            _request = new NewDepartmentRequest
            {
                Info = newDepartment,
                Users = newUsers
            };

            _expectedDbDepartment = new DbDepartment
            {
                Name = newDepartment.Name,
                Description = newDepartment.Description,
                IsActive = true,
                DirectorUserId = newDepartment.DirectorUserId,
                Users = new List<DbDepartmentUser>()
            };

            foreach (var newUser in newUsers)
            {
                _expectedDbDepartment.Users.Add(
                    new DbDepartmentUser
                    {
                        UserId = newUser.UserId,
                        PositionId = newUser.PositionId,
                        IsActive = true
                    });
            }
        }


        [Test]
        public void ShouldThrowArgumentNullExceptionWhenRequestIsNull()
        {
            NewDepartmentRequest request = null;

            Assert.Throws<ArgumentNullException>(() => _mapper.Map(request));
        }

        [Test]
        public void ShouldReturnDbDepartmentSuccessfully()
        {
            var dbDepartment = _mapper.Map(_request);

            _expectedDbDepartment.Id = dbDepartment.Id;

            for (int i = 0; i < dbDepartment.Users.Count; i++)
            {
                _expectedDbDepartment.Users.ElementAt(i).StartTime =
                    dbDepartment.Users.ElementAt(i).StartTime;

                _expectedDbDepartment.Users.ElementAt(i).Id =
                    dbDepartment.Users.ElementAt(i).Id;

                _expectedDbDepartment.Users.ElementAt(i).DepartmentId = dbDepartment.Id;
            }

            SerializerAssert.AreEqual(_expectedDbDepartment, dbDepartment);
        }
    }
}