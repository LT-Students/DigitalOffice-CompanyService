using FluentValidation;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using System.Linq;
using LT.DigitalOffice.Kernel.Exceptions;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IAddPositionCommand"/>
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
            var validationResult = validator.Validate(request);

            if (validationResult != null && !validationResult.IsValid)
            {
                var messages = validationResult.Errors.Select(x => x.ErrorMessage);
                string message = messages.Aggregate((x, y) => x + "\n" + y);

                throw new BadRequestException(message);
            }

            var position = mapper.Map(request);

            return repository.AddPosition(position);
        }
    }
}