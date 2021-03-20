using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    internal class DepartmentRequestValidatorTests
    {
        private IValidator<NewDepartmentRequest> _validator;

        private NewDepartmentRequest _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new DepartmentRequestValidator();

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
        }

        [Test]
        public void ShouldFailValidationWhenListUsersIsEmpty()
        {
            List<DepartmentUser> users = new List<DepartmentUser>();

            _validator.ShouldHaveValidationErrorFor(x => x.Users, users);
        }

        [Test]
        public void ShouldFailValidationWhenListContainsNull()
        {
            List<DepartmentUser> users = new List<DepartmentUser>() { null };

            _validator.ShouldHaveValidationErrorFor(x => x.Users, users);
        }

        [Test]
        public void ShouldFailValidationWhenInfoIsNull()
        {
            Department departmentInfo = null;

            _validator.ShouldHaveValidationErrorFor(x => x.Info, departmentInfo);
        }

        [Test]
        public void SuccessfulWhenDepartmentRequestIsValid()
        {
            _validator.TestValidate(_request).ShouldNotHaveAnyValidationErrors();
        }
    }
}
