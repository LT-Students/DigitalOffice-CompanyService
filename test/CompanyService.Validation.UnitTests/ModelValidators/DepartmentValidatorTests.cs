using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Validation.ModelValidators;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests.ModelValidators
{
    public class DepartmentValidatorTests
    {
        private IValidator<Department> validator;
        private Department request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new DepartmentValidator();

            request = new Department
            {
                DirectorUserId = Guid.NewGuid(),
                Name = "Department A",
                Description = "Description"
            };
        }

        [Test]
        public void SuccessfulCompanyValidation()
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
        public void FailValidationEmptyDescription()
        {
            var description = string.Empty;

            validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }

        [Test]
        public void FailValidationTooLongDescription()
        {
            var description = "Positiion" + new string('a', 1000);
            validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}