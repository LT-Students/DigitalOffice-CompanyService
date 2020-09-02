using LT.DigitalOffice.CompanyService.Models;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for changing a company.
    /// </summary>
    public interface IEditCompanyCommand
    {
        /// <summary>
        /// Find and update company. Returns whether an update exited.
        /// </summary>
        /// <param name="request">New company data.</param>
        /// <returns>true if the data is up to date. Otherwise false.</returns>
        bool Execute(EditCompanyRequest request);
    }
}
