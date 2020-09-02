using LT.DigitalOffice.CompanyService.Database;
using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.CompanyService.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyServiceDbContext dbContext;

        public CompanyRepository(CompanyServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbCompany GetCompanyById(Guid companyId)
        {
            var dbCompany = dbContext.Companies.FirstOrDefault(x => x.Id == companyId);
            if (dbCompany == null)
            {
                throw new Exception("Company was not found.");
            }

            return dbCompany;
        }

        public Guid AddCompany(DbCompany company)
        {
            dbContext.Companies.Add(company);
            dbContext.SaveChanges();

            return company.Id;
        }

        public List<DbCompany> GetCompaniesList()
        {
            return dbContext.Companies.ToList();
        }

        public bool UpdateCompany(DbCompany company)
        {
            var dbCompany = dbContext.Companies
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == company.Id);

            if (dbCompany == null)
            {
                throw new Exception("Company was not found.");
            }

            dbContext.Companies.Update(company);
            dbContext.SaveChanges();

            return true;
        }
    }
}
