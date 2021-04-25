using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using LT.DigitalOffice.CompanyService.Business.Commands.Department.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Department
{
    public class GetDepartmentByIdCommand : IGetDepartmentByIdCommand
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentInfoMapper _mapper;

        public GetDepartmentByIdCommand(
            IDepartmentRepository repository,
            IDepartmentInfoMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public DepartmentInfo Execute(Guid departmentId)
        {
            // TODO: update
            return _mapper.Map(_repository.GetDepartment(departmentId, null), null, new List<UserInfo>());
        }
    }
}
