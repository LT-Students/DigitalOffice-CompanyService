using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for editing a position.
    /// </summary>
    public interface IEditPositionCommand
    {
        /// <summary>
        ///  Edits an existing position. Returns true if it succeeded to edit a position, otherwise false.
        /// </summary>
        /// <param name="request">Position data.</param>
        /// <returns>Whether it was successful to edit.</returns>
        bool Execute(EditPositionRequest request);
    }
}