using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for editing a position.
    /// </summary>
    [AutoInject]
    public interface IEditPositionCommand
    {
        /// <summary>
        ///  Edits an existing position. Returns true if it succeeded to edit a position, otherwise false.
        /// </summary>
        /// <param name="request">Position data.</param>
        /// <returns>Whether it was successful to edit.</returns>
        /// <exception cref="ValidationException">Thrown when position data is incorrect.</exception>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when position is not found.</exception>
        bool Execute(PositionInfo request);
    }
}