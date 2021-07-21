using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.Kernel.Responses;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office
{
    public class FindOfficesCommand : IFindOfficesCommand
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IOfficeInfoMapper _mapper;

        public FindOfficesCommand(
            IOfficeRepository officeRepository,
            IOfficeInfoMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public FindResultResponse<OfficeInfo> Execute(int skipCount, int takeCount, bool? includeDeactivated)
        {
            return new()
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _officeRepository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(o => _mapper.Map(o)).ToList(),
                TotalCount = totalCount
            };
        }
    }
}
