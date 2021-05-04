using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Attributes;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for getting all added positions.
    /// </summary>
    [AutoInject]
    public interface IFindPositionsCommand
    {
        /// <summary>
        /// Returns all added positions.
        /// </summary>
        /// <returns>All added positions.</returns>
        List<PositionResponse> Execute();
    }
}
