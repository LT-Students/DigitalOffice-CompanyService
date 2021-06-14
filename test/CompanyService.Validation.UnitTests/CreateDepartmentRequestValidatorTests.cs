using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    internal class CreateDepartmentRequestValidatorTests
    {
        private IValidator<CreateDepartmentRequest> _validator;

        private CreateDepartmentRequest _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new CreateDepartmentRequestValidator();

            var newDepartment = new BaseDepartmentInfo()
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid()
            };

            var newUsers = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _request = new CreateDepartmentRequest
            {
                Info = newDepartment,
                Users = newUsers
            };
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
