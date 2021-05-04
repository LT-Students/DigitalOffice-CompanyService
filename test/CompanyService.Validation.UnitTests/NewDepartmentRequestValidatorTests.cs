using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    internal class NewDepartmentRequestValidatorTests
    {
        private IValidator<NewDepartmentRequest> _validator;

        private NewDepartmentRequest _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new NewDepartmentRequestValidator();

            var newDepartment = new BaseDepartmentInfo()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid()
            };

            var newUsers = new List<DepartmentUserInfo>
            {
                new DepartmentUserInfo
                {
                    UserId = Guid.NewGuid(),
                    PositionId = Guid.NewGuid()
                },
                new DepartmentUserInfo
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
        }

        [Test]
        public void ShouldFailValidationWhenListUsersIsEmpty()
        {
            List<DepartmentUserInfo> users = new List<DepartmentUserInfo>();

            _validator.ShouldHaveValidationErrorFor(x => x.Users, users);
        }

        [Test]
        public void ShouldFailValidationWhenListContainsNull()
        {
            List<DepartmentUserInfo> users = new List<DepartmentUserInfo>() { null };

            _validator.ShouldHaveValidationErrorFor(x => x.Users, users);
        }

        [Test]
        public void ShouldFailValidationWhenInfoIsNull()
        {
            BaseDepartmentInfo departmentInfo = null;

            _validator.ShouldHaveValidationErrorFor(x => x.Info, departmentInfo);
        }

        [Test]
        public void SuccessfulWhenDepartmentRequestIsValid()
        {
            _validator.TestValidate(_request).ShouldNotHaveAnyValidationErrors();
        }
    }
}
