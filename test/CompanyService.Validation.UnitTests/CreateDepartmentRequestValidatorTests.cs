using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    internal class CreateDepartmentRequestValidatorTests
    {
        private IValidator<CreateDepartmentRequest> _validator;

        private CreateDepartmentRequest _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new CreateDepartmentRequestValidator();

            var newUsers = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _request = new CreateDepartmentRequest
            {
                Name = "Department",
                Description = "Description",
                DirectorUserId = Guid.NewGuid(),
                Users = newUsers
            };
        }

        [Test]
        public void SuccessfulWhenDepartmentRequestIsValid()
        {
            _validator.TestValidate(_request).ShouldNotHaveAnyValidationErrors();
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
            var name = "Department" + new string('a', 100);
            _validator.ShouldHaveValidationErrorFor(x => x.Name, name);
        }

        [Test]
        public void FailValidationTooLongDescription()
        {
            var description = "Positiion" + new string('a', 1000);
            _validator.ShouldHaveValidationErrorFor(x => x.Description, description);
        }
    }
}
