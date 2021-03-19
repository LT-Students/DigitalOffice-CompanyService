using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for adding a new position.
    /// </summary>
    public interface ICreatePositionCommand
    {
        /// <summary>
        ///  Adds a new position. Returns its Id if it succeeded to add a position, otherwise Exception.
        /// </summary>
        /// <param name="request">Position data.</param>
        /// <returns>New position Id.</returns>
        /// <exception cref="ValidationException">Thrown when position data is incorrect.</exception>
        Guid Execute(Position request);
    }
}