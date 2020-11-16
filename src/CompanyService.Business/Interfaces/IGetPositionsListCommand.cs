using LT.DigitalOffice.CompanyService.Models.Dto;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for getting all added positions.
    /// </summary>
    public interface IGetPositionsListCommand
    {
        /// <summary>
        /// Returns all added positions.
        /// </summary>
        /// <returns>All added positions.</returns>
        List<PositionResponse> Execute();
    }
}
