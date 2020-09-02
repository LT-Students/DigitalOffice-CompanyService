using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Validators;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyServiceUnitTests.Validators
{
    public class EditPositionRequestValidatorTests
    {
        private IValidator<EditPositionRequest> validator;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new EditPositionRequestValidator();
        }

        [Test]
        public void ShouldReturnIsValid()
        {
            var request = new EditPositionRequest
            {
                Id = Guid.NewGuid(),
                Name = "Position",
                Description = "Description"
            };

            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldFailValidationEmptyPositionId()
        {
            var positionId = Guid.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Id, positionId);
        }

        [Test]
        public void ShouldFailValidationNameIsEmptyString()
        {
            var name = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void ShouldFailValidationTooLongName()
        {
            var positionName = "Position" + new string('a', 100);
            validator.ShouldHaveValidationErrorFor(x => x.Name, positionName);
        }

        [Test]
        public void ShouldFailValidationDescriptionIsEmptyString()
        {
            var description = string.Empty;
            validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }

        [Test]
        public void ShouldFailValidationTooLongDescription()
        {
            var description = "Position" + new string('a', 350);
            validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}
