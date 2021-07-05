using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.CompanyService.Validation.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IEditPositionCommand"/>
    public class EditPositionCommand : IEditPositionCommand
    {
        private readonly IPositionInfoValidator _validator;
        private readonly IPositionRepository _repository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDbPositionMapper _mapper;
        private readonly IAccessValidator _accessValidator;

        public EditPositionCommand(
            IPositionInfoValidator validator,
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

        public bool Execute(PositionInfo request)
        {
            if (!(_accessValidator.IsAdmin() ||
                  _accessValidator.HasRights(Rights.AddEditRemovePositions)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _validator.ValidateAndThrowCustom(request);

            var position = _mapper.Map(request, _companyRepository.Get().Id);

            return _repository.EditPosition(position);
        }
    }
}