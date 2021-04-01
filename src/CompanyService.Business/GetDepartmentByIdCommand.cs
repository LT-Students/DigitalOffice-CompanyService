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
        private readonly IDepartmentRepository _repository;
        private readonly IDepartmentMapper _mapper;
        private ILogger<GetDepartmentByIdCommand> _logger;

        public GetDepartmentByIdCommand(
            [FromServices] ILogger<GetDepartmentByIdCommand> logger,
            [FromServices] IDepartmentRepository repository,
            [FromServices] IDepartmentMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        public Department Execute(Guid departmentId)
        {
            return _mapper.Map(_repository.GetDepartment(departmentId));
        }
    }
}
