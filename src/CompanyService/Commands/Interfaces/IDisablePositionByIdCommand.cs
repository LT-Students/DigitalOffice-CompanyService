using System;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for deleting a position.
    /// </summary>
    public interface IDisablePositionByIdCommand
    {
        /// <summary>
        ///  Deletes an existing position. Returns nothing if it succeeded to delete a position, otherwise Exception.
        /// </summary>
        /// <param name="positionId">Position Id.</param>
        /// <returns>Returns nothing if it succeeded to delete a position, otherwise Exception.</returns>
        void Execute(Guid positionId);
    }
}