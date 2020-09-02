using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Validators;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Validators
{
    public class AddCompanyValidatorTests
    {
        private IValidator<AddCompanyRequest> validator;

        private AddCompanyRequest request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new AddCompanyValidator();

            request = new AddCompanyRequest
            {
                Name = "Lanit-Tercom"
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
        public void ShouldHaveValidationErrorWhenNameIsTooLong()
        {
            var companyName = "Company" + new string('a', 100);
            validator.ShouldHaveValidationErrorFor(x => x.Name, companyName);
        }
    }
}
