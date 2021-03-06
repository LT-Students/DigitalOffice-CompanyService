﻿using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
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

        public FindOfficesResponse Execute(int skipCount, int takeCount, bool? includeDeactivated)
        {
            return new FindOfficesResponse
            {
                Offices = _officeRepository.Find(skipCount, takeCount, includeDeactivated, out int totalCount).Select(o => _mapper.Map(o)).ToList(),
                TotalCount = totalCount
            };
        }
    }
}
