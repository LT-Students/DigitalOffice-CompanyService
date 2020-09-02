using LT.DigitalOffice.CompanyService.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting all added positions.
    /// </summary>
    public interface IGetPositionsListCommand
    {
        /// <summary>
        /// Returns all added positions.
        /// </summary>
        /// <returns>All added positions.</returns>
        List<Position> Execute();
    }
}
