﻿using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.Company.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class CompanyController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> Create(
      [FromServices] ICreateCompanyCommand command,
      [FromBody] CreateCompanyRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet("get")]
    public async Task<OperationResultResponse<CompanyResponse>> GetAsync(
      [FromServices] IGetCompanyCommand command,
      [FromQuery] GetCompanyFilter filter)
    {
      return await command.ExecuteAsync(filter);
    }

    [HttpPatch("edit")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditCompanyCommand command,
      [FromQuery] Guid companyId,
      [FromBody] JsonPatchDocument<EditCompanyRequest> request)
    {
      return await command.ExecuteAsync(companyId, request);
    }
  }
}
