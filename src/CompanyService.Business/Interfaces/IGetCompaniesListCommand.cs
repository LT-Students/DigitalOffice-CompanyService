using LT.DigitalOffice.CompanyService.Models;
using LT.DigitalOffice.CompanyService.Models.Dto;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
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
