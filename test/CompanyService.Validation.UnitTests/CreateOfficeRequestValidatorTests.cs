using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Validation.Office;
using LT.DigitalOffice.CompanyService.Validation.Office.Interfaces;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class CreateOfficeRequestValidatorTests
    {
        private ICreateOfficeRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateOfficeRequestValidator();
        }

        [Test]
        public void ShouldNotThrowValidationException()
        {
            Guid id = Guid.NewGuid();

            CreateOfficeRequest request = new CreateOfficeRequest
            {
                Address = "Address",
                City = "City",
                Name = ""
            };

            _validator.ShouldNotHaveValidationErrorFor(x => x.Address, "Address");

            _validator.ShouldNotHaveValidationErrorFor(x => x.City, "City");

            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, "");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, null as string);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Address, "");
            _validator.ShouldHaveValidationErrorFor(x => x.Address, null as string);

            _validator.ShouldHaveValidationErrorFor(x => x.City, "");
            _validator.ShouldHaveValidationErrorFor(x => x.City, null as string);
        }
    }
}
