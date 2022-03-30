using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Business.Commands.ContractSubject.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.ContractSubject;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ContractSubjectController : ControllerBase
  {
    [HttpPost]
    public async Task<OperationResultResponse<Guid?>> CreateAsync(
      [FromServices] ICreateContractSubjectCommand command,
      [FromBody] CreateContractSubjectRequest request)
    {
      return await command.ExecuteAsync(request);
    }

    [HttpGet]
    public async Task<OperationResultResponse<List<ContractSubjectInfo>>> GetAsync(
      [FromServices] IGetContractSubjectsCommand command,
      [FromQuery] Guid companyId)
    {
      return await command.ExecuteAsync(companyId);
    }

    [HttpPatch("{contractSubjectId}")]
    public async Task<OperationResultResponse<bool>> EditAsync(
      [FromServices] IEditContractSubjectCommand command,
      [FromRoute] Guid contractSubjectId,
      [FromBody] JsonPatchDocument<EditContractSubjectRequest> request)
    {
      return await command.ExecuteAsync(contractSubjectId, request);
    }
  }
}
