using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IAddPositionCommand"/>
    public class AddPositionCommand : IAddPositionCommand
    {
        private readonly IValidator<Position> validator;
        private readonly IPositionRepository repository;
        private readonly IMapper<Position, DbPosition> mapper;

        public AddPositionCommand(
            [FromServices] IValidator<Position> validator,
            [FromServices] IPositionRepository repository,
            [FromServices] IMapper<Position, DbPosition> mapper)
        {
            this.validator = validator;
            this.repository = repository;
            this.mapper = mapper;
        }

        public Guid Execute(Position request)
        {
            validator.ValidateAndThrowCustom(request);

            var position = mapper.Map(request);

            return repository.AddPosition(position);
        }
    }
}