using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class DepartmentRequestValidatorTests
    {
        private IValidator<DepartmentRequest> validator;
        private DepartmentRequest request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new DepartmentRequestValidator();

            request = new DepartmentRequest
            {
                Name = "Position",
                Description = "Description",
                CompanyId = Guid.NewGuid()
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
            var name = string.Empty;

            validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationTooLongName()
        {
            var name = "Department" + new string('a', 100);
            validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationTooShortName()
        {
            var name = "D";
            validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationEmptyCompanyId()
        {
            var companyId = Guid.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.CompanyId, companyId);
        }
    }
}
