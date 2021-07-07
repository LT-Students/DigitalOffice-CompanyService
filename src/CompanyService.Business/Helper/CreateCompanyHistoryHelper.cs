using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace LT.DigitalOffice.CompanyService.Business.Helper
{
    public class CreateCompanyHistoryHelper
    {
        ICompanyChangesRepository _repository;

        public CreateCompanyHistoryHelper(ICompanyChangesRepository repository)
        {
            _repository = repository;
        }

        public void AddChanges(DbCompany company)
        {
            _repository.Add(
                company.Id,
                null,
                CreateHistoryMessageHelper.Create(company));
        }

        public void AddChanges(DbCompany company, Guid changedBy, JsonPatchDocument<DbCompany> request)
        {
            _repository.Add(
                company.Id,
                changedBy,
                CreateHistoryMessageHelper.Create(company, request));
        }
    }
}
