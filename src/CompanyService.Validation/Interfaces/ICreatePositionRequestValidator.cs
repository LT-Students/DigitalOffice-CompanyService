﻿using FluentValidation;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Validation.Interfaces
{
    [AutoInject]
    public interface ICreatePositionRequestValidator : IValidator<CreatePositionRequest>
    {
    }
}