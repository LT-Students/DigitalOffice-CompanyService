using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class BaseDepartmentInfoValidatorTests
    {
        private IValidator<BaseDepartmentInfo> _validator;
        private BaseDepartmentInfo _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new BaseDepartmentInfoValidator();

            _request = new BaseDepartmentInfo
            {
                DirectorUserId = Guid.NewGuid(),
                Name = "Department A",
                Description = "Description"
            };
        }

        [Test]
        public void SuccessfulCompanyValidation()
        {
            _validator.TestValidate(_request).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void FailValidationEmptyName()
        {
            var name = string.Empty;

            _validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationTooLongName()
        {
            var name = "Department" + new string('a', 100);
            _validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationEmptyDescription()
        {
            var description = string.Empty;

            _validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }

        [Test]
        public void FailValidationTooLongDescription()
        {
            var description = "Positiion" + new string('a', 1000);
            _validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}