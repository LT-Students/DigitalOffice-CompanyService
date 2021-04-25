using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    internal class DepartmentUserInfoValidatorTests
    {
        private IValidator<DepartmentUserInfo> _validator;

        private DepartmentUserInfo _request;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _validator = new DepartmentUserInfoValidator();

            _request = new DepartmentUserInfo
            {
                UserId = Guid.NewGuid(),
                PositionId = Guid.NewGuid()
            };
        }

        [Test]
        public void ShouldFailValidationWhenUserIdIsEmpty()
        {
            var userId = Guid.Empty;

            _validator.ShouldHaveValidationErrorFor(x => x.UserId, userId);
        }

        [Test]
        public void ShouldFailValidationWhenPositionIdIsEmpty()
        {
            var positionId = Guid.Empty;

            _validator.ShouldHaveValidationErrorFor(x => x.PositionId, positionId);
        }

        [Test]
        public void SuccessfulDepartmentUserRequestWhenIsValid()
        {
            _validator.TestValidate(_request).ShouldNotHaveAnyValidationErrors();
        }
    }
}
