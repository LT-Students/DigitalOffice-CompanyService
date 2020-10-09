﻿using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    /// <inheritdoc cref="ICompanyRepository"/>
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDataProvider provider;

        public CompanyRepository(IDataProvider provider)
        {
            this.provider = provider;
        }

        public DbCompany GetCompanyById(Guid companyId)
        {
            var dbCompany = provider.Companies.FirstOrDefault(x => x.Id == companyId);
            if (dbCompany == null)
            {
                throw new NotFoundException("Company was not found.");
            }

            return dbCompany;
        }

        public Guid AddCompany(DbCompany company)
        {
            provider.Companies.Add(company);
            provider.Save();

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
                throw new NotFoundException("Company was not found.");
            }

            provider.Companies.Update(company);
            provider.Save();

            return true;
        }
    }
}
