using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class GetDepartmentByIdCommand : IGetDepartmentByIdCommand
    {
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentMapper _mapper;

        public GetDepartmentByIdCommand(
            IDepartmentRepository repository,
            IDepartmentMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public Department Execute(Guid departmentId)
        {
            return _mapper.Map(_repository.GetDepartment(departmentId, null));
        }
    }
}
