using LT.DigitalOffice.CompanyService.Database.Entities;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers
{
    public class CompanyMapper : IMapper<DbCompany, Company>, IMapper<AddCompanyRequest, DbCompany>, IMapper<EditCompanyRequest, DbCompany>
    {
        public Company Map(DbCompany company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            return new Company
            {
                Id = company.Id,
                Name = company.Name,
                IsActive = company.IsActive
            };
        }

        public DbCompany Map(AddCompanyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbCompany
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                IsActive = true
            };
        }

        public DbCompany Map(EditCompanyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbCompany
            {
                Id = request.CompanyId,
                Name = request.Name,
                IsActive = request.IsActive
            };
        }
    }
}
