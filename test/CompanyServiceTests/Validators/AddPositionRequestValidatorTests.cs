using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Validators;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Validators
{
    class AddPositionRequestValidatorTests
    {
        private IValidator<AddPositionRequest> validator;
        private AddPositionRequest request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new AddPositionRequestValidator();

            request = new AddPositionRequest
            {
                Name = "Position",
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
            var companyName = "Company" + new string('a', 100);
            validator.ShouldHaveValidationErrorFor(x => x.Name, companyName);
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
            var description = "Positiion" + new string('a', 350);
            validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}