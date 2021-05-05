using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position
{
    /// <inheritdoc cref="IDisablePositionByIdCommand"/>
    public class DisablePositionByIdCommand : IDisablePositionByIdCommand
    {
        private readonly IPositionRepository _repository;
        private readonly IAccessValidator _accessValidator;

        public DisablePositionByIdCommand(
            IPositionRepository repository,
            IAccessValidator accessValidator)
        {
            _repository = repository;
            _accessValidator = accessValidator;
        }

        public void Execute(Guid positionId)
        {
            if (!(_accessValidator.IsAdmin() ||
                  _accessValidator.HasRights(Rights.AddEditRemovePositions)))
            {
                throw new ForbiddenException("Not enough rights.");
            }

            _repository.DisablePosition(positionId);
        }
    }
}