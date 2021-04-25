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

        public DisablePositionByIdCommand(
            IPositionRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Guid positionId)
        {
            _repository.DisablePosition(positionId);
        }
    }
}