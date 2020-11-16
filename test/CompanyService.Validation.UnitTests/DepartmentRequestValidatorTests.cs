using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.RequestValidators;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class DepartmentRequestValidatorTests
    {
        private IValidator<NewDepartmentRequest> validator;
        private NewDepartmentRequest request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new DepartmentRequestValidator();
        }

        [SetUp]
        public void SetUp()
        {
            request = new NewDepartmentRequest
            {
                UsersIds = new List<Guid> { Guid.NewGuid() },
                Info = new Department
                {
                    Name = "Position",
                    Description = "Description"
                }
            };
        }

        [Test]
        public void SuccessfullyValidateDepartmentRequest()
        {
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void FailValidationEmptyName()
        {
            request.Info.Name = string.Empty;
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Name);
        }

        [Test]
        public void FailValidationTooLongName()
        {
            request.Info.Name = "Department" + new string('a', 100);
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Name);
        }

        [Test]
        public void FailValidationTooShortName()
        {
            request.Info.Name = "D";
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Name);
        }
    }
}
