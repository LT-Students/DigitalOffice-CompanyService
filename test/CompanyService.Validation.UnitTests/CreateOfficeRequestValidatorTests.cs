using FluentValidation.TestHelper;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Validation.UnitTests
{
    public class CreateOfficeRequestValidatorTests
    {
        private ICreateOfficeRequestValidator _validator;
        private AutoMocker _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new();
            _validator = _mocker.CreateInstance<CreateOfficeRequestValidator>();
        }

        [Test]
        public void ShouldNotThrowValidationException()
        {
            Guid id = Guid.NewGuid();

            _mocker
                .Setup<ICompanyRepository, DbCompany>(x => x.Get())
                .Returns(new DbCompany { Id = id });

            CreateOfficeRequest request = new CreateOfficeRequest
            {
                CompanyId = id,
                Address = "Address",
                City = "City",
                Name = ""
            };

            _validator.ShouldNotHaveValidationErrorFor(x => x.CompanyId, id);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Address, "Address");
            _validator.ShouldNotHaveValidationErrorFor(x => x.City, "City");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, null as string);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, "");
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
        }

        [Test]
        public void ShouldThrowValidationException()
        {
            _mocker
                .Setup<ICompanyRepository, DbCompany>(x => x.Get())
                .Returns(new DbCompany { Id = Guid.NewGuid() });

            _validator.ShouldHaveValidationErrorFor(x => x.CompanyId, Guid.NewGuid());
            _validator.ShouldHaveValidationErrorFor(x => x.Address, "");
            _validator.ShouldHaveValidationErrorFor(x => x.Address, null as string);
            _validator.ShouldHaveValidationErrorFor(x => x.City, "");
            _validator.ShouldHaveValidationErrorFor(x => x.City, null as string);
        }
    }
}
