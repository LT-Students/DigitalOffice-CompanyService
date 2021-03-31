﻿using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace LT.DigitalOffice.CompanyService.Business
{
    public class GetDepartmentByIdCommand : IGetDepartmentByIdCommand
    {
        private readonly IDepartmentRepository repository;
        private readonly IDepartmentMapper mapper;

        public GetDepartmentByIdCommand(
            [FromServices] ILogger logger,
            [FromServices] IDepartmentRepository repository,
            [FromServices] IDepartmentMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public Department Execute(Guid departmentId)
        {
            return mapper.Map(repository.GetDepartment(departmentId));
        }
    }
}
