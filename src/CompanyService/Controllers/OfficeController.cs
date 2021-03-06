﻿using LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Office;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LT.DigitalOffice.CompanyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OfficeController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        public OperationResultResponse<Guid> Create(
            [FromServices] ICreateOfficeCommand command,
            [FromBody] CreateOfficeRequest request)
        {
            var result = command.Execute(request);
            if (result.Status != Kernel.Enums.OperationResultStatusType.Failed)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            }

            return result;
        }

        [HttpGet("find")]
        public FindOfficesResponse Find(
            [FromServices] IFindOfficesCommand command,
            [FromQuery] int skipCount,
            [FromQuery] int takeCount,
            [FromQuery] bool? includeDeactivated)
        {
            return command.Execute(skipCount, takeCount, includeDeactivated);
        }
    }
}
