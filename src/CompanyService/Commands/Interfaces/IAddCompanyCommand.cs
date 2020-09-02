using LT.DigitalOffice.CompanyService.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for adding a new company.
    /// </summary>
    public interface IAddCompanyCommand
    {
        /// <summary>
        /// Adds a new company. Returns id of the added company.
        /// </summary>
        /// <param name="request">Company data.</param>
        /// <returns>Id of the added company.</returns>
        Guid Execute(AddCompanyRequest request);
    }
}
