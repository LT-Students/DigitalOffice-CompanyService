using FluentValidation;
using LT.DigitalOffice.CompanyService.Commands.Interfaces;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Commands
{
    public class AddPositionCommand : IAddPositionCommand
    {
        private readonly IValidator<AddPositionRequest> validator;
        private readonly IPositionRepository repository;
        private readonly IMapper<AddPositionRequest, DbPosition> mapper;

        public AddPositionCommand(
            [FromServices] IValidator<AddPositionRequest> validator,
            [FromServices] IPositionRepository repository,
            [FromServices] IMapper<AddPositionRequest, DbPosition> mapper)
        {
            this.validator = validator;
            this.repository = repository;
            this.mapper = mapper;
        }

        public Guid Execute(AddPositionRequest request)
        {
            validator.ValidateAndThrow(request);

            var position = mapper.Map(request);

            return repository.AddPosition(position);
        }
    }
}