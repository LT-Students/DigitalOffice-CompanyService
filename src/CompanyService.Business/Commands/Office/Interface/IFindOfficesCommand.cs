﻿using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.CompanyService.Business.Commands.Office.Interface
{
  [AutoInject]
  public interface IFindOfficesCommand
  {
    FindResultResponse<OfficeInfo> Execute(BaseFindFilter filter);
  }
}
