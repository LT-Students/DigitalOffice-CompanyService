using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.ModelValidators;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests.ModelValidators
{
    public class PositionInfoValidatorTests
    {
        private IValidator<PositionInfo> validator;
        private PositionInfo request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new PositionInfoValidator();

            request = new PositionInfo
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
            var name = "Position" + new string('a', 100);
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
            var description = "Positiion" + new string('a', 350);
            validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}