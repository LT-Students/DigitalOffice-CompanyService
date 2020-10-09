using LT.DigitalOffice.CompanyService.Models.Dto;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Interfaces
{
    /// <summary>
    /// Represents the command pattern.
    /// Provides method for getting company model by id.
    /// </summary>
    public interface IGetCompanyByIdCommand
    {
        /// <summary>
        /// Returns the company model with the specified id.
        /// </summary>
        /// <param name="companyId">Specified id of company.</param>
        /// <returns>Company model with specified id.</returns>
        /// <exception cref="Kernel.Exceptions.NotFoundException">Thrown when company is not found.</exception>
        Company Execute(Guid companyId);
    }
}
