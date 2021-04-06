using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IAddPositionCommand"/>
    public class CreatePositionCommand : ICreatePositionCommand
    {
        private readonly IValidator<Position> validator;
        private readonly IPositionRepository repository;
        private readonly IDbPositionMapper mapper;

        public CreatePositionCommand(
            [FromServices] IValidator<Position> validator,
            [FromServices] IPositionRepository repository,
            [FromServices] IDbPositionMapper mapper)
        {
            this.validator = validator;
            this.repository = repository;
            this.mapper = mapper;
        }

        public Guid Execute(Position request)
        {
            validator.ValidateAndThrowCustom(request);

            var position = mapper.Map(request);

            return repository.CreatePosition(position);
        }
    }
}