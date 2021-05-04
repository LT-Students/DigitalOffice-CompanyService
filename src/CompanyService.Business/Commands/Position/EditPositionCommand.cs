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
        private readonly IDbPositionMapper _mapper;

        public EditPositionCommand(
            IPositionInfoValidator validator,
            IPositionRepository repository,
            IDbPositionMapper mapper)
        {
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
        }

        public bool Execute(PositionInfo request)
        {
            _validator.ValidateAndThrowCustom(request);

            var position = _mapper.Map(request);

            return _repository.EditPosition(position);
        }
    }
}