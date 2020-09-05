using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class DisablePositionByIdCommand : IDisablePositionByIdCommand
    {
        private readonly IPositionRepository repository;

        public DisablePositionByIdCommand([FromServices] IPositionRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(Guid positionId)
        {
            repository.DisablePositionById(positionId);
        }
    }
}