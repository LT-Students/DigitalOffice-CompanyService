using System;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using LT.DigitalOffice.CompanyService.Validation.Position.Interfaces;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IAddPositionCommand"/>
    public class CreatePositionCommand : ICreatePositionCommand
    {
        private readonly ICreatePositionRequestValidator _validator;
        private readonly IPositionRepository _repository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDbPositionMapper _mapper;
        private readonly IAccessValidator _accessValidator;

        public CreatePositionCommand(
            ICreatePositionRequestValidator validator,
            IPositionRepository repository,
            ICompanyRepository companyRepository,
            IDbPositionMapper mapper,
            IAccessValidator accessValidator)
        {
            _validator = validator;
            _repository = repository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _accessValidator = accessValidator;
        }

        public OperationResultResponse<Guid> Execute(CreatePositionRequest request)
        {
            if (!(_accessValidator.IsAdmin() ||
                  _accessValidator.HasRights(Rights.AddEditRemovePositions)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            if (!_validator.ValidateCustom(request, out List<string> errors))
            {
                return new OperationResultResponse<Guid>
                {
                    Status = OperationResultStatusType.BadRequest,
                    Errors = errors
                };
            }

            OperationResultResponse<Guid> response = new();

            if (_repository.DoesNameExist(request.Name))
            {
                response.Status = OperationResultStatusType.Conflict;
                response.Errors.Add("The position name already exists");
                return response;
            }

            response.Body = _repository.Create(_mapper.Map(request, _companyRepository.Get().Id));
            response.Status = OperationResultStatusType.FullSuccess;
            return response;
        }
    }
}