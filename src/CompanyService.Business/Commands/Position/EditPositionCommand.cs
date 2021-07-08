using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using System;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.CompanyService.Validation.Position.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IEditPositionCommand"/>
    public class EditPositionCommand : IEditPositionCommand
    {
        private readonly IEditPositionRequestValidator _validator;
        private readonly IPositionRepository _repository;
        private readonly IPatchDbPositionMapper _mapper;
        private readonly IAccessValidator _accessValidator;

        public EditPositionCommand(
            IEditPositionRequestValidator validator,
            IPositionRepository repository,
            IPatchDbPositionMapper mapper,
            IAccessValidator accessValidator)
        {
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
            _accessValidator = accessValidator;
        }

        public OperationResultResponse<bool> Execute(Guid positionId, JsonPatchDocument<EditPositionRequest> request)
        {
            if (!(_accessValidator.IsAdmin() ||
                  _accessValidator.HasRights(Rights.AddEditRemovePositions)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            var result = _repository.Edit(positionId, _mapper.Map(request));

            return new OperationResultResponse<bool>
            {
                Status = result ? OperationResultStatusType.FullSuccess : OperationResultStatusType.Failed,
                Body = result
            };
        }
    }
}