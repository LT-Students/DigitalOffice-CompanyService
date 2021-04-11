using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using System;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IDisablePositionByIdCommand"/>
    public class DisablePositionByIdCommand : IDisablePositionByIdCommand
    {
        private readonly IPositionRepository repository;

        public DisablePositionByIdCommand(IPositionRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(Guid positionId)
        {
            repository.DisablePositionById(positionId);
        }
    }
}