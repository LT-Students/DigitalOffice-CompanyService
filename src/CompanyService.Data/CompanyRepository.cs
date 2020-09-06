using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    public class CompanyRepository : ICompanyDataProvider
    {
        private ICompanyDataProvider provider;

        public CompanyRepository(ICompanyDataProvider provider)
        {
            this.provider = provider;
        }

        public DbCompany GetCompanyById(Guid companyId)
        {
            var dbCompany = provider.GetCompanies().FirstOrDefault(x => x.Id == companyId);
            if (dbCompany == null)
            {
                throw new Exception("Company was not found.");
            }

            return dbCompany;
        }

        public Guid AddCompany(DbCompany company)
        {
            provider.GetCompanies().Add(company);
            provider.SaveChanges();

            return company.Id;
        }

        public List<DbCompany> GetCompaniesList()
        {
            return provider.Companies.ToList();
        }

        public bool UpdateCompany(DbCompany company)
        {
            var dbCompany = provider.Companies
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == company.Id);

            if (dbCompany == null)
            {
                throw new Exception("Company was not found.");
            }

            provider.Companies.Update(company);
            provider.SaveChanges();

            return true;
        }
    }
}
