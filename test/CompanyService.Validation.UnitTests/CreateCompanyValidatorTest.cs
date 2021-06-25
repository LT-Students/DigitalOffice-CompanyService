using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using Moq.AutoMock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Logo, null as AddImageRequest);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Logo, new AddImageRequest());
            _validator.ShouldNotHaveValidationErrorFor(x => x.Description, "");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Description, null as string);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Description, "description");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Tagline, "tagline");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Tagline, "");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Tagline, null as string);
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, "");
            _validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
        }
    }
}
