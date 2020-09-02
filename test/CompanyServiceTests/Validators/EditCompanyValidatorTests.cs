using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Validators;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Validators
{
    public class EditCompanyValidatorTests
    {
        private IValidator<EditCompanyRequest> validator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new EditCompanyValidator();
        }

        [Test]
        public void ShouldSuccessfulCompanyValidationWhenEverythingIsCorrect()
        {
            var request1 = new EditCompanyRequest
            {
                CompanyId = Guid.NewGuid(),
                Name = "Lanit-Tercom"
            };

            var request2 = new EditCompanyRequest
            {
                CompanyId = Guid.NewGuid(),
                IsActive = false
            };

            validator.TestValidate(request1).ShouldNotHaveAnyValidationErrors();
            validator.TestValidate(request2).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldThrowValidationErrorWhenCompanyIdIsEmpty()
        {
            var companyId = Guid.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.CompanyId, companyId);
        }

        [Test]
        public void ShouldThrowValidationErrorWhenNameIsEmpty()
        {
            var name = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void ShouldThrowValidationErrorWhenNameTooLong()
        {
            var companyName = "Company" + new string('a', 100);
            validator.ShouldHaveValidationErrorFor(x => x.Name, companyName);
        }
    }
}
