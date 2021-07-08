using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.CompanyService.Validation.Position;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class CreatePositionRequestValidatorTests
    {
        private IValidator<CreatePositionRequest> _validator;
        private CreatePositionRequest _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new CreatePositionRequestValidator();

            _request = new CreatePositionRequest
            {
                Name = "Position",
                Description = "Description"
            };
        }

        [Test]
        public void SuccessfulCompanyValidationWhenDescriptionIsNull()
        {
            var request = new CreatePositionRequest
            {
                Name = "Position",
                Description = null
            };

            _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
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
            var name = "Position" + new string('a', 100);
            _validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationTooLongDescription()
        {
            var description = "Positiion" + new string('a', 350);
            _validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}