using LT.DigitalOffice.CompanyService.Models.Dto;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for getting position model by id.
    /// </summary>
    public interface IGetPositionByIdCommand
    {
        /// <summary>
        /// Returns the position model with the specified id.
        /// </summary>
        /// <param name="positionId">Specified id of position.</param>
        /// <returns>Position model with specified id.</returns>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when position is not found.</exception>
        PositionResponse Execute(Guid positionId);
    }
}
