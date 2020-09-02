using LT.DigitalOffice.CompanyService.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Commands.Interfaces
{
    /// <summary>
    /// Represents interface for a command in command pattern.
    /// Provides method for getting all companies.
    /// </summary>
    public interface IGetCompaniesListCommand
    {
        /// <summary>
        /// Returns all added companies.
        /// </summary>
        /// <returns>List of company models.</returns>
        List<Company> Execute();
    }
}
