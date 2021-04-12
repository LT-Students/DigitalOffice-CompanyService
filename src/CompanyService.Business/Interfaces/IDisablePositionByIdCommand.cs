using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for deleting a position.
    /// </summary>
    [AutoInject]
    public interface IDisablePositionByIdCommand
    {
        /// <summary>
        /// Deletes an existing position. Returns nothing if it succeeded to delete a position, otherwise Exception.
        /// </summary>
        /// <param name="positionId">Position Id.</param>
        /// <returns>Returns nothing if it succeeded to delete a position, otherwise Exception.</returns>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when position is not found.</exception>
        void Execute(Guid positionId);
    }
}