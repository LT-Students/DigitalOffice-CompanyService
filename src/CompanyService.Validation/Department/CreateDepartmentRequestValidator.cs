﻿using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.CompanyService.Validation.Department.Interfaces;

namespace LT.DigitalOffice.CompanyService.Validation.Department
{
    public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>, ICreateDepartmentRequestValidator
    {
        public CreateDepartmentRequestValidator()
        {
            When(request => request.Users != null, () =>
            {
                RuleForEach(request => request.Users)
                    .NotEmpty();
            });

            When(request => request.DirectorUserId != null, () =>
            {
                RuleFor(request => request.DirectorUserId)
                    .NotEmpty()
                    .WithMessage("Director id can not be empty.");
            });

            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Department name can not be empty.")
                .MinimumLength(2)
                .WithMessage("Department name is too short")
                .MaximumLength(100)
                .WithMessage("Department name is too long.");

            When(request => request.Description != null, () =>
            {
                RuleFor(request => request.Description)
                    .MaximumLength(1000)
                    .WithMessage("Department description is too long.");
            });
        }
    }
}
