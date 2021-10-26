using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Provider;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDataProvider _provider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IQueryable<DbCompany> CreateGetPredicates(
          GetCompanyFilter filter,
          IQueryable<DbCompany> dbCompanies)
        {

            if (filter.IsIncludeOffices)
            {
                dbCompanies = dbCompanies.Include(c => c.Offices.Where(o => o.IsActive));
            }

            return dbCompanies;
        }

        public CompanyRepository(
            IDataProvider provider,
            IHttpContextAccessor httpContextAccessor)
        {
            _provider = provider;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Add(DbCompany company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            if (_provider.Companies.Any())
            {
                throw new BadRequestException("Company already exists");
            }

            _provider.Companies.Add(company);
            _provider.Save();
        }

        public DbCompany Get(GetCompanyFilter filter = null)
        {
            if (filter == null)
            {
                return _provider.Companies.FirstOrDefault();
            }

            var dbUsers = _provider.Companies
                .AsSingleQuery()
                .AsQueryable();

            return CreateGetPredicates(filter, dbUsers).FirstOrDefault();
        }

        public void Edit(JsonPatchDocument<DbCompany> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var company = _provider.Companies.FirstOrDefault();

            if (company == null)
            {
                throw new NotFoundException("Company does not exist");
            }

            request.ApplyTo(company);
            company.ModifiedAtUtc = DateTime.UtcNow;
            company.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
            _provider.Save();
        }
    }
}
