using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Validation.Company;
using LT.DigitalOffice.CompanyService.Validation.Company.Interfaces;
using Moq.AutoMock;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    class CreateCompanyValidatorTest
    {
        private ICreateCompanyRequestValidator _validator;
        private AutoMocker _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _validator = _mocker.CreateInstance<CreateCompanyRequestValidator>();
        }

        [Test]
        public void ShouldNotThrowValidationException()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.CompanyName, "Name");

            _validator.ShouldNotHaveValidationErrorFor(x => x.PortalName, "Name");

            _validator.ShouldNotHaveValidationErrorFor(x => x.SiteUrl, "siteurl");
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.PortalName, "");
            _validator.ShouldHaveValidationErrorFor(x => x.PortalName, null as string);

            _validator.ShouldHaveValidationErrorFor(x => x.CompanyName, "");
            _validator.ShouldHaveValidationErrorFor(x => x.CompanyName, null as string);

            _validator.ShouldHaveValidationErrorFor(x => x.SiteUrl, "");
            _validator.ShouldHaveValidationErrorFor(x => x.SiteUrl, null as string);
        }
    }
}
