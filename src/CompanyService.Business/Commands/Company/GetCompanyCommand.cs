using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Company
{
    public class GetCompanyCommand : IGetCompanyCommand
    {
        private readonly ICompanyRepository _repository;
        private readonly ICompanyInfoMapper _companyInfoMapper;

        public GetCompanyCommand(
            ICompanyRepository repository,
            ICompanyInfoMapper mapper)
        {
            _repository = repository;
            _companyInfoMapper = mapper;
        }

        public CompanyInfo Execute()
        {
            DbCompany company = _repository.Get(true);

            return _companyInfoMapper.Map(company, null);
        }
    }
}
