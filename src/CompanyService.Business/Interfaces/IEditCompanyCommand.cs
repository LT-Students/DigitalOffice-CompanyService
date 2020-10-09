using LT.DigitalOffice.CompanyService.Models.Dto;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for changing a company.
    /// </summary>
    public interface IEditCompanyCommand
    {
        /// <summary>
        /// Find and update company. Returns whether an update exited.
        /// </summary>
        /// <param name="request">New company data.</param>
        /// <returns>true if the data is up to date. Otherwise false.</returns>
        /// <exception cref="ValidationException">Thrown when company data is incorrect.</exception>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when company is not found.</exception>
        bool Execute(EditCompanyRequest request);
    }
}
