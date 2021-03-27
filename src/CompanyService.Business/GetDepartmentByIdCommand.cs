using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class GetDepartmentByIdCommand : IGetDepartmentByIdCommand
    {
        private readonly IDepartmentRepository repository;
        private readonly IDepartmentResponseMapper mapper;

        public GetDepartmentByIdCommand(
            [FromServices] IDepartmentRepository repository,
            [FromServices] IDepartmentResponseMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public Department Execute(Guid departmentId)
            => mapper.Map(repository.GetDepartment(departmentId));
    }
}
