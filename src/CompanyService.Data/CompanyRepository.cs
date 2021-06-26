using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDataProvider _provider;

        public CompanyRepository(
            IDataProvider provider)
        {
            _provider = provider;
        }

        public void Add(DbCompany company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            if (_provider.Companies.Any())
            {
                throw new BadRequestException("Company already exist");
            }

            _provider.Companies.Add(company);
            _provider.Save();
        }

        public DbCompany Get(bool full)
        {
            if (!_provider.Companies.Any())
            {
                throw new NotFoundException("Company doesn't exist");
            }

            if (full)
            {
                return _provider.Companies
                                .Include(c => c.Departments)
                                .Include(c => c.Positions)
                                .Include(c => c.Offices)
                                .First();
            }

            return _provider.Companies.First();
        }
    }
}
