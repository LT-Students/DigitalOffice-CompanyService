using LT.DigitalOffice.CompanyService.Business.Interfaces;
using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IAddPositionCommand"/>
    public class CreatePositionCommand : ICreatePositionCommand
    {
        private readonly IPositionValidator _validator;
        private readonly IPositionRepository _repository;
        private readonly IDbPositionMapper _mapper;
        private readonly IAccessValidator _accessValidator;

        public CreatePositionCommand(
            IPositionValidator validator,
            IPositionRepository repository,
            IDbPositionMapper mapper,
            IAccessValidator accessValidator)
        {
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
            _accessValidator = accessValidator;
        }

        public Guid Execute(Position request)
        {
            if (!(_accessValidator.IsAdmin() ||
                  _accessValidator.HasRights(Rights.AddEditRemovePositions)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            var position = _mapper.Map(request);

            return _repository.CreatePosition(position);
        }
    }
}