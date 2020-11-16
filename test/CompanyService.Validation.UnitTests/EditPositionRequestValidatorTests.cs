using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class EditPositionRequestValidatorTests
    {
        private IValidator<EditPositionRequest> validator;
        private EditPositionRequest request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            validator = new EditPositionRequestValidator();
        }

        [SetUp]
        public void SetUp()
        {
            request = new EditPositionRequest
            {
                Id = Guid.NewGuid(),
                Info = new PositionInfo
                {
                    Name = "Position",
                    Description = "Description"
                }
            };
        }

        [Test]
        public void ShouldReturnIsValid()
        {
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldFailValidationEmptyPositionId()
        {
            request.Id = Guid.Empty;
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void ShouldFailValidationNameIsEmptyString()
        {
            request.Info.Name = string.Empty;
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Name);
        }

        [Test]
        public void ShouldFailValidationTooLongName()
        {
            request.Info.Name = "Position" + new string('a', 100);
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Name);
        }

        [Test]
        public void ShouldFailValidationDescriptionIsEmptyString()
        {
            request.Info.Description = string.Empty;
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Description);
        }

        [Test]
        public void ShouldFailValidationTooLongDescription()
        {
            request.Info.Description = "Position" + new string('a', 350);
            validator.TestValidate(request).ShouldHaveValidationErrorFor(x => x.Info.Description);
        }
    }
}
