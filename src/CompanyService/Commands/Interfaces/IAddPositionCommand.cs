using LT.DigitalOffice.CompanyService.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding a new position.
    /// </summary>
    public interface IAddPositionCommand
    {
        /// <summary>
        ///  Adds a new position. Returns its Id if it succeeded to add a position, otherwise Exception.
        /// </summary>
        /// <param name="request">Position data.</param>
        /// <returns>New position Id.</returns>
        Guid Execute(AddPositionRequest request);
    }
}