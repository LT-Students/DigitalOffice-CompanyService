﻿using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Data.Interfaces;

namespace LT.DigitalOffice.CompanyService.Business
{
    /// <inheritdoc cref="IGetPositionsListCommand"/>
    public class GetPositionsListCommand : IGetPositionsListCommand
    {
        private readonly IPositionRepository repository;
        private readonly IMapper<DbPosition, PositionResponse> mapper;

        public GetPositionsListCommand(
            [FromServices] IPositionRepository repository,
            [FromServices] IMapper<DbPosition, PositionResponse> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public List<PositionResponse> Execute()
        {
            return repository.GetPositionsList().Select(position => mapper.Map(position)).ToList();
        }
    }
}