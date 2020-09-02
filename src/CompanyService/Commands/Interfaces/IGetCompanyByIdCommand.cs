using LT.DigitalOffice.CompanyService.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting company model by id.
    /// </summary>
    public interface IGetCompanyByIdCommand
    {
        /// <summary>
        /// Returns the company model with the specified id.
        /// </summary>
        /// <param name="companyId">Specified id of company.</param>
        /// <returns>Company model with specified id.</returns>
        Company Execute(Guid companyId);
    }
}
