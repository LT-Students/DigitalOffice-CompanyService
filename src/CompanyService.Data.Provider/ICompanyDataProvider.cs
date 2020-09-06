using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Data.Provider
{
    public interface ICompanyDataProvider
    {
        /// <summary>
        /// Returns the company with the specified id from database.
        /// </summary>
        /// <param name="companyId">Specified id of company.</param>
        /// <returns>Company with specified id.</returns>
        DbCompany GetCompanyById(Guid companyId);

        /// <summary>
        /// Returns all added companies from database.
        /// </summary>
        /// <returns>List of all added companies.</returns>
        List<DbCompany> GetCompaniesList();

        /// <summary>
        /// Adds new company to the database. Returns the id of the added company.
        /// </summary>
        /// <param name="company">Company to add.</param>
        /// <returns>Id of the added company.</returns>
        Guid AddCompany(DbCompany company);

        /// <summary>
        /// Trying to update the company. Returns whether an update exited.
        /// </summary>
        /// <param name="company">Edited company model.</param>
        /// <returns>true if the company is up to date. Otherwise false.</returns>
        bool UpdateCompany(DbCompany company);
    }
}